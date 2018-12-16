using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Data;

namespace Artemkv.Darwin.Server
{
	/// <summary>
	/// Long-living objects storage.
	/// See the pattern description at: <see cref="http://artemkondratyev.net/createstoreget.htm"/>.
	/// </summary>
	internal class Registry
	{
		public DataProvider DataProvider { get; set; }
	}
}
