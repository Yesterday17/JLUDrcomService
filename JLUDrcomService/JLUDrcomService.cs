﻿using System;
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
        private RegistryKey jlu = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\JLUDrcomService");

        private String username;
        private String password;
        public JLUDrcomService()
        {
            // 初始化
            InitializeComponent();

            // 设置用户名密码
            username = ReadRegistry("username");
            password = ReadRegistry("password");
        }

        protected override void OnStart(string[] args)
        {
            // 调试用 手动创造断点
            System.Diagnostics.Debugger.Launch();

            // 使用用户名密码登录
            Network.Login(username, password);

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
