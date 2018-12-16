using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.SqlServer;

using DarwinValidation = Artemkv.Darwin.Common.Validation;
using System.IO;
using Artemkv.Darwin.Common.DTO;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Windows.Controls;
using Artemkv.Darwin.Controller.Windows;
using Artemkv.Darwin.Controller.DataSources;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Areas
{
	public class ImportController
	{
		public void ProcessImportObjectsFromDatabase()
		{
			var database = new ProjectTreeHelper().GetFirstAncestor<DatabaseDTO>();
			if (database == null)
			{
				throw new InvalidOperationException("No database selected.");
			}
			if (String.IsNullOrWhiteSpace(database.ConnectionString))
			{
				// TODO: Should be application error, not InvalidOperationException.
				throw new InvalidOperationException("Database connection string is not set.");
			}

			var serviceLocator = ServiceLocator.GetActive();
			var busyIndicator = serviceLocator.SessionState.BusyIndicator;

			List<DarwinValidation.ValidationError> errors = null;

			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += (sender, e) =>
			{
				errors = DoImport(database);
			};
			worker.RunWorkerCompleted += (sender, e) =>
			{
				if (busyIndicator != null)
				{
					lock (this)
					{
						busyIndicator.IsBusy = false;
						// Re-throw async exception.
						if (e.Error != null)
						{
							throw e.Error;
						}
					}
				}

				ShowImportReport(errors);
			};
			worker.RunWorkerAsync();
			Thread.Sleep(100);
			lock (this)
			{
				if (busyIndicator != null && worker.IsBusy)
				{
					busyIndicator.IsBusy = true;
				}
			}
		}

		public bool CanImportObjectsFromDatabase
		{
			get
			{
				var database = new ProjectTreeHelper().GetFirstAncestor<DatabaseDTO>();
				if (database == null)
				{
					return false;
				}
				return true;
			}
		}

		#region Private Methods

		private void ShowImportReport(List<DarwinValidation.ValidationError> errors)
		{
			var serviceLocator = ServiceLocator.GetActive();

			StringBuilder sb = new StringBuilder(@"<?xml version=""1.0"" encoding=""utf-8""?>");
			sb.AppendLine("<ImportReport>");
			foreach (var error in errors)
			{
				sb.AppendLine(error.ToXmlString());
			}
			sb.AppendLine("</ImportReport>");

			var doc = new XmlDocument();
			doc.LoadXml(sb.ToString());
			string reportPath = Path.Combine(serviceLocator.LocalApplicationDataFolder, String.Format("importlog_{0}.xml", DateTime.Now.Ticks.ToString()));
			doc.Save(reportPath);

			WebBrowser browser = new WebBrowser();
			browser.Source = new Uri(reportPath);

			var popup = new PopupWindow();
			popup.Title = String.Format("Import Results ({0})", reportPath);
			popup.ViewPanel.Children.Add(browser);
			popup.ShowDialog();
		}

		private List<DarwinValidation.ValidationError> DoImport(DatabaseDTO database)
		{
			var entityIds = new Dictionary<string, Guid>();
			var attributeIds = new Dictionary<string, Guid>();

			var services = new SqlServerServices();
			var dataSource = new ObjectDataSource();
			var entities = new List<EntityDTO>();
			int lastTable = 0;

			var errors = new List<DarwinValidation.ValidationError>();
			var validator = new Validator();
			do
			{
				entities = services.ImportObjects(database, ref lastTable);
				dataSource.SaveObject(database);
				foreach (var entity in entities)
				{
					var validationError = validator.Validate(entity);
					if (validationError != null)
					{
						errors.Add(validationError);
					}

					dataSource.SaveObject(entity, bypassValidation: true);
					entityIds.Add(entity.EntitySchemaPrefixedName, entity.ID);
					foreach (var attribute in entity.Attributes)
					{
						attributeIds.Add(entity.EntitySchemaPrefixedName + "." + attribute.AttributeName, attribute.ID);
					}
				}
			} while (entities.Count > 0);

			List<RelationDTO> relations = new List<RelationDTO>();
			lastTable = 0;
			do
			{
				relations = services.ImportRelations(database, entityIds, attributeIds, ref lastTable);
				foreach (var relation in relations)
				{
					dataSource.SaveObject(relation);
				}
			} while (relations.Count > 0);

			return errors;
		}

		#endregion Private Methods
	}
}