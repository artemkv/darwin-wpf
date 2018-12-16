namespace Artemkv.Darwin.DarwinService
{
	partial class ProjectInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.DarwinServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.DarwinServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// DarwinServiceProcessInstaller
			// 
			this.DarwinServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.DarwinServiceProcessInstaller.Password = null;
			this.DarwinServiceProcessInstaller.Username = null;
			// 
			// DarwinServiceInstaller
			// 
			this.DarwinServiceInstaller.Description = "Darwin Service";
			this.DarwinServiceInstaller.DisplayName = "Darwin";
			this.DarwinServiceInstaller.ServiceName = "DarwinService";
			this.DarwinServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DarwinServiceProcessInstaller,
            this.DarwinServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller DarwinServiceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller DarwinServiceInstaller;
	}
}