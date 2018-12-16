using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.DTO;
using NHibernate;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.Mapping;
using Artemkv.Darwin.Common.Validation;

namespace Artemkv.Darwin.Server
{
	/// <summary>
	/// Provides functionality to assemble DTO from real object and back.
	/// </summary>
	public class Assembler
	{
		#region .Ctors

		public Assembler()
		{
		}

		public Assembler(bool bypassValidation)
		{
			this.BypassValidation = bypassValidation;
		}

		#endregion .Ctors

		#region Public Properties

		public bool BypassValidation { get; private set; }

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Returns DTO assembled from the real object.
		/// </summary>
		/// <param name="real">The real object.</param>
		/// <returns>DTO.</returns>
		public PersistentObjectDTO GetDTO(PersistentObject real)
		{
			if (real == null)
			{
				return null;
			}

			var handle = Activator.CreateInstance(
				typeof(PersistentObjectDTO).Assembly.FullName,
				typeof(PersistentObjectDTO).Namespace + "." + real.GetType().Name.Replace("Proxy", String.Empty) + "DTO"); // TODO: is there any better way to get the name?

			var dto = handle.Unwrap() as PersistentObjectDTO;

			dto.ID = real.ID;
			AssembleDTOFromRealObject(dto, real);

			return dto;
		}

		/// <summary>
		/// Returns real object assembled from the DTO.
		/// </summary>
		/// <param name="dto">DTO.</param>
		/// <param name="session">The real object.</param>
		/// <returns></returns>
		public PersistentObject GetRealObject(PersistentObjectDTO dto, ISession session)
        {
			if (dto == null)
				throw new ArgumentNullException("dto");
			if (session == null)
				throw new ArgumentNullException("session");

			// Insert or update only for the top level object.
			dynamic real = session.Get(dto.PersistentType, dto.ID); // TODO: eager load for the object collections
			bool isNew = false;
			if (real == null)
			{
				// Insert
				real = Activator.CreateInstance(dto.PersistentType);
				real.ID = dto.ID;
				isNew = true;
			}
			SetParent(real, dto, session);
			AssembleRealObjectFromDTO(real, dto, session, isNew);
			return real;
        }

		#endregion Public Methods

		#region Private Methods

		#region DTO From Real Object

		private void AssembleDTOFromRealObject(PersistentObjectDTO dto, PersistentObject real)
		{
			BuildDTOSimpleProperties(dto, real);
			BuildDTOCalculatedProperties(dto, real);
			BuildDTOObjectProperties(dto, real);
			BuildDTOObjectViewProperties(dto, real);
			BuildDTOParentObjectProperties(dto, real);
			BuildDTOObjectCollections(dto, real);
		}

		private void BuildDTOSimpleProperties(PersistentObjectDTO dto, PersistentObject real)
		{
			Type dtoType = dto.GetType();
			var simpleProps = from x in dtoType.GetProperties()
							  where x.IsDefined(typeof(SimplePropertyAttribute), false)
							  select x;
			foreach (var propDTO in simpleProps)
			{
				var propReal = real.GetType().GetProperty(propDTO.Name);
				propDTO.SetValue(dto, propReal.GetValue(real, null), null);
			}
		}

		private void BuildDTOCalculatedProperties(PersistentObjectDTO dto, PersistentObject real)
		{
			Type dtoType = dto.GetType();
			var calcProps = from x in dtoType.GetProperties()
							where x.IsDefined(typeof(CalculatedPropertyAttribute), false)
							select x;
			foreach (var propDTO in calcProps)
			{
				// No checks, since it's guaranteed that it is defined, and only once.
				var attr = propDTO.GetCustomAttributes(typeof(CalculatedPropertyAttribute), false)[0] as CalculatedPropertyAttribute;

				object currentObject = real;
				var propChain = attr.PropertyChain.Split('.');
				foreach (string propName in propChain)
				{
					var propReal = currentObject.GetType().GetProperty(propName);
					if (propReal == null)
					{
						throw new InvalidOperationException(String.Format("Object {0} does not have property {1}.", currentObject.GetType().ToString(), propName));
					}
					currentObject = propReal.GetValue(currentObject, null);
					if (currentObject == null)
					{
						break;
					}
				}

				propDTO.SetValue(dto, currentObject, null);
			}
		}

		private void BuildDTOObjectProperties(PersistentObjectDTO dto, PersistentObject real)
		{
			BuildDTOAnyObjectProperties(dto, real, typeof(ObjectPropertyAttribute));
		}

