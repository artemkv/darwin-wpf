using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Visual.Primitives
{
	[Serializable]
	public abstract class Container : Primitive
	{
		#region Class Members

		private List<Primitive> _items = new List<Primitive>();

		#endregion Class Members

		#region Internal Methods

		internal abstract double GetItemX(Primitive item);
		internal abstract double GetItemY(Primitive item);

		#endregion Internal Methods

		#region Public Properties

		public Primitive[] Items
		{
			get
			{
				Primitive[] items = new Primitive[_items.Count];
				_items.CopyTo(items);
				return items;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void AddItem(string name, Primitive item)
		{
			item.Container = this;
			_items.Add(item);
		}

		#endregion Public Methods
	}
}
