using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace JLUDrcomService
{
    public partial class JLUDrcomService : ServiceBase
    {
        private RegistryKey jlu;

        private String username;
        private String password;

        private byte[] tail = new byte[2];
        public JLUDrcomService()
        {
            // 初始化
            InitializeComponent();
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\JLUDrcomService");
            jlu = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\JLUDrcomService");

            // 设置用户名密码
            username = ReadRegistry("username");
            password = ReadRegistry("password");
        }

        protected override void OnStart(string[] args)
        {
            // 调试用 手动创造断点
            System.Diagnostics.Debugger.Launch();

            // 发送 Challenge 包
            int retry = 0;
            while (!Network.Challenge(retry))
            {
                retry++;
            }

            // 使用用户名密码登录
            if (!Network.Login(username, password))
            {
                this.Stop();
            }

            // 启动 heartbeat 发包
            HeartBeat.Enabled = true;
        }

        protected override void OnStop()
        {
            // 停止 heartbeat 发包
            HeartBeat.Enabled = false;
        }

        protected override void OnPause()
        {
            // 暂停 heartbeat 发包
            HeartBeat.Enabled = false;
        }

        protected override void OnContinue()
        {
            // 继续 heartbeat 发包
            HeartBeat.Enabled = false;
        }

        private void HeartBeat_Tick(object sender, EventArgs e)
        {
            Network.HeartBeat(tail);
        }

        private String ReadRegistry(String key)
        {
            if (jlu.GetValue(key) == null)
            {
                jlu.SetValue(key, "");
                return "";
            }
            return jlu.GetValue(key).ToString();
        }
    }
}
