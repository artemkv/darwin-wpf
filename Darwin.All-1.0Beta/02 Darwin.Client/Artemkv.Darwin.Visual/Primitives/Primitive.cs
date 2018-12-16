using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Visual.Primitives
{
	[Serializable]
	public abstract class Primitive
	{
		#region Class Memebers

		private Guid _id = Guid.NewGuid();
		private double _x;
		private double _y;
		private double _width;
		private double _height;
		private Color _color = Colors.Black;
		private Color _bgColor = Colors.White;
		private Container _container;

		#endregion Class Memebers

		#region Internal Properties

		internal virtual double OwnWidth
		{
			get { return _width; }
		}

		internal virtual double OwnHeight
		{
			get { return _height; }
		}

		internal virtual Container Container
		{
			get { return _container; }
			set { _container = value; }
		}

		#endregion Internal Properties

		#region Public Properties

		public Guid Id
		{
			get { return _id; }
		}

		public virtual double X
		{
			get { return _x; }
			set { _x = value; }
		}

		public virtual double Y
		{
			get { return _y; }
			set { _y = value; }
		}

		public virtual double Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public virtual double Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public virtual Color Color
		{
			get { return _color; }
			set { _color = value; }
		}

		public virtual Color BgColor
		{
			get { return _bgColor; }
			set { _bgColor = value; }
		}

		public virtual PersistentObjectDTO Object { get; set; }

		#endregion Public Properties
	}
}
