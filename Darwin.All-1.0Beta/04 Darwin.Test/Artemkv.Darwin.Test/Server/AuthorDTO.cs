using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Mapping;

namespace Artemkv.Darwin.Server
{
	public class AuthorDTO : PersistentObjectDTO
	{
		private List<BookDTO> _books = new List<BookDTO>();

		[SimpleProperty]
		public string FirstName { get; set; }
		[SimpleProperty]
		public string LastName { get; set; }

		[ObjectCollection(parentProperty: "Author")]
		public List<BookDTO> Books
		{
			get
			{
				return _books;
			}
		}

		public override Type PersistentType
		{
			get { return typeof(Author); }
		}
	}
}