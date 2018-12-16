using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Artemkv.Darwin.Services;

namespace Artemkv.Darwin.DarwinService
{
	partial class DarwinService : ServiceBase
	{
		private ServiceHost _host;

		public DarwinService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			_host = new ServiceHost(typeof(DarwinDataService));
			_host.Open();
		}

		protected override void OnStop()
		{
			if (_host != null)
			{
				_host.Close();
			}
		}
	}
}
