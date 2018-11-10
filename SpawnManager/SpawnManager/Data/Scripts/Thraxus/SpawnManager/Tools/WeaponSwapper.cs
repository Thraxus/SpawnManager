using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using SpawnManager.Support;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using static SpawnManager.Support.Definitions;

namespace SpawnManager.Tools
{
	internal static class WeaponSwapper
	{
		/* Rules:
		 *	x and y must be the same, z can be any size up to max of original block
		 *	mount points must be on the same side
		 *
		 *	MyObjectBuilder_WeaponBlockDefinition
		 *	MyObjectBuilder_LargeTurretBaseDefinition
		 */

		//public static List<Definitions.WeaponInformation> LargeGridWeaponTurretBases = new List<Definitions.WeaponInformation>();

		//public static List<Definitions.WeaponInformation> LargeGridWeaponBlocks = new List<Definitions.WeaponInformation>();

		//public static List<Definitions.WeaponInformation> SmallGridWeaponTurretBases = new List<Definitions.WeaponInformation>();

		//public static List<Definitions.WeaponInformation> SmallGridWeaponBlocks = new List<Definitions.WeaponInformation>();


		internal static void ProcessPrefab(MyPrefabDefinition prefab, Options options)
		{
			try
			{
				foreach (MyObjectBuilder_CubeGrid gridBuilder in prefab.CubeGrids)
				{
					foreach (MyObjectBuilder_CubeBlock block in gridBuilder.CubeBlocks)
					{
						Action<MyObjectBuilder_CubeBlock, Options, MyCubeSize> action;
						ProcessWeapons.TryGetValue(block.GetType(), out action);
						action?.Invoke(block, options, gridBuilder.GridSizeEnum);
					}
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessPrefab", $"Exception!\t{e}");
			}
		}

		private static readonly Dictionary<MyObjectBuilderType, Action<MyObjectBuilder_CubeBlock, Options, MyCubeSize>> ProcessWeapons = new Dictionary<MyObjectBuilderType, Action<MyObjectBuilder_CubeBlock, Options, MyCubeSize>>
		{
			{ typeof(MyObjectBuilder_InteriorTurret), ProcessInteriorTurret },
			{ typeof(MyObjectBuilder_LargeGatlingTurret), ProcessLargeGatlingTurret },
			{ typeof(MyObjectBuilder_LargeMissileTurret), ProcessLargeMissileTurret },
			{ typeof(MyObjectBuilder_SmallGatlingGun), ProcessSmallGatlingGun },
			{ typeof(MyObjectBuilder_SmallMissileLauncher), ProcessSmallMissileLauncher },
			{ typeof(MyObjectBuilder_SmallMissileLauncherReload), ProcessSmallMissileLauncherReload },
		};

		private static void ProcessInteriorTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{
				List<WeaponInformation> replacementOptions = LargeGridWeaponTurretBases.FindAll(x =>
					x.ModName != "Vanilla" &&
					x.MountPoint == BlockSideEnum.Bottom &&
					x.SizeX == 1 && 
					x.SizeY == 1);
				if (replacementOptions.Count == 0) return;
				block = CreateReplacementTurretBase(replacementOptions[Core.Random.Next(0, replacementOptions.Count)], block, size);
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessInteriorTurret", $"Block {block.SubtypeId} should have been replaced...");
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessInteriorTurret", $"Exception!\t{e}");
			}
		}

		private static MyObjectBuilder_CubeBlock CreateReplacementTurretBase(WeaponInformation weaponInformation, MyObjectBuilder_CubeBlock block, MyCubeSize size)
		{

			MyObjectBuilder_CubeBlock myNewWeapon = new MyObjectBuilder_TurretBase()
			{
				SubtypeName = weaponInformation.SubtypeName,
				Range = 2500,
				Orientation = block.Orientation,
				BlockOrientation = block.BlockOrientation,
				ColorMaskHSV = block.ColorMaskHSV,
				Min = block.Min,
				CustomName = "Retrofit " + block.Name,
				TargetCharacters = ((MyObjectBuilder_TurretBase) block).TargetCharacters,
				TargetLargeGrids = ((MyObjectBuilder_TurretBase)block).TargetLargeGrids,
				TargetMeteors = ((MyObjectBuilder_TurretBase)block).TargetMeteors,
				TargetMissiles = ((MyObjectBuilder_TurretBase)block).TargetMissiles,
				TargetNeutrals = ((MyObjectBuilder_TurretBase)block).TargetNeutrals,
				TargetSmallGrids = ((MyObjectBuilder_TurretBase)block).TargetSmallGrids,
				TargetStations = ((MyObjectBuilder_TurretBase)block).TargetStations,
				EnableIdleRotation = ((MyObjectBuilder_TurretBase)block).EnableIdleRotation,

			};
			return myNewWeapon;
		}

