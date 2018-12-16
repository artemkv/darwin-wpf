using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Controller
{
	public class Cardinality
	{
		private static List<Cardinality> Cardinalities = new List<Cardinality>()
		{
			new Cardinality() { OneToOne = false, CardinalityName = "Many" },
			new Cardinality() { OneToOne = true, CardinalityName = "One" }
		};

		public bool OneToOne { get; private set; }
		public string CardinalityName { get; private set; }

		public override string ToString()
		{
			return CardinalityName;
		}

		public static List<Cardinality> GetCardinalities()
		{
			return Cardinalities;
		}
	}
}
