using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace SpawnManager.Support
{
	public static class CubeProcessing
	{
		public static void Close()
		{
			CubeBlockProcessing.Clear();
			PbPrograms.Clear();
			Core.GeneralLog.WriteToLog("CubeProcessing", "Done processing...");
		}

		public static readonly Dictionary<MyObjectBuilderType, Action<MyObjectBuilder_CubeBlock, Options, MyCubeSize>> CubeBlockProcessing = new Dictionary<MyObjectBuilderType, Action<MyObjectBuilder_CubeBlock, Options, MyCubeSize>>
		{
			{ typeof(MyObjectBuilder_BatteryBlock), ProcessBattery },
			{ typeof(MyObjectBuilder_Beacon), ProcessBeacon },
			{ typeof(MyObjectBuilder_CargoContainer), ProcessCargoContainer },
			{ typeof(MyObjectBuilder_ShipConnector), ProcessConnector },
			{ typeof(MyObjectBuilder_InteriorTurret), ProcessInteriorTurret },
			{ typeof(MyObjectBuilder_LargeGatlingTurret), ProcessLargeGatlingTurret },
			{ typeof(MyObjectBuilder_LargeMissileTurret), ProcessLargeMissileTurret },
			{ typeof(MyObjectBuilder_MyProgrammableBlock), ProcessMyProgrammableBlock },
			{ typeof(MyObjectBuilder_OxygenGenerator), ProcessOxygenGenerator },
			{ typeof(MyObjectBuilder_OxygenTank), ProcessOxygenTank },
			{ typeof(MyObjectBuilder_Parachute), ProcessParachute },
			{ typeof(MyObjectBuilder_Reactor), ProcessReactor },
			{ typeof(MyObjectBuilder_SmallGatlingGun), ProcessSmallGatlingGun },
			{ typeof(MyObjectBuilder_SmallMissileLauncher), ProcessSmallMissileLauncher },
			{ typeof(MyObjectBuilder_SmallMissileLauncherReload), ProcessSmallMissileLauncherReload },
		};

		public struct PbReplacement
		{
			public readonly string NewName;
			public readonly string OriginalName;
			public readonly string Program;


			public PbReplacement(string newName, string oldName, string program)
			{
				NewName = newName;
				OriginalName = oldName;
				Program = program;
			}
		}

		private static string TempPbName => $"EEMPbTemp{DateTime.Now:mmss.fffff}";

		public static Dictionary<long, List<PbReplacement>> PbPrograms = new Dictionary<long, List<PbReplacement>>();

		public static void GeneralGridSettings(MyObjectBuilder_CubeGrid grid, Options options)
		{
			grid.DestructibleBlocks = options.DestructableBlocks;
			grid.GridGeneralDamageModifier = options.GeneralDamageModifier;
			if (options.DisableDampners) grid.DampenersEnabled = false;
			if (options.ForceStatic) grid.IsStatic = true;
			if (options.ForcePhysics) grid.CreatePhysics = true;
			if (options.SetAngularVelocity) grid.AngularVelocity = options.AngularVelocity;
			if (options.SetLinearVelocity) grid.LinearVelocity = options.LinearVelocity;
		}

		public static void GeneralBlockSettings(MyObjectBuilder_CubeBlock block, Options options)
		{
			block.Owner = options.OwnerId;
			block.BuiltBy = options.BuiltBy;
			block.BlockGeneralDamageModifier = options.GeneralDamageModifier;
			if (options.SpawnDamaged) block.IntegrityPercent = (float) options.SpawmDamagedPercent;
			if (options.SpawnAsWireframe)
			{
				block.IntegrityPercent = 0.01f;
				block.BuildPercent = 0.01f;
			}
			if (options.SetFactionShareMode) block.ShareMode = options.FactionShareMode;
		}
		
		private static void ProcessBattery(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Doesn't need inventory processing, just set the current to the eventual max
			if (options.PowerDownGrid) ((MyObjectBuilder_BatteryBlock) block).Enabled = false;
			if (!options.Restock) return;
			try
			{
				((MyObjectBuilder_BatteryBlock)block).CurrentStoredPower = ((MyBatteryBlockDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(block.GetId())).MaxStoredPower;
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_BatteryBlock", $"Exception! {e}");
			}
		}

		private static void ProcessBeacon(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{
			if (options.RenameBeacon) ((MyObjectBuilder_Beacon)block).CustomName = options.BeaconText;
			if (options.RenameBeacon) ((MyObjectBuilder_Beacon)block).BroadcastRadius = options.BeaconBroadcastRadius;
		}

		private static void ProcessCargoContainer(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{
			if (!options.Restock) return;
			try
			{
				//((MyObjectBuilder_CargoContainer)block).Inventory.Clear();
				ClearInventory(block.ComponentContainer);
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_CargoContainer", $"Exception! {e}");
			}
		}

		private static void ProcessConnector(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{
			try
			{
				
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_CargoContainer", $"Exception! {e}");
			}
		}

		private static void ProcessInteriorTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Doesn't have it's own inventory, needs supporting function to add inventory
			if (!options.Restock) return;
			((MyObjectBuilder_InteriorTurret)block).EnableIdleRotation = !options.DisableIdleTurretMovement;
			ProcessWeaponRestocking(block, options, size);
		}

		private static void ProcessLargeGatlingTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Doesn't have it's own inventory, needs supporting function to add inventory
			if (!options.Restock) return;
			((MyObjectBuilder_LargeGatlingTurret)block).EnableIdleRotation = !options.DisableIdleTurretMovement;
			ProcessWeaponRestocking(block, options, size);
		}

		private static void ProcessLargeMissileTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Doesn't have it's own inventory, needs supporting function to add inventory
			if (!options.Restock) return;
			((MyObjectBuilder_LargeMissileTurret) block).EnableIdleRotation = !options.DisableIdleTurretMovement;
			ProcessWeaponRestocking(block, options, size);
		}

		private static void ProcessOxygenGenerator(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Has it's own inventory, supporting function to add inventory doesn't work on this block
			if (!options.Restock) return;
			try
			{
				MyObjectBuilder_Ore ice = new MyObjectBuilder_Ore { SubtypeName = "Ice" };
				MyPhysicalItemDefinition iceDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(ice.GetId());
				((MyObjectBuilder_OxygenGenerator)block).Inventory.Clear();
				((MyObjectBuilder_OxygenGenerator)block).Inventory.Items.Add(
					new MyObjectBuilder_InventoryItem
					{
						Amount = (int)(GetMaxVolume(block, size) / iceDefinition.Volume),
						PhysicalContent = ice
					});
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_OxygenGenerator", $"Exception! {e}");
			}
		}

		private static void ProcessOxygenTank(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Doesn't need inventory processing, just max the float
			if (!options.Restock) return;
			try
			{
				((MyObjectBuilder_OxygenTank)block).FilledRatio = 1;
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_OxygenTank", $"Exception! {e}");
			}
		}

		private static void ProcessMyProgrammableBlock(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // No inventory to deal with here, woo!
			try
			{
				if (options.PreservePrograms)
				{
					Core.GeneralLog.WriteToLog("ProcessMyProgrammableBlock", $"Entity: {options.EntityId}");
					string tempPbName = TempPbName;
					if(PbPrograms.ContainsKey(options.EntityId))
						PbPrograms[options.EntityId].Add(new PbReplacement(((MyObjectBuilder_MyProgrammableBlock)block).CustomName, TempPbName, ((MyObjectBuilder_MyProgrammableBlock)block).Program));
					else
						PbPrograms.Add(options.EntityId, new List<PbReplacement> {(new PbReplacement(((MyObjectBuilder_MyProgrammableBlock)block).CustomName, TempPbName, ((MyObjectBuilder_MyProgrammableBlock)block).Program))});
					((MyObjectBuilder_MyProgrammableBlock)block).CustomName = tempPbName;
				}
				((MyObjectBuilder_MyProgrammableBlock)block).Program = null;
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_MyProgrammableBlock", $"Exception! {e}");
			}
		}

		private static void ProcessParachute(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Doesn't have it's own inventory, needs supporting function to add inventory
			if (!options.Restock) return;
			try
			{
				List<MyDefinitionId> parachuteMaterialList = GetItemDefinitionList(block);
				if (parachuteMaterialList == null) return;
				MyObjectBuilder_Component parachuteMaterial = new MyObjectBuilder_Component { SubtypeName = parachuteMaterialList[0].SubtypeName };
				MyPhysicalItemDefinition parachuteMaterialDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(parachuteMaterial.GetId());
				//AddToInventory(block.ComponentContainer, parachuteMaterial, (int)(GetMaxVolume(block, size) / parachuteMaterialDefinition.Volume));
				AddToInventory(block.ComponentContainer, parachuteMaterial, GetAmount(GetMaxVolume(block, size), parachuteMaterialDefinition.Volume, 1), true);
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_Parachute", $"Exception! {e}");
			}
		}


		private static void ProcessReactor(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Has it's own inventory, but it doesn't work, so still needs supporting function to add inventory
			if (options.PowerDownGrid) ((MyObjectBuilder_Reactor)block).Enabled = false;
			if (!options.Restock) return;
			try
			{
				List<MyDefinitionId> fuelList = GetItemDefinitionList(block);
				if (fuelList == null) return;
				MyObjectBuilder_Ingot fuel = new MyObjectBuilder_Ingot { SubtypeName = fuelList[0].SubtypeName };
				MyPhysicalItemDefinition fuelDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(fuel.GetId());
				//AddToInventory(block.ComponentContainer, fuel, (int)(GetMaxVolume(block, size) / fuelDefinition.Volume), true);
				AddToInventory(block.ComponentContainer, fuel, GetAmount(GetMaxVolume(block, size), fuelDefinition.Volume, 1), true);
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("MyObjectBuilder_Reactor", $"Exception! {e}");
			}
		}

		private static void ProcessSmallGatlingGun(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{
			if (!options.Restock) return;
			ProcessWeaponRestocking(block, options, size);
		}


		private static void ProcessSmallMissileLauncher(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Has it's own inventory, supporting function to add inventory doesn't work on this block
			if (!options.Restock) return;
			try
			{
				List<MyDefinitionId> ammoSubTypeIds = GetItemDefinitionList(block);
				if (ammoSubTypeIds == null || ammoSubTypeIds.Count == 0) return;
				((MyObjectBuilder_SmallMissileLauncher)block).Inventory.Clear();
				foreach (MyDefinitionId ammoSubType in ammoSubTypeIds)
				{
					MyAmmoMagazineDefinition myAmmo = MyDefinitionManager.Static.GetAmmoMagazineDefinition(ammoSubType);
					int amountToAdd = (int)(GetMaxVolume(block, size) / myAmmo.Volume) / ammoSubTypeIds.Count;
					((MyObjectBuilder_SmallMissileLauncher)block).Inventory.Items.Add(new MyObjectBuilder_InventoryItem
					{
						Amount = amountToAdd,
						PhysicalContent = new MyObjectBuilder_AmmoMagazine() { SubtypeName = ammoSubType.SubtypeName }
					});
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("ProcessWeaponRestocking", $"Exception! {e}");
			}
		}

		private static void ProcessSmallMissileLauncherReload(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // Has it's own inventory, supporting function to add inventory doesn't work on this block
			if (!options.Restock) return;
			try
			{
				List<MyDefinitionId> ammoSubTypeIds = GetItemDefinitionList(block);
				if (ammoSubTypeIds == null || ammoSubTypeIds.Count == 0) return;
				((MyObjectBuilder_SmallMissileLauncherReload)block).Inventory.Clear();
				foreach (MyDefinitionId ammoSubType in ammoSubTypeIds)
				{
					MyAmmoMagazineDefinition myAmmo = MyDefinitionManager.Static.GetAmmoMagazineDefinition(ammoSubType);
					int amountToAdd = (int)(GetMaxVolume(block, size) / myAmmo.Volume) / ammoSubTypeIds.Count;
					((MyObjectBuilder_SmallMissileLauncherReload)block).Inventory.Items.Add(new MyObjectBuilder_InventoryItem
					{
						Amount = amountToAdd,
						PhysicalContent = new MyObjectBuilder_AmmoMagazine() { SubtypeName = ammoSubType.SubtypeName }
					});
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("ProcessWeaponRestocking", $"Exception! {e}");
			}
		}

		private static void ClearInventory(MyObjectBuilder_ComponentContainer componentContainer)
		{
			try
			{
				foreach (MyObjectBuilder_ComponentContainer.ComponentData componentData in componentContainer.Components)
				{
					if (componentData.TypeId != "MyInventoryBase") continue;
					((MyObjectBuilder_Inventory)componentData.Component)?.Items.Clear();
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("ClearInventory", $"Exception! {e}");
			}
		}

		private static void ProcessWeaponRestocking(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{
			try
			{
				List<MyDefinitionId> ammoSubTypeIds = GetItemDefinitionList(block);
				if (ammoSubTypeIds == null || ammoSubTypeIds.Count == 0) return;
				ClearInventory(block.ComponentContainer);
				foreach (MyDefinitionId ammoSubType in ammoSubTypeIds)
				{
					MyAmmoMagazineDefinition myAmmo = MyDefinitionManager.Static.GetAmmoMagazineDefinition(ammoSubType);
					//int amountToAdd = (int)Math.Ceiling((GetMaxVolume(block, size) / myAmmo.Volume) / ammoSubTypeIds.Count);
					int amountToAdd = GetAmount(GetMaxVolume(block, size), myAmmo.Volume, ammoSubTypeIds.Count);
					AddToInventory(block.ComponentContainer, new MyObjectBuilder_AmmoMagazine() {SubtypeName = ammoSubType.SubtypeName}, amountToAdd, false);
					Core.GeneralLog.WriteToLog("ProcessWeaponRestocking", $"Adding to inventory:\t{ammoSubType.SubtypeName}: {amountToAdd}");
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("ProcessWeaponRestocking", $"Exception! {e}");
			}
		}

		private static void AddToInventory(MyObjectBuilder_ComponentContainer componentContainer, MyObjectBuilder_PhysicalObject item, MyFixedPoint amount, bool clearInventory)
		{
			try
			{
				//Core.GeneralLog.WriteToLog("AddToInventory", $"{componentContainer}\t{item}\t{amount}");
				foreach (MyObjectBuilder_ComponentContainer.ComponentData componentData in componentContainer.Components)
				{
					if (componentData.TypeId != "MyInventoryBase") continue;
					if (clearInventory) ((MyObjectBuilder_Inventory)componentData.Component)?.Items.Clear();
					((MyObjectBuilder_Inventory)componentData.Component)?.Items.Add(
						new MyObjectBuilder_InventoryItem
						{
							Amount = amount,
							PhysicalContent = item
						});
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("AddToInventory", $"Exception! {e}");
			}
		}

		private static List<MyDefinitionId> GetItemDefinitionList(MyObjectBuilder_CubeBlock block)
		{
			Func<MyObjectBuilder_CubeBlock, List<MyDefinitionId>> getParachuteMaterialFunc;
			Definitions.RestockDefinitions.TryGetValue(block.GetType(), out getParachuteMaterialFunc);
			return getParachuteMaterialFunc?.Invoke(block);
		}

		private static double GetMaxVolume(MyObjectBuilder_Base block, MyCubeSize size)
		{
			Func<MyCubeSize, MyStringHash, double> volumeFunc;
			Definitions.MaxInventoryVolume.TryGetValue(block.TypeId, out volumeFunc);
			if (volumeFunc == null) return 0;
			return volumeFunc.Invoke(size, block.SubtypeId);
		}

		private static int GetAmount(double blockVolume, double itemVolume, int count)
		{
			return (int)Math.Ceiling((blockVolume / itemVolume) / count);
		}
	}
}


//MyObjectBuilder_InventoryItem newItem = new MyObjectBuilder_InventoryItem
//{
//	PhysicalContent = new MyObjectBuilder_AmmoMagazine() { SubtypeName = ammoSubType.SubtypeName },
//	Amount = amountToAdd,
//};

//MyObjectBuilder_Inventory myObjectBuilderInventory = componentData.Component as MyObjectBuilder_Inventory;
//myObjectBuilderInventory?.Items.Clear();
//myObjectBuilderInventory?.Items.Add(newItem);

//foreach (MyObjectBuilder_ComponentContainer.ComponentData componentData in block.ComponentContainer.Components)
//{
//	if (componentData.TypeId != "MyInventoryBase") continue;
//	Core.GeneralLog.WriteToLog("ProcessWeaponRestocking", $"{block.GetType()}\t{block.EntityId}\t{componentData.TypeId}\t{ammoSubType.SubtypeName}\t{amountToAdd}");
//	((MyObjectBuilder_Inventory) componentData.Component)?.Items.Clear();
//	((MyObjectBuilder_Inventory) componentData.Component)?.Items.Add(
//		new MyObjectBuilder_InventoryItem
//		{
//			Amount = amountToAdd,
//			PhysicalContent = new MyObjectBuilder_AmmoMagazine() { SubtypeName = ammoSubType.SubtypeName }
//		});
//}