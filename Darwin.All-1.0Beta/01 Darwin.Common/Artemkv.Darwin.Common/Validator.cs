using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Mapping;
using Artemkv.Darwin.Common.Validation;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;

namespace Artemkv.Darwin.Common
{
	public class Validator
	{
		public ValidationError Validate(PersistentObjectDTO dto)
		{
			ValidationError rootError = null;

			var simplePropertyErrors = ValidateSimpleProperties(dto);
			var collectionErrors = ValidateObjectCollections(dto);

			if (simplePropertyErrors.Count > 0 || collectionErrors.Count > 0)
			{
				rootError = new ValidationError(
					String.Format("Object (Type: '{0}', ID: '{1}') state is invalid.", dto.PersistentType.Name, dto.ID.ToString()));

				foreach (var error in simplePropertyErrors)
				{
					rootError.AddErrorAsNested(error);
				}
				foreach (var error in collectionErrors)
				{
					rootError.AddErrorAsNested(error);
				}
			}

			return rootError;
		}

		public ValidationError Validate(PersistentObjectDTO obj, string propertyName)
		{
			Type objType = obj.GetType();
			var propertyAsList = (from x in objType.GetProperties()
								  where (x.IsDefined(typeof(SimplePropertyAttribute), false) || x.IsDefined(typeof(ObjectPropertyAttribute), false)) &&
									x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)
								  select x).Take(1).ToList();
			if (propertyAsList.Count() < 1)
			{
				throw new InvalidOperationException(String.Format("Property {0} not found in type {1}.", propertyName, objType.FullName));
			}
			var propToValidate = propertyAsList[0];
			return Validate(obj, propToValidate);
		}

		#region Private Methods

		private List<ValidationError> ValidateSimpleProperties(PersistentObjectDTO obj)
		{
			var errors = new List<ValidationError>();

			Type objType = obj.GetType();
			var simpleProps = (from x in objType.GetProperties()
							   where x.IsDefined(typeof(SimplePropertyAttribute), false) ||
							   x.IsDefined(typeof(ObjectPropertyAttribute), false)
							   select x).ToList();

			foreach (var propToValidate in simpleProps)
			{
				var validationError = Validate(obj, propToValidate);
				if (validationError != null)
				{
					errors.Add(validationError);
				}
			}

			return errors;
		}

		private List<ValidationError> ValidateObjectCollections(PersistentObjectDTO obj)
		{
			var errors = new List<ValidationError>();

			Type objType = obj.GetType();
			var collectionProps = from x in objType.GetProperties()
								  where x.IsDefined(typeof(ObjectCollectionAttribute), false)
								  select x;
			foreach (var collectionProp in collectionProps)
			{
				var items = collectionProp.GetValue(obj, null) as IList;
				if (items == null)
					throw new InvalidOperationException(String.Format("DTO's object collection property {0} is not IList", collectionProp.GetType()));
				foreach (PersistentObjectDTO itemDTO in items)
				{
					var validationError = Validate(itemDTO);
					if (validationError != null)
					{
						errors.Add(validationError);
					}
				}
			}

			return errors;
		}

		private ValidationError Validate(PersistentObjectDTO obj, PropertyInfo propToValidate)
		{
			var propValue = propToValidate.GetValue(obj, null);

			var validationRules = (from x in propToValidate.GetCustomAttributes(typeof(ValidationRule), false)
								   select x).ToList();
			foreach (var rule in validationRules)
			{
				var regExpRule = rule as RegExpValidationRuleAttribute;
				if (regExpRule != null)
				{
					if (!regExpRule.IsValid(propValue))
					{
						return new ValidationError(String.Format("Property '{0}' has invalid value ({1}) ({2}).",
							propToValidate.Name, propValue == null ? "NULL" : "'" + propValue.ToString() + "'", regExpRule.ExpressionDescription));
					}
				}
				var notNullRule = rule as NotNullValidationRuleAttribute;
				if (notNullRule != null)
				{
					if (!notNullRule.IsValid(propValue))
					{
						return new ValidationError(String.Format("Property '{0}' value has to be set.", propToValidate.Name));
					}
				}
			}

			return null;
		}

		#endregion Private Methods
	}
}