		private void BuildDTOParentObjectProperties(PersistentObjectDTO dto, PersistentObject real)
		{
			BuildDTOAnyObjectProperties(dto, real, typeof(ParentObjectAttribute));
		}

		private void BuildDTOAnyObjectProperties(PersistentObjectDTO dto, PersistentObject real, Type AttributeType)
		{
			Type dtoType = dto.GetType();
			var objectProps = from x in dtoType.GetProperties()
							  where x.IsDefined(AttributeType, false)
							  select x;
			foreach (var propDTO in objectProps)
			{
				// TODO: this just cuts an ID at the end.
				// In case more sophisticated logic is required, it should be possible to set name as a property of ObjectPropertyAttribute.
				var realPropName = propDTO.Name.Substring(0, propDTO.Name.Length - 2);
				var propReal = real.GetType().GetProperty(realPropName);
				var obj = propReal.GetValue(real, null) as PersistentObject;
				if (obj == null)
				{
					propDTO.SetValue(dto, Guid.Empty, null);
				}
				else
				{
					propDTO.SetValue(dto, obj.ID, null);
				}
			}
		}

		private void BuildDTOObjectViewProperties(PersistentObjectDTO dto, PersistentObject real)
		{
			Type dtoType = dto.GetType();
			var objectViewProps = from x in dtoType.GetProperties()
								  where x.IsDefined(typeof(ObjectViewPropertyAttribute), false)
								  select x;
			foreach (var propDTO in objectViewProps)
			{
				var propReal = real.GetType().GetProperty(propDTO.Name);
				var obj = propReal.GetValue(real, null) as PersistentObject;
				propDTO.SetValue(dto, GetDTO(obj), null);
			}
		}

		private void BuildDTOObjectCollections(PersistentObjectDTO dto, PersistentObject real)
		{
			Type dtoType = dto.GetType();
			var collectionProps = from x in dtoType.GetProperties()
							 where x.IsDefined(typeof(ObjectCollectionAttribute), false)
							 select x;
			foreach (var collectionProp in collectionProps)
			{
				PropertyInfo realCollectionProp = real.GetType().GetProperty(collectionProp.Name);

				var realItems = realCollectionProp.GetValue(real, null) as IList;
				if (realItems == null)
					throw new InvalidOperationException(String.Format("Real object's object collection property {0} is not IList", realCollectionProp.GetType()));

				var items = collectionProp.GetValue(dto, null) as IList;
				if (items == null)
					throw new InvalidOperationException(String.Format("DTO's object collection property {0} is not IList", collectionProp.GetType()));

				foreach (PersistentObject itemReal in realItems)
				{
					var itemDTO = GetDTO(itemReal);
					items.Add(itemDTO);
				}
			}
		}

		#endregion DTO From Real Object

		#region Real Object From DTO

		private void SetParent(dynamic real, PersistentObjectDTO dto, ISession session)
		{
			Type dtoType = dto.GetType();
			var parentProps = from x in dtoType.GetProperties()
							  where x.IsDefined(typeof(ParentObjectAttribute), false)
							  select x;
			foreach (var propDTO in parentProps)
			{
				// No checks, since it's guaranteed that it is defined, and only once.
				var attr = propDTO.GetCustomAttributes(typeof(ParentObjectAttribute), false)[0] as ParentObjectAttribute;

				var parentReal = session.Get(attr.ObjectType, propDTO.GetValue(dto, null));
				if (parentReal != null)
				{
					// TODO: this just cuts an ID at the end.
					// In case more sophisticated logic is required, it should be possible to set name as a property of ObjectPropertyAttribute.
					var realPropName = propDTO.Name.Substring(0, propDTO.Name.Length - 2);

					var propReal = real.GetType().GetProperty(realPropName);
					propReal.SetValue(real, parentReal, null);
				}
			}
		}

		private void AssembleRealObjectFromDTO(PersistentObject real, PersistentObjectDTO dto, ISession session, bool isNew)
        {
			if (!BypassValidation)
			{
				var error = new Validator().Validate(dto);
				if (error != null)
				{
					throw new ValidationException(error.ToString());
				}
			}

			BuildRealSimpleProperties(real, dto, session);
			BuildRealObjectProperties(real, dto, session);
			if (isNew)
			{
				session.Save(real);
			}
			BuildRealObjectCollections(real, dto, session);
		}

