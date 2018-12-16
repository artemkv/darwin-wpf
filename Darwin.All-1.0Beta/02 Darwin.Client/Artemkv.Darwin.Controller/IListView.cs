using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using System.Collections;

namespace Artemkv.Darwin.Controller
{
	public interface IListView
	{
		IListDataSource DataSource { get; set; }
	}
}
