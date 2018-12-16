using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Mapping;

namespace Artemkv.Darwin.Server
{
	public class BookDTO : PersistentObjectDTO
	{
		[SimpleProperty]
		public string Title { get; set; }
		[SimpleProperty]
		public int Year { get; set; }

		public Guid AuthorID { get; set; }

		public string FullTitle 
		{
			get
			{
				return Title + ", " + Year.ToString();
			}
		}

		public override Type PersistentType
		{
			get { return typeof(Book); }
		}
	}
}
