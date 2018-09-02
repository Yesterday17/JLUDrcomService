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

        private Network client;
        public JLUDrcomService()
        {
            // 初始化
            InitializeComponent();
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\JLUDrcomService");
            jlu = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\JLUDrcomService");

            // 配置 client
            client = new Network(ReadRegistry("username"), ReadRegistry("password"));
        }

        protected override void OnStart(string[] args)
        {
            // 发送 StartRequest 包
            int retry = 0;
            while (!client.StartRequest(retry))
            {
                retry++;
            }

            // 使用用户名密码登录
            if (!client.LoginAuth())
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
            // 发送 Logout 包
            client.LogoutAuth();
        }

        protected override void OnPause()
        {
            this.Stop();
        }
        
        private void HeartBeat_Tick(object sender, EventArgs e)
        {
            // 发送心跳包
            client.HeartBeat();
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