		private void BuildRealSimpleProperties(PersistentObject real, PersistentObjectDTO dto, ISession session)
		{
			Type dtoType = dto.GetType();
			var simpleProps = from x in dtoType.GetProperties()
						 where x.IsDefined(typeof(SimplePropertyAttribute), false)
						 select x;
			foreach (var propDTO in simpleProps)
			{
				var propReal = real.GetType().GetProperty(propDTO.Name);
				propReal.SetValue(real, propDTO.GetValue(dto, null), null);
			}
		}

		private void BuildRealObjectProperties(PersistentObject real, PersistentObjectDTO dto, ISession session)
		{
			// TODO: Consider scenario when real object does not exists but should be created from dto,
			// which is sent with the current dto, and can be found somewhere in the hierarchy.

			Type dtoType = dto.GetType();
			var objectProps = from x in dtoType.GetProperties()
							  where x.IsDefined(typeof(ObjectPropertyAttribute), false)
							  select x;
			foreach (var propDTO in objectProps)
			{
				// No checks, since it's guaranteed that it is defined, and only once.
				var attr = propDTO.GetCustomAttributes(typeof(ObjectPropertyAttribute), false)[0] as ObjectPropertyAttribute;

				// TODO: this just cuts an ID at the end.
				// In case more sophisticated logic is required, it should be possible to set name as a property of ObjectPropertyAttribute.
				var realPropName = propDTO.Name.Substring(0, propDTO.Name.Length - 2);

				var propReal = real.GetType().GetProperty(realPropName);
				// If object does not exist, assign null. If property is not nullable, we'll get a database error.
				var obj = session.Get(attr.ObjectType, propDTO.GetValue(dto, null));
				propReal.SetValue(real, obj, null);
			}
		}

		private void BuildRealObjectCollections(PersistentObject real, PersistentObjectDTO dto, ISession session)
		{
			Type dtoType = dto.GetType();
			var collectionProps = from x in dtoType.GetProperties()
							 where x.IsDefined(typeof(ObjectCollectionAttribute), false)
							 select x;
			foreach (var collectionProp in collectionProps)
			{
				// No checks, since it's guaranteed that it is defined, and only once.
				var attr = collectionProp.GetCustomAttributes(typeof(ObjectCollectionAttribute), false)[0] as ObjectCollectionAttribute;

				PropertyInfo realCollectionProp = real.GetType().GetProperty(collectionProp.Name);

				var realItems = realCollectionProp.GetValue(real, null) as IList;
				if (realItems == null)
					throw new InvalidOperationException(String.Format("Real object's object collection property {0} is not IList", realCollectionProp.GetType()));

				var items = collectionProp.GetValue(dto, null) as IList;
				if (items == null)
					throw new InvalidOperationException(String.Format("DTO's object collection property {0} is not IList", collectionProp.GetType()));

				var realItemsDictionary = new Dictionary<Guid, PersistentObject>();
				foreach (PersistentObject itemReal in realItems)
				{
					realItemsDictionary.Add(itemReal.ID, itemReal);
				}

				foreach (PersistentObjectDTO itemDTO in items)
				{
					dynamic itemReal;
					bool isNew = false;
					if (realItemsDictionary.ContainsKey(itemDTO.ID))
					{
						// Update in the collection
						itemReal = realItemsDictionary[itemDTO.ID];
						realItemsDictionary.Remove(itemDTO.ID);
					}
					else
					{
						// Look if exists detached
						itemReal = session.Get(itemDTO.PersistentType, itemDTO.ID);
						if (itemReal == null)
						{
							// Insert
							itemReal = Activator.CreateInstance(itemDTO.PersistentType);
							itemReal.ID = itemDTO.ID;
							isNew = true;
						}
						// Attach
						realItems.Add(itemReal);
					}

					// Update link to parent
					if (!String.IsNullOrWhiteSpace(attr.ParentProperty))
					{
						var parentProp = itemDTO.PersistentType.GetProperty(attr.ParentProperty);
						if (parentProp == null)
							throw new InvalidOperationException(
								String.Format("Object collection mapping error. Type {0} does not have property {1}",
								itemDTO.PersistentType.ToString(),
								attr.ParentProperty));
						parentProp.SetValue(itemReal, real, null);
					}

					AssembleRealObjectFromDTO(itemReal, itemDTO, session, isNew);
				}

				foreach (var objectReal in realItemsDictionary.Values)
				{
					// Detach
					realItems.Remove(objectReal);

					if (attr.DeletionBehavior == ObjectCollectionDeletionBehavior.Delete)
					{
						// Delete
						session.Delete(objectReal);
					}
				}
			}
		}

		#endregion Real Object From DTO

		#endregion Private Methods
	}
}
