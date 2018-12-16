using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Artemkv.Darwin.Controller.Views;
using Microsoft.Windows.Controls;
using System.Collections.ObjectModel;
using Artemkv.Darwin.Common.Validation;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Controller
{
	/// <summary>
	/// Short-living objects storage.
	/// See the pattern description at: <see cref="http://artemkondratyev.net/createstoreget.htm"/>.
	/// </summary>
	public class SessionState
	{
		private ObservableCollection<ObjectPropertyValidationDetails> _validationErrors = new ObservableCollection<ObjectPropertyValidationDetails>();

		public DockPanel TreePanel { get; set; }
		public DockPanel DetailsPanel { get; set; }
		public ProjectTreeView ProjectTreeView { get; set; }
		public IDetailsView DetailsView { get; set; }
		public BusyIndicator BusyIndicator { get; set; }
		public ObservableCollection<ObjectPropertyValidationDetails> ValidationErrors { get { return _validationErrors; } }

		public PersistentObjectDTO ObjectInEditor { get; set; }
		public bool ObjectChanged { get; set; }
	}
}
