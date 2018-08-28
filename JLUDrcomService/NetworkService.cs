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
    public partial class NetworkService : ServiceBase
    {
        private RegistryKey jlu;
        public NetworkService()
        {
            InitializeComponent();
            jlu = Registry.CurrentUser.CreateSubKey("");
        }

        protected override void OnStart(string[] args)
        {
            String username = args.Length >= 1 ? args[0] : jlu.GetValue("drcomUsername").ToString();
            String password = args.Length >= 2 ? args[1] : jlu.GetValue("drcomPassword").ToString();
            Console.WriteLine("Logining with username: " + username + ", password(hash): " + password.GetHashCode());

            
        }

        protected override void OnStop()
        {
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }
    }
}
