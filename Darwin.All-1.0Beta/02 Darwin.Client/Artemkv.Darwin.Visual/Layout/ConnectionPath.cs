using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;

namespace Artemkv.Darwin.Visual.Layout
{
	/// <summary>
	/// Represents a physical path connecting two primitives, A and B, regardless of a logical connection direction.
	/// </summary>
	public class ConnectionPath
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets primitive at the beginning of the path.
		/// </summary>
		public Primitive PrimitiveA { get; set; }
		
		/// <summary>
		/// Gets or sets primitive at the end of the path.
		/// </summary>
		public Primitive PrimitiveB { get; set; }

		/// <summary>
		/// Gets or sets the side of the primitive from which the path begins.
		/// </summary>
		public Side SideA { get; set; }

		/// <summary>
		/// Gets or sets the side of the primitive on which the path ends.
		/// </summary>
		public Side SideB { get; set; }

		/// <summary>
		/// Gets or sets the connector type at the path beginning.
		/// </summary>
		public ConnectorType ConnectorTypeA { get; set; }

		/// <summary>
		/// Gets or sets the connector type at the path end.
		/// </summary>
		public ConnectorType ConnectorTypeB { get; set; }

		/// <summary>
		/// Gets the value which defines whether path begins and ends on the same primitive.
		/// </summary>
		public bool IsSelfReference
		{
			get
			{
				if (PrimitiveA != null &&
					PrimitiveB != null &&
					PrimitiveA.Id == PrimitiveB.Id)
				{
					return true;
				}
				return false;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return SideA.ToString() + "-" + SideB.ToString();
		}

		#endregion Public Methods
	}
}
