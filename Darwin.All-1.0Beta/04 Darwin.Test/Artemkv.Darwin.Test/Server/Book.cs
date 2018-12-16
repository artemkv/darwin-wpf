using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Server
{
	public class Book : PersistentObject
	{
		public string Title { get; set; }
		public int Year { get; set; }

		public Author Author { get; set; }
	}
}
