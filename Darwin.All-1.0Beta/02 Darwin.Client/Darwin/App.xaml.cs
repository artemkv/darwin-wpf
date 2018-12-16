using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Darwin
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var exceptionWindow = new ExceptionWindow()
			{
				Message = e.Exception.Message,
				Details = e.Exception.StackTrace
			};
			exceptionWindow.ShowDialog();
			e.Handled = true;
		}
	}
}
