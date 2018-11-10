using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpawnManager.Eem;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverQueried.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SpawnManager.Support
{
	public static class Definitions
	{
		private static readonly float InventoryMultiplier = MyAPIGateway.Session.SessionSettings.InventorySizeMultiplier;
		
		// Large Grid Max Volumes
		private static readonly double LargeBlockSmallContainer = 15.625 * InventoryMultiplier;
		private static readonly double LargeBlockLargeContainer = 421.875008 * InventoryMultiplier;
		private static readonly double LargeBlockLargeGenerator = 8 * InventoryMultiplier;
		private static readonly double LargeBlockSmallGenerator = 1 * InventoryMultiplier;
		private static readonly double OxygenGenerator = 4 * InventoryMultiplier;
		private static readonly double LargeMissileLauncher = 1.14 * InventoryMultiplier;
		private static readonly double LargeMissileTurret = 0.384 * InventoryMultiplier;
		private static readonly double LargeGatlingTurret = 0.384 * InventoryMultiplier;
		private static readonly double LargeInteriorTurret = 0.384 * InventoryMultiplier;
		private static readonly double LgParachute = 0.04 * InventoryMultiplier;

		// Small Grid Max Volumes
		private static readonly double OxygenGeneratorSmall = 1 * InventoryMultiplier;
		private static readonly double SmallGatlingTurret = 0.084 * InventoryMultiplier;
		private static readonly double SmallGatlingGun = 0.064 * InventoryMultiplier;
		private static readonly double SmallRocketLauncherReload = 0.24 * InventoryMultiplier;
		private static readonly double SmallBlockLargeGenerator = 1 * InventoryMultiplier;
		private static readonly double SmallMissileLauncher = 0.24 * InventoryMultiplier;
		private static readonly double SmallMissileTurret = 0.12 * InventoryMultiplier;
		private static readonly double SmallBlockSmallGenerator = 0.125 * InventoryMultiplier;
		private static readonly double SmallBlockSmallContainer = 0.125 * InventoryMultiplier;
		private static readonly double SmallBlockMediumContainer = 3.375 * InventoryMultiplier;
		private static readonly double SmallBlockLargeContainer = 15.625 * InventoryMultiplier;
		private static readonly double SmParachute = 0.008 * InventoryMultiplier;

		public struct DefinitionCategory
		{
			public string ModName;
			public string Id;

			public DefinitionCategory(string modName, string id)
			{
				ModName = modName;
				Id = id;
			}
		}

		public struct PirateAntennaDefinition
		{
			public readonly float SpawnDistance;
			public readonly int SpawnTimeMs;
			public readonly int FirstSpawnTimeMs;
			public readonly int MaxDrones;
			public readonly List<string> SpawnGroups;

			public PirateAntennaDefinition(float spawnDistance, int spawnTimeMs, int firstSpawnTimeMs, int maxDrones, List<string> spawnGroups)
			{
				SpawnDistance = spawnDistance;
				SpawnTimeMs = spawnTimeMs;
				FirstSpawnTimeMs = firstSpawnTimeMs;
				MaxDrones = maxDrones;
				SpawnGroups = spawnGroups;
			}
		}

		public struct SpawnGroupPrefabDefinition
		{
			public string Id;
			public Vector3D Position;
			public double Speed;
			public string BeaconText;

			public SpawnGroupPrefabDefinition(string id, Vector3D position = new Vector3D(), double speed = 0, string beaconText = "")
			{
				Id = id;
				Position = position;
				Speed = speed;
				BeaconText = beaconText;
			}
		}

		public struct SpawnGroupDefinition
		{
			public double Frequency;
			public bool IsPirate;
			public bool IsEncounter;
			public bool ReactorsOn;
			public List<SpawnGroupPrefabDefinition> Prefabdefinitions;

			public SpawnGroupDefinition(double frequency, bool isPirate, bool isEncounter, bool reactorsOn, List<SpawnGroupPrefabDefinition> prefabdefinitions)
			{
				Frequency = frequency;
				IsPirate = isPirate;
				IsEncounter = isEncounter;
				ReactorsOn = reactorsOn;
				Prefabdefinitions = prefabdefinitions;
			}
		}

		public static readonly Dictionary<MyObjectBuilderType, Func<MyCubeSize, MyStringHash, double>> MaxInventoryVolume = new Dictionary<MyObjectBuilderType, Func<MyCubeSize, MyStringHash, double>>()
		{
			{ typeof(MyObjectBuilder_CargoContainer), (size, hash) => size == MyCubeSize.Large
				? hash == MyStringHash.GetOrCompute("LargeBlockLargeContainer") ? LargeBlockLargeContainer : LargeBlockSmallContainer
				: hash == MyStringHash.GetOrCompute("SmallBlockLargeContainer") ? SmallBlockLargeContainer :
					hash == MyStringHash.GetOrCompute("SmallBlockMediumContainer") ? SmallBlockMediumContainer : SmallBlockSmallContainer },
			{ typeof(MyObjectBuilder_InteriorTurret), (size, hash) => LargeInteriorTurret},
			{ typeof(MyObjectBuilder_LargeMissileTurret), (size, hash) => size == MyCubeSize.Large ? LargeMissileTurret : SmallMissileTurret},
			{ typeof(MyObjectBuilder_LargeGatlingTurret), (size, hash) => size == MyCubeSize.Large ? LargeGatlingTurret : SmallGatlingTurret},
			{ typeof(MyObjectBuilder_OxygenGenerator), (size, hash) => size == MyCubeSize.Large ? OxygenGenerator : OxygenGeneratorSmall},
			{ typeof(MyObjectBuilder_Parachute), (size, hash) => size == MyCubeSize.Large ? LgParachute : SmParachute},
			{ typeof(MyObjectBuilder_Reactor), (size, hash) => size == MyCubeSize.Large ? hash == MyStringHash.GetOrCompute("LargeBlockLargeGenerator") ? LargeBlockLargeGenerator
				: LargeBlockSmallGenerator : hash == MyStringHash.GetOrCompute("SmallBlockLargeGenerator") ? SmallBlockLargeGenerator : SmallBlockSmallGenerator },
			{ typeof(MyObjectBuilder_SmallGatlingGun), (size, hash) => SmallGatlingGun},
			{ typeof(MyObjectBuilder_SmallMissileLauncher), (size, hash) => size == MyCubeSize.Large ? LargeMissileLauncher : SmallMissileLauncher},
			{ typeof(MyObjectBuilder_SmallMissileLauncherReload), (size, hash) => SmallRocketLauncherReload}
		};

		public static readonly Dictionary<MyObjectBuilderType, Func<MyObjectBuilder_CubeBlock, List<MyDefinitionId>>> RestockDefinitions = new Dictionary<MyObjectBuilderType, Func<MyObjectBuilder_CubeBlock, List<MyDefinitionId>>>
		{
			{ typeof(MyObjectBuilder_InteriorTurret), GetWeaponAmmoList },
			{ typeof(MyObjectBuilder_LargeGatlingTurret), GetWeaponAmmoList },
			{ typeof(MyObjectBuilder_LargeMissileTurret), GetWeaponAmmoList },
			{ typeof(MyObjectBuilder_Parachute), GetParachuteList },
			{ typeof(MyObjectBuilder_Reactor), GetReactorFuelList },
			{ typeof(MyObjectBuilder_SmallGatlingGun), GetWeaponAmmoList },
			{ typeof(MyObjectBuilder_SmallMissileLauncher), GetWeaponAmmoList },
			{ typeof(MyObjectBuilder_SmallMissileLauncherReload), GetWeaponAmmoList }
		};

		private static List<MyDefinitionId> GetReactorFuelList(MyObjectBuilder_CubeBlock cube)
		{
			List<MyDefinitionId> fuelIds = new List<MyDefinitionId>();
			try
			{
				MyReactorDefinition reactorDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(cube.GetId()) as MyReactorDefinition;
				if (reactorDefinition == null) return fuelIds;
				fuelIds.Add(reactorDefinition.FuelId);
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("GetReactorFuelList", $"Exception getting reactor fuel list! {e}");
			}
			return fuelIds;
		}

		private static List<MyDefinitionId> GetParachuteList(MyObjectBuilder_CubeBlock cube)
		{
			List<MyDefinitionId> parachuteIds = new List<MyDefinitionId>();
			try
			{
				MyParachuteDefinition parachuteDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(cube.GetId()) as MyParachuteDefinition;
				if (parachuteDefinition == null) return parachuteIds;
				parachuteIds.Add(parachuteDefinition.MaterialDefinitionId);
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("GetParachuteList", $"Exception getting parachute list! {e}");
			}
			return parachuteIds;
		}

		private static List<MyDefinitionId> GetWeaponAmmoList(MyObjectBuilder_CubeBlock cube)
		{
			List<MyDefinitionId> ammoSubTypeIds = new List<MyDefinitionId>();
			try
			{
				MyWeaponDefinition myWeapon = MyDefinitionManager.Static.GetWeaponDefinition(((MyWeaponBlockDefinition) MyDefinitionManager.Static.GetCubeBlockDefinition(cube.GetId())).WeaponDefinitionId);
				if (myWeapon == null) return ammoSubTypeIds;
				ammoSubTypeIds.AddRange(myWeapon.AmmoMagazinesId);
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("GetWeaponAmmoList", $"Exception getting ammo list! {e}");
			}
			return ammoSubTypeIds;
		}

		private static readonly Dictionary<MyObjectBuilderType, Action<MyStringHash, string, string>> DictionaryBuilder = new Dictionary<MyObjectBuilderType, Action<MyStringHash, string, string>>()
		{
			{ typeof(MyObjectBuilder_AmmoMagazine), delegate(MyStringHash subTypeId, string modName, string id) {AmmoMagazineDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}},
			{ typeof(MyObjectBuilder_Component), delegate(MyStringHash subTypeId, string modName, string id) {ComponentDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}},
			{ typeof(MyObjectBuilder_GasContainerObject), delegate(MyStringHash subTypeId, string modName, string id) {GasContainerDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}},
			{ typeof(MyObjectBuilder_Ingot), delegate(MyStringHash subTypeId, string modName, string id) {IngotDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}},
			{ typeof(MyObjectBuilder_Ore), delegate(MyStringHash subTypeId, string modName, string id) {OreDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}},
			{ typeof(MyObjectBuilder_OxygenContainerObject), delegate(MyStringHash subTypeId, string modName, string id) {OxygenContainerDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}},
			{ typeof(MyObjectBuilder_PhysicalGunObject), delegate(MyStringHash subTypeId, string modName, string id) {PhysicalGunDefinitions.Add(subTypeId,  new DefinitionCategory(modName, id));}}
		};

		public static readonly Dictionary<MyStringHash, DefinitionCategory> AmmoMagazineDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();
		public static readonly Dictionary<MyStringHash, DefinitionCategory> ComponentDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();
		public static readonly Dictionary<MyStringHash, DefinitionCategory> GasContainerDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();
		public static readonly Dictionary<MyStringHash, DefinitionCategory> IngotDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();
		public static readonly Dictionary<MyStringHash, DefinitionCategory> OreDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();
		public static readonly Dictionary<MyStringHash, DefinitionCategory> OxygenContainerDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();
		public static readonly Dictionary<MyStringHash, DefinitionCategory> PhysicalGunDefinitions = new Dictionary<MyStringHash, DefinitionCategory>();

		public static readonly Dictionary<string, PirateAntennaDefinition> PirateAntennaDefinitions = new Dictionary<string, PirateAntennaDefinition>();
		public static readonly Dictionary<string, SpawnGroupDefinition> SpawnGroupDefinitions = new Dictionary<string, SpawnGroupDefinition>();

		public static Dictionary<string, CustomPrefabConfiguration> CustomPrefabConfigurations;

		/* Rules:
		 *	x and y must be the same, z can be any size up to max of original block
		 *	mount points must be on the same side
		 *
		 *	MyObjectBuilder_WeaponBlockDefinition
		 *	MyObjectBuilder_LargeTurretBaseDefinition
		 */

		private static readonly Dictionary<MyObjectBuilderType, Action<MyDefinitionBase>> WeaponDictionaryBuilder = new Dictionary<MyObjectBuilderType, Action<MyDefinitionBase>>()
		{
			{typeof(MyWeaponBlockDefinition), ProcessWeaponBlock},
			{typeof(MyLargeTurretBaseDefinition), ProcessLargeTurretBase}
		};

		private static void ProcessWeaponBlock(MyDefinitionBase myDefinition)
		{
			try
			{
				//MyCubeBlockDefinition myCubeBlock = MyDefinitionManager.Static.GetCubeBlockDefinition(myDefinition.Id);
				MyWeaponBlockDefinition myWeaponBlock = (MyWeaponBlockDefinition) MyDefinitionManager.Static.GetCubeBlockDefinition(myDefinition.Id);

				foreach (MyCubeBlockDefinition.MountPoint myMountPoint in myWeaponBlock.MountPoints)
				{
					WeaponInformation myWeaponInformation = new WeaponInformation(
						myMountPoint.GetObjectBuilder(myMountPoint.Normal).Side,
						myWeaponBlock.CubeSize,
						myWeaponBlock.Size.X,
						myWeaponBlock.Size.Y,
						myWeaponBlock.Size.Z,
						myDefinition.Id.SubtypeId, 
						myDefinition.Id.SubtypeName,
						myDefinition.Context?.ModName ?? "Vanilla", 
						myDefinition.Id.ToString()
					);
					if (myWeaponBlock.CubeSize == MyCubeSize.Large)
						LargeGridWeaponBlocks.Add(myWeaponInformation);
					else SmallGridWeaponBlocks.Add(myWeaponInformation);
					Core.GeneralLog.WriteToLog("ProcessWeaponBlock", myWeaponInformation.ToString());
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("ProcessWeaponBlock", $"Exception! {e}");
			}
		}

		private static void ProcessLargeTurretBase(MyDefinitionBase myDefinition)
		{
			try
			{
				//MyCubeBlockDefinition myCubeBlock = MyDefinitionManager.Static.GetCubeBlockDefinition(myDefinition.Id);
				MyLargeTurretBaseDefinition myLargeTurret = (MyLargeTurretBaseDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(myDefinition.Id);
				foreach (MyCubeBlockDefinition.MountPoint myMountPoint in myLargeTurret.MountPoints)
				{
					WeaponInformation myWeaponInformation = new WeaponInformation(
						myMountPoint.GetObjectBuilder(myMountPoint.Normal).Side,
						myLargeTurret.CubeSize,
						myLargeTurret.Size.X,
						myLargeTurret.Size.Y,
						myLargeTurret.Size.Z,
						myDefinition.Id.SubtypeId,
						myDefinition.Id.SubtypeName,
						myDefinition.Context?.ModName ?? "Vanilla",
						myDefinition.Id.ToString()
					);
					if (myLargeTurret.CubeSize == MyCubeSize.Large)
						LargeGridWeaponTurretBases.Add(myWeaponInformation);
					else SmallGridWeaponTurretBases.Add(myWeaponInformation);
					Core.GeneralLog.WriteToLog("ProcessLargeTurretBase", myWeaponInformation.ToString());
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("ProcessLargeTurretBase", $"Exception! {e}");
			}
		}

		public struct WeaponInformation
		{
			// Back, Bottom, Front, Left, Right, Top
			public readonly BlockSideEnum MountPoint;
			
			public readonly MyCubeSize MyCubeSize;
			
			public readonly int SizeX;
			public readonly int SizeY;
			public readonly int SizeZ;
			
			public readonly MyStringHash SubtypeId;
			public readonly string SubtypeName;
			public readonly string ModName;
			public readonly string Id;

			public WeaponInformation(BlockSideEnum mountPoint, MyCubeSize myCubeSize, int sizeX, int sizeY, int sizeZ, MyStringHash subtypeId, string subtypeName, string modName, string id)
			{
				MountPoint = mountPoint;
				MyCubeSize = myCubeSize;
				SizeX = sizeX;
				SizeY = sizeY;
				SizeZ = sizeZ;
				SubtypeId = subtypeId;
				SubtypeName = subtypeName;
				ModName = modName;
				Id = id;
			}

			public override string ToString()
			{
				return $"MountPoint:\t{MountPoint.ToString()}\tCubeSize:\t{MyCubeSize.ToString()}\tSizeX:\t{SizeX.ToString()}\tSizeY:\t{SizeY.ToString()}\tSizeZ:\t{SizeZ.ToString()}\tSubtypeId:\t{SubtypeId.ToString()}\tSubtypeName:\t{SubtypeName}\tModName:\t{ModName}\tID:\t{Id}";
			}
		}

		public static List<WeaponInformation> LargeGridWeaponTurretBases = new List<WeaponInformation>();

		public static List<WeaponInformation> LargeGridWeaponBlocks = new List<WeaponInformation>();

		public static List<WeaponInformation> SmallGridWeaponTurretBases = new List<WeaponInformation>();
		
		public static List<WeaponInformation> SmallGridWeaponBlocks = new List<WeaponInformation>();

		private static bool _registered;

		public static void Register()
		{
			if (_registered) return;

			try
			{
				foreach (MyDefinitionBase definition in MyDefinitionManager.Static.GetAllDefinitions())
				{
					Action<MyStringHash, string, string> action;
					DictionaryBuilder.TryGetValue(definition.Id.TypeId, out action);
					if (!definition.Public) continue;
					action?.Invoke(definition.Id.SubtypeId, definition.Context?.ModName ?? "Vanilla", definition.Id.ToString());

					try
					{
						Action<MyDefinitionBase> weaponAction;
						WeaponDictionaryBuilder.TryGetValue(definition.GetType(), out weaponAction);
						weaponAction?.Invoke(definition);
					}
					catch (Exception e)
					{
						Core.GeneralLog.WriteToLog("WeaponDefinitions", $".GetType failed?! {e}");
					}
				}

				foreach (MySpawnGroupDefinition spawnGroupDefinition in MyDefinitionManager.Static.GetSpawnGroupDefinitions())
				{
					if (!spawnGroupDefinition.Public) continue;
					List<SpawnGroupPrefabDefinition> spawnGroupPrefabDefinitions = new List<SpawnGroupPrefabDefinition>();
					foreach (MySpawnGroupDefinition.SpawnGroupPrefab spawnGroupPrefab in spawnGroupDefinition.Prefabs)
						spawnGroupPrefabDefinitions.Add(new SpawnGroupPrefabDefinition(spawnGroupPrefab.SubtypeId, spawnGroupPrefab.Position, spawnGroupPrefab.Speed, spawnGroupPrefab.BeaconText));
					SpawnGroupDefinitions.Add(spawnGroupDefinition.Id.SubtypeName, new SpawnGroupDefinition(spawnGroupDefinition.Frequency, spawnGroupDefinition.IsPirate, spawnGroupDefinition.IsEncounter, spawnGroupDefinition.ReactorsOn, spawnGroupPrefabDefinitions));
				}

				foreach (MyPirateAntennaDefinition pirateAntennaDefinition in MyDefinitionManager.Static.GetPirateAntennaDefinitions())
				{
					List<string> spawnGroups = new List<string>();
					foreach (MySpawnGroupDefinition mySpawnGroupDefinition in pirateAntennaDefinition.SpawnGroupSampler)
						spawnGroups.Add(mySpawnGroupDefinition.Id.SubtypeName);
					PirateAntennaDefinitions.Add(pirateAntennaDefinition.Name, new PirateAntennaDefinition(pirateAntennaDefinition.SpawnDistance, pirateAntennaDefinition.SpawnTimeMs, pirateAntennaDefinition.FirstSpawnTimeMs, pirateAntennaDefinition.MaxDrones, spawnGroups));
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("Definitions", $"Exception building Dictionaries! {e}");
			}

			_registered = true;
			Core.GeneralLog.WriteToLog("Definitions", "Defined!... :)");
		}

		public static void Close()
		{
			AmmoMagazineDefinitions?.Clear();
			ComponentDefinitions?.Clear();
			GasContainerDefinitions?.Clear();
			IngotDefinitions?.Clear();
			OreDefinitions?.Clear();
			OxygenContainerDefinitions?.Clear();
			PhysicalGunDefinitions?.Clear();
			PirateAntennaDefinitions?.Clear();
			SpawnGroupDefinitions?.Clear();
			DictionaryBuilder?.Clear();
			CustomPrefabConfigurations?.Clear();
			RestockDefinitions?.Clear();
			MaxInventoryVolume?.Clear();
			LargeGridWeaponTurretBases?.Clear();
			LargeGridWeaponBlocks?.Clear();
			SmallGridWeaponTurretBases?.Clear();
			SmallGridWeaponBlocks?.Clear();
			Core.GeneralLog.WriteToLog("Definitions", "Undefined... :(");
		}
	}
}