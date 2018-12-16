using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common.TreePaths.ProjectTreePath;
using Artemkv.Darwin.Common.Validation;

namespace Artemkv.Darwin.Controller.DataSources
{
	public class ObjectDataSource
	{
		public PersistentObjectDTO GetObject(string objectType, Guid objectID)
		{
			using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
			{
				return proxy.GetObject(new GetObjectRequest(objectType, objectID)).Object;
			}
		}

		public PersistentObjectDTO GetObject(Type objectType, Guid objectID)
		{
			return GetObject(objectType.Name, objectID);
		}

		public void SaveObject(PersistentObjectDTO obj, bool bypassValidation = false)
		{
			using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
			{
				var response = proxy.SaveObject(new SaveObjectRequest(obj, bypassValidation));
				if (response.Error != null)
				{
					throw new InvalidOperationException(response.Error);
				}
			}
		}

		public void DeleteObject(PersistentObjectDTO obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
			{
				var response = proxy.DeleteObject(new DeleteObjectRequest(obj));
				if (response.Error != null)
				{
					throw new InvalidOperationException(response.Error);
				}
			}
		}
	}
}