		/*	Full Code to build something from scratch in OB:
		-------------------------------
		MyObjectBuilder_CubeGrid grid = new MyObjectBuilder_CubeGrid()
		{
			EntityId = 0,
			Name = "FakeNoise",
			DisplayName = "SmallCargoTestGrid",
			GridSizeEnum = MyCubeSize.Small,
			Skeleton = new List<BoneInfo>(),
			PositionAndOrientation = new MyPositionAndOrientation(MyAPIGateway.Session.Player.GetPosition() + new Vector3D(5, 5, 5), Vector3.Forward, Vector3.Up),
			CubeBlocks = new List<MyObjectBuilder_CubeBlock>()
					{
						new MyObjectBuilder_CubeBlock()
						{
							EntityId = 0,
							SubtypeName = "SmallBlockSmallContainer",
							Name = "smallGridSmallCargoTest",
							Owner = 0,
							ShareMode = MyOwnershipShareModeEnum.All,
							BlockOrientation = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Down),
							Min = new SerializableVector3I(0, 0, 0),
							ComponentContainer = new MyObjectBuilder_ComponentContainer()
							{
								Components = new List<MyObjectBuilder_ComponentContainer.ComponentData>()
								{
									new MyObjectBuilder_ComponentContainer.ComponentData()
									{
										TypeId = "MyInventoryBase",
										Component = new MyObjectBuilder_Inventory()
										{
											InventoryFlags = MyInventoryFlags.CanSend | MyInventoryFlags.CanReceive,
											RemoveEntityOnEmpty = false
										}
									}
								}
							}
						}
					},
			LinearVelocity = Vector3.Zero,
			AngularVelocity = Vector3.Zero,
			ConveyorLines = new List<MyObjectBuilder_ConveyorLine>(),
			BlockGroups = new List<MyObjectBuilder_BlockGroup>(),
			Handbrake = false,
			XMirroxPlane = null,
			YMirroxPlane = null,
			ZMirroxPlane = null,
			PersistentFlags = MyPersistentEntityFlags2.InScene,
			CreatePhysics = true,
			DestructibleBlocks = true,
			IsStatic = false,
		};
			MyObjectBuilder_EntityBase newCargo = grid;
			MyAPIGateway.Entities.RemapObjectBuilder(newCargo);
				MyAPIGateway.Entities.CreateFromObjectBuilderParallel(newCargo, true);
		------------------------------- */

		private static void ProcessLargeGatlingTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeGatlingTurret", $"Exception!\t{e}");
			}
		}

		private static void ProcessLargeMissileTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeMissileTurret", $"Exception!\t{e}");
			}
		}

		private static void ProcessSmallGatlingGun(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // WeaponBlock
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessSmallGatlingGun", $"Exception!\t{e}");
			}
		}

		private static void ProcessSmallMissileLauncher(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // WeaponBlock
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessSmallMissileLauncher", $"Exception!\t{e}");
			}
		}

		private static void ProcessSmallMissileLauncherReload(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // WeaponBlock
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessSmallMissileLauncherReload", $"Exception!\t{e}");
			}
		}
		
		//public void Junk()
		//{
		//MyWeaponBlockDefinition myWeapon = new MyWeaponBlockDefinition();
		//MyLargeTurretBaseDefinition myLargeTurret = new MyLargeTurretBaseDefinition();
		//MyCubeBlockDefinition myCube = new MyCubeBlockDefinition();
		//myCube.
		//myCube.Skeleton
		//MyCubeBlockDefinition.MountPoint mountPoint = new MyCubeBlockDefinition.MountPoint();
		//MyObjectBuilder_CubeBlockDefinition.MountPoint objectBuilder = myCube.MountPoints[0].GetObjectBuilder(myCube.MountPoints[0].Normal);
		//BlockSideEnum objectBuilderSide =  objectBuilder.Side;
		//myWeapon.MountPoints[0].

		//}

		//public struct WeaponInformation
		//{
		//	// Back, Bottom, Front, Left, Right, Top
		//	private readonly BlockSideEnum _mountPoint;

		//	private readonly MyCubeSize _myCubeSize;

		//	private readonly int _sizeX;
		//	private readonly int _sizeY;
		//	private readonly int _sizeZ;

		//	private readonly string _subtypeId;
		//	private readonly string _modName;
		//	private readonly string _id;

		//	public WeaponInformation(BlockSideEnum mountPoint, MyCubeSize myCubeSize, int sizeX, int sizeY, int sizeZ, string subtypeId, string modName, string id)
		//	{
		//		_mountPoint = mountPoint;
		//		_myCubeSize = myCubeSize;
		//		_sizeX = sizeX;
		//		_sizeY = sizeY;
		//		_sizeZ = sizeZ;
		//		_subtypeId = subtypeId;
		//		_modName = modName;
		//		_id = id;
		//	}
		//}

		//public List<WeaponInformation> LargeGridWeaponsList = new List<WeaponInformation>();

		//public List<WeaponInformation> SmallGridWeaponsList = new List<WeaponInformation>();

		//public Dictionary<MyCubeSize, List<WeaponInformation>> WeaponRepository = new Dictionary<MyCubeSize, List<WeaponInformation>>();

		//public struct ReplacementWeapon
		//{
		//	private WeaponLocation weaponLocation;
		//	private Definitions.DefinitionCategory definitionCategory;
		//}

		//private static readonly Dictionary<MyObjectBuilderType, Action<MyStringHash, string, string>> DictionaryBuilder = new Dictionary<MyObjectBuilderType, Action<MyStringHash, string, string>>()
		//{
		//	{ typeof(MyObjectBuilder_WeaponBlockDefinition), delegate(MyStringHash subTypeId, string modName, string id) {AmmoMagazineDefinitions.Add(subTypeId,  new Definitions.DefinitionCategory(modName, id));}},
		//	{ typeof(MyObjectBuilder_LargeTurretBaseDefinition), delegate(MyStringHash subTypeId, string modName, string id) {ComponentDefinitions.Add(subTypeId,  new Definitions.DefinitionCategory(modName, id));}},
		//};
	}
}
