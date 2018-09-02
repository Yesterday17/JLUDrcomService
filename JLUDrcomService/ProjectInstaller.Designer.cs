namespace JLUDrcomService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaler = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaler
            // 
            this.serviceProcessInstaler.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaler.Password = null;
            this.serviceProcessInstaler.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.Description = "吉林大学校园网登录的Windows服务版本实现。";
            this.serviceInstaller.DisplayName = "吉林大学校园网登录服务";
            this.serviceInstaller.ServiceName = "JLUDrcomService";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaler,
            this.serviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaler;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
    }
}