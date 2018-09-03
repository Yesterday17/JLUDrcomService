using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace JLUDrcomUtils
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            if (e.Args.Length == 0)
            {
                mainWindow.Show();
            }
            else
            {
                switch (e.Args[0])
                {
                    case "start":
                        mainWindow.StartService();
                        break;
                    case "uninstall":
                        mainWindow.UninstallService();
                        break;
                    case "stop":
                        mainWindow.StopService();
                        break;
                    case "restart":
                        mainWindow.RestartService();
                        break;
                }
                Application.Current.Shutdown();
            }
        }
    }
}
