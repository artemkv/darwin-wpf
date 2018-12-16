using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;


namespace Artemkv.Darwin.DarwinService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}

		protected override void OnAfterInstall(System.Collections.IDictionary savedState)
		{
			base.OnAfterInstall(savedState);
			using (var serviceController = new ServiceController(this.DarwinServiceInstaller.ServiceName, Environment.MachineName))
			{
				serviceController.Start();
			}
		}

		// TODO: This crappy code is for custom setup steps, until I migrate to WIX
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			string targetDirectory = Context.Parameters["targetdir"];

			string serverName = Context.Parameters["ServerName"];
			string dbName = Context.Parameters["dbName"];

			var connectionStringCreateDB = string.Format(@"Data Source={0};Integrated Security=True", serverName);
			using (var connectionCreateDB = new SqlConnection(connectionStringCreateDB))
			{
				var commandCreateDB = connectionCreateDB.CreateCommand();
				commandCreateDB.CommandType = CommandType.Text;
				commandCreateDB.CommandText = String.Format(@"CREATE DATABASE [{0}];", dbName);
				connectionCreateDB.Open();
				commandCreateDB.ExecuteNonQuery();
				connectionCreateDB.Close();
			}

			var connectionString = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", serverName, dbName);
			using (var connection = new SqlConnection(connectionString))
			{
				var command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				connection.Open();
				
				string query = File.ReadAllText(string.Format(@"{0}createdbcomplete.sql", targetDirectory));
				string[] splitter = new string[] { "\r\nGO\r\n" };
				string[] commandTexts = query.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
				foreach (string commandText in commandTexts)
				{
					command.CommandText = commandText;
					command.ExecuteNonQuery();
				}
				connection.Close();
			}

			string exePath = string.Format(@"{0}Artemkv.Darwin.DarwinService.exe", targetDirectory);

			var config = ConfigurationManager.OpenExeConfiguration(exePath);
			config.ConnectionStrings.ConnectionStrings["Darwin"].ConnectionString = connectionString;
			config.Save();

			base.Install(stateSaver);
		}
	}
}
