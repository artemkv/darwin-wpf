using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.DTO;
using System.ComponentModel;
using System.Diagnostics;

namespace Artemkv.Darwin.Controller
{
	public class ObjectListItem : INotifyPropertyChanged
	{
		private bool _isSelected;

		public ObjectListItem(PersistentObjectDTO persistentObject)
		{
			if (persistentObject == null)
				throw new ArgumentNullException("persistentObject");

			PersistentObject = persistentObject;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public PersistentObjectDTO PersistentObject { get; private set; }

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}
	}
}
