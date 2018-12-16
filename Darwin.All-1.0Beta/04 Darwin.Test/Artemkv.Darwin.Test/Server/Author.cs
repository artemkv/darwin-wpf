using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Server
{
	public class Author : PersistentObject
	{
		private List<Book> _books = new List<Book>();

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public List<Book> Books
		{
			get
			{
				return _books;
			}
		}
	}
}
