using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Visual.Primitives;

namespace Artemkv.Darwin.Visual.Connectors
{
	/// <summary>
	/// Represents the logical connection between two primitives.
	/// The connector direction is from FromPrimitive to ToPrimitive.
	/// </summary>
	public class Connector
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the logical end of the connector where connection begins.
		/// </summary>
		public Primitive From { get; set; }

		/// <summary>
		/// Gets or sets the logical end of the connector where connection ends.
		/// </summary>
		public Primitive To { get; set; }

		/// <summary>
		/// Gets or sets the connector type at the connector From end.
		/// </summary>
		public ConnectorType FromConnectorType { get; set; }

		/// <summary>
		/// Gets or sets the connector type at the connector To end.
		/// </summary>
		public ConnectorType ToConnectorType { get; set; }

		#endregion Public Properties
	}
}
