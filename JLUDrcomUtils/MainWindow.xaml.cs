using Microsoft.Win32;
using System;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Windows;

namespace JLUDrcomUtils
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceController service;
        RegistryKey jlu;

        public void StartService(bool msg = true)
        {
            service = new ServiceController("JLUDrcomService");

            if (service.Status == ServiceControllerStatus.Running || service.Status == ServiceControllerStatus.StartPending)
            {
                if (msg) MessageBox.Show("服务已处于启动状态！", "JLUDrcomUtils");
                return;
            }
            try
            {
                service.Start();
            }
            catch (Exception e)
            {
                if (msg) MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            if (msg) MessageBox.Show("服务启动成功！", "JLUDrcomUtils");
        }

        public void StopService(bool msg = true)
        {
            service = new ServiceController("JLUDrcomService");

            if (service.Status == ServiceControllerStatus.Stopped || service.Status == ServiceControllerStatus.StopPending)
            {
                if (msg) MessageBox.Show("服务已处于停止状态！", "JLUDrcomUtils");
                return;
            }
            try
            {
                service.Stop();
            }
            catch (Exception e)
            {
                if (msg) MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            if (msg) MessageBox.Show("服务停止成功！", "JLUDrcomUtils");
        }

        public void RestartService(bool msg = true)
        {
            service = new ServiceController("JLUDrcomService");

            try
            {
                if (service.Status == ServiceControllerStatus.Running) StopService(msg);
                StartService(msg);
            }
            catch (Exception e)
            {
                if (msg) MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            if (msg) MessageBox.Show("服务重启成功！", "JLUDrcomUtils");
        }

        public void UninstallService(bool msg = true)
        {
            service = new ServiceController("JLUDrcomService");

            try
            {
                if (service.Status == ServiceControllerStatus.Running) StopService(msg);

                ServiceInstaller installer = new ServiceInstaller();
                installer.Context = new InstallContext("JLUDrcomService_uninstall.log", null);
                installer.ServiceName = "JLUDrcomService";
                installer.Uninstall(null);
            }
            catch (Exception e)
            {
                if (msg) MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            if (msg) MessageBox.Show("服务卸载成功！", "JLUDrcomUtils");
        }

        public void UpdateSetting()
        {
            try
            {
                jlu.SetValue("username", this.txt_username.Text);
                jlu.GetValue("password", this.txt_username.Text);
                jlu.Flush();
                MessageBox.Show("配置成功！MAC地址暂不支持修改。", "JLUDrcomUtils");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "JLUDrcomUtils");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            jlu = Registry.CurrentConfig.CreateSubKey(@"SOFTWARE\JLUDrcomService", true);
            this.txt_username.Text = jlu.GetValue("username", "").ToString();
            this.txt_password.Password = jlu.GetValue("password", "").ToString();
            this.txt_mac.Text = jlu.GetValue("MAC", "").ToString();
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            StartService();
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            StopService();
        }

        private void btn_uninstall_Click(object sender, RoutedEventArgs e)
        {
            UninstallService();
        }

        private void btn_Start_Copy_Click(object sender, RoutedEventArgs e)
        {
            RestartService();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            UpdateSetting();
        }
    }
}
