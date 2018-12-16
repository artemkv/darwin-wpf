using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Controller
{
	public interface IDetailsView
	{
		PersistentObjectDTO Object { get; set; }
	}
}
