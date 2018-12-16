using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Validation
{
	public class ObjectPropertyValidationDetails
	{
		public ObjectPropertyValidationDetails(
			Type objectType,
			Guid objectID,
			string propertyName)
		{
			if (objectType == null)
				throw new ArgumentNullException("objectType");
			if (objectID == Guid.Empty)
				throw new ArgumentNullException("objectID");
			if (String.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName");

			this.ObjectType = objectType;
			this.ObjectID = objectID;
			this.PropertyName = propertyName;
		}

		public Type ObjectType { get; private set; }
		public Guid ObjectID { get; private set; }
		public string PropertyName { get; private set; }
		public string ErrorMessage { get; set; }

		public override string ToString()
		{
			return ErrorMessage;
		}

		public override bool Equals(object obj)
		{
			var another = obj as ObjectPropertyValidationDetails;
			if (another == null)
			{
				return false;
			}

			return (ObjectType == another.ObjectType &&
				ObjectID == another.ObjectID &&
				PropertyName.Equals(another.PropertyName, StringComparison.InvariantCulture));
		}

		public override int GetHashCode()
		{
			// Simplistic approach. If this object is going to be really used as a hash key, the method can be optimized.
			return (ObjectType.ToString() + ObjectID.ToString() + PropertyName).GetHashCode();
		}
	}
}
