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
        private JLUDrcomClient client;
        System.Timers.Timer HeartBeat = new System.Timers.Timer(20000);

        public JLUDrcomService()
        {
            // 初始化
            InitializeComponent();
            jlu = Registry.CurrentConfig.CreateSubKey(@"SOFTWARE\JLUDrcomService", true);

            // 配置 client
            client = new JLUDrcomClient(ReadRegistry("username"), ReadRegistry("password"));
            jlu.Flush();

            // 配置 Timer
            HeartBeat.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 发送心跳包
            client.HeartBeat();
        }

        protected override void OnStart(string[] args)
        {
            // Debug
            // System.Diagnostics.Debugger.Launch();

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
            HeartBeat.Start();

            // 手动执行一次心跳包
            client.HeartBeat();
        }

        protected override void OnStop()
        {
            // 停止 heartbeat 发包
            HeartBeat.Stop();
            // 发送 Logout 包
            client.LogoutAuth();
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
