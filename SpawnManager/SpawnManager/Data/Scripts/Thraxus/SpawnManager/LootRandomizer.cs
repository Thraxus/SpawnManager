using System.Collections.Generic;
using Sandbox.Definitions;
using VRage.Collections;

namespace SpawnManager.SpawnManager
{
	public class LootRandomizer
	{


		public void AddLoot() //(IMyCargoContainer cargo)
		{
			//var definedComponents = MyDefinitionManager.Static.GetEntityComponentDefinitions<>();

			//List<MyDefinitionId> definedComponents = null;
			//MyDefinitionManager.Static.GetDefinedEntityComponents(ref definedComponents);
			ListReader<MyPhysicalItemDefinition> myPhysicalItemDefinitions = MyDefinitionManager.Static.GetPhysicalItemDefinitions();


			foreach (MyPhysicalItemDefinition comp in myPhysicalItemDefinitions)
			{
				Core.GeneralLog.WriteToLog("AddLoot", $"comp:\t{comp}\tcomp.Id:\t{comp.Id}\tcomp.Id.TypeId:\t{comp.Id.TypeId}\tcomp.Public:\t{comp.Public}\tcomp.Id.SubtypeName:\t{comp.Id.SubtypeName}\tcomp.Volume:\t{comp.Volume}\tcomp.Mass:\t{comp.Mass}\tcomp.MaxStackAmount:\t{comp.MaxStackAmount}");
			}
		}

		private readonly List<string> _componentSubtypeNames = new List<string>()
		{
			"Construction", "Canvas", "PowerCell", "SolarCell", "Explosives", "Detector",
			"RadioCommunication", "Medical", "GravityGenerator", "Thrust", "Reactor",
			"Computer", "Superconductor", "BulletproofGlass", "Display", "Motor", "LargeTube",
			"SmallTube", "Girder", "SteelPlate", "InteriorPlate", "MetalGrid"
		};

		public enum CargoOptions
		{
			Add,
			Empty,
			Ignore,
			Replace
		}

		public struct InventoryObject
		{
			private readonly string _objectBuilder;
			private readonly string _subtypeName;
			private readonly double _volume;
			private readonly double _quantity;

			public InventoryObject(string objectBuilder, string subtypeName, double volume, double quantity)
			{
				_objectBuilder = objectBuilder;
				_subtypeName = subtypeName;
				_volume = volume;
				_quantity = quantity;
			}
		}

		public List<InventoryObject> VanillaLoot;
		public List<InventoryObject> ModdedLoot;
	}
}