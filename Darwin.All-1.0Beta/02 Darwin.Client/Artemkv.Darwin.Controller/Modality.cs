using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Controller
{
	public class Modality
	{
		private static List<Modality> Modalities = new List<Modality>()
		{
			new Modality() { AtLeastOne = false, ModalityName = "Zero or" },
			new Modality() { AtLeastOne = true, ModalityName = "One or" }
		};

		public bool AtLeastOne { get; private set; }
		public string ModalityName { get; private set; }

		public override string ToString()
		{
			return ModalityName;
		}

		public static List<Modality> GetModalities()
		{
			return Modalities;
		}
	}
}
