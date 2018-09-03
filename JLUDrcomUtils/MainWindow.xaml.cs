using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JLUDrcomUtils
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceController service = new ServiceController("JLUDrcomService");

        public void StartService()
        {
            try
            {
                service.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            MessageBox.Show("服务启动成功！", "JLUDrcomUtils");
        }

        public void StopService()
        {
            try
            {
                service.Stop();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            MessageBox.Show("服务停止成功！", "JLUDrcomUtils");
        }

        public void RestartService()
        {
            try
            {
                StartService();
                StopService();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            MessageBox.Show("服务重启成功！", "JLUDrcomUtils");
        }

        public void UninstallService()
        {
            try
            {
                StopService();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "JLUDrcomUtils");
                return;
            }
            // TODO: Uninstall here.
            MessageBox.Show("服务卸载成功！", "JLUDrcomUtils");
        }

        public MainWindow()
        {
            InitializeComponent();
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
    }
}
