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
using VRage.Game.ObjectBuilders.ComponentSystem;
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
					for (int index = 0; index < gridBuilder.CubeBlocks.Count; index++)
					{
						MyObjectBuilder_CubeBlock block = gridBuilder.CubeBlocks[index];
						Func<MyObjectBuilder_CubeBlock, Options, MyCubeSize, MyObjectBuilder_CubeBlock> func;
						ProcessWeapons.TryGetValue(block.GetType(), out func);
						if (func != null) gridBuilder.CubeBlocks[index] = func.Invoke(block, options, gridBuilder.GridSizeEnum);
					}
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessPrefab", $"Exception!\t{e}");
			}
		}

		private static readonly Dictionary<MyObjectBuilderType, Func<MyObjectBuilder_CubeBlock, Options, MyCubeSize, MyObjectBuilder_CubeBlock>> ProcessWeapons = new Dictionary<MyObjectBuilderType, Func<MyObjectBuilder_CubeBlock, Options, MyCubeSize, MyObjectBuilder_CubeBlock>>
		{
			{ typeof(MyObjectBuilder_InteriorTurret), ProcessLargeTurretBase },
			{ typeof(MyObjectBuilder_LargeGatlingTurret), ProcessLargeTurretBase },
			{ typeof(MyObjectBuilder_LargeMissileTurret), ProcessLargeTurretBase },
			{ typeof(MyObjectBuilder_SmallGatlingGun), ProcessWeaponBlock },
			{ typeof(MyObjectBuilder_SmallMissileLauncher), ProcessWeaponBlock },
			{ typeof(MyObjectBuilder_SmallMissileLauncherReload), ProcessWeaponBlock },
		};

		private static MyObjectBuilder_CubeBlock ProcessLargeTurretBase(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{
				MyLargeTurretBaseDefinition myLargeTurret = (MyLargeTurretBaseDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(block.GetId());
				List<BlockSideEnum> mountPoints = myLargeTurret.MountPoints.Select(myMountPoint => myMountPoint.GetObjectBuilder(myMountPoint.Normal).Side).ToList();
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeTurretBase", $"Block {myLargeTurret.GetType()} targeted for replacement...");
				//myLargeTurret.MountPointLocalNormalToBlockLocal()
				List<WeaponInformation> replacementOptions = LargeGridWeaponTurretBases.FindAll(x =>
					//x.ModName != "Vanilla" &&
					x.SubtypeId != block.SubtypeId &&
					x.MountPoints.Intersect(mountPoints).Any() &&
					x.SizeX == myLargeTurret.Size.X &&
					x.SizeY == myLargeTurret.Size.Y //&&
					//x.SizeZ <= myLargeTurret.Size.Z + 1
					);
				if (replacementOptions.Count == 0) return block;
				block = CreateReplacementTurretBase(replacementOptions[Core.Random.Next(0, replacementOptions.Count)], block, size, myLargeTurret);
				//MyLargeTurretBaseDefinition myNewLargeTurret = (MyLargeTurretBaseDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(block.GetId());
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeTurretBase", $"Block {block.SubtypeId} has been replaced...");
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeTurretBase", $"Exception!\t{e}");
			}
			return block;
		}

		private static MyObjectBuilder_CubeBlock CreateReplacementTurretBase(WeaponInformation weaponInformation, MyObjectBuilder_CubeBlock block, MyCubeSize size, MyLargeTurretBaseDefinition myLargeTurret)
		{
			//weaponInformation.MyLargeTurret.GeneratedBlockType
			//MyObjectBuilder_CubeBlock myNewWeapon = new MyObjectBuilder_TurretBase()
			Core.GeneralLog.WriteToLog("CreateReplacementTurretBase-ProcessWeaponBlock", $"Block {weaponInformation.SubtypeId} chosen as the replacement...");
			MyObjectBuilder_CubeBlock myNewWeapon = MyObjectBuilderSerializer.CreateNewObject(
				weaponInformation.TypeId, weaponInformation.SubtypeName) as MyObjectBuilder_CubeBlock;
			if (myNewWeapon == null)
			{
				Core.GeneralLog.WriteToLog("CreateReplacementTurretBase", $"Replacement came up null...");
				return block;
			}
			((MyObjectBuilder_TurretBase)myNewWeapon).EntityId = block.EntityId;
			((MyObjectBuilder_TurretBase)myNewWeapon).SubtypeName = weaponInformation.SubtypeName;
			((MyObjectBuilder_TurretBase)myNewWeapon).IntegrityPercent = block.IntegrityPercent;
			((MyObjectBuilder_TurretBase)myNewWeapon).BuildPercent = block.BuildPercent;
			((MyObjectBuilder_TurretBase)myNewWeapon).Range = 2500;
			((MyObjectBuilder_TurretBase)myNewWeapon).Orientation = block.Orientation;
			((MyObjectBuilder_TurretBase)myNewWeapon).BlockOrientation = block.BlockOrientation;
			((MyObjectBuilder_TurretBase)myNewWeapon).ColorMaskHSV = block.ColorMaskHSV;
			((MyObjectBuilder_TurretBase)myNewWeapon).Min = block.Min;
			//((MyObjectBuilder_TurretBase)myNewWeapon).Name = "Retrofit " + ((MyObjectBuilder_TurretBase)myNewWeapon).Name;
			//((MyObjectBuilder_TurretBase)myNewWeapon).CustomName = "Retrofit " + ((MyObjectBuilder_TurretBase)myNewWeapon).CustomName;
			((MyObjectBuilder_TurretBase)myNewWeapon).ShowInTerminal = true;
			((MyObjectBuilder_TurretBase)myNewWeapon).ShowOnHUD = false;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetCharacters = ((MyObjectBuilder_TurretBase)block).TargetCharacters;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetLargeGrids = ((MyObjectBuilder_TurretBase)block).TargetLargeGrids;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetMeteors = ((MyObjectBuilder_TurretBase)block).TargetMeteors;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetMissiles = ((MyObjectBuilder_TurretBase)block).TargetMissiles;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetNeutrals = ((MyObjectBuilder_TurretBase)block).TargetNeutrals;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetSmallGrids = ((MyObjectBuilder_TurretBase)block).TargetSmallGrids;
			((MyObjectBuilder_TurretBase)myNewWeapon).TargetStations = ((MyObjectBuilder_TurretBase)block).TargetStations;
			((MyObjectBuilder_TurretBase)myNewWeapon).EnableIdleRotation = ((MyObjectBuilder_TurretBase)block).EnableIdleRotation;
			((MyObjectBuilder_TurretBase)myNewWeapon).Enabled = ((MyObjectBuilder_TurretBase)block).Enabled;
			((MyObjectBuilder_TurretBase)myNewWeapon).ComponentContainer = new MyObjectBuilder_ComponentContainer()
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
			};
			//((MyObjectBuilder_TurretBase)myNewWeapon).Inventory = new MyObjectBuilder_Inventory();
			//((MyObjectBuilder_TurretBase)myNewWeapon).GunBase = new MyObjectBuilder_GunBase();

			return myNewWeapon;
		}

		private static MyObjectBuilder_CubeBlock ProcessWeaponBlock(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{
				MyWeaponBlockDefinition myWeaponBlock = (MyWeaponBlockDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(block.GetId());
				List<BlockSideEnum> mountPoints = myWeaponBlock.MountPoints.Select(myMountPoint => myMountPoint.GetObjectBuilder(myMountPoint.Normal).Side).ToList();
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessWeaponBlock", $"Block {myWeaponBlock.GetType()} targeted for replacement...");
				//myLargeTurret.MountPointLocalNormalToBlockLocal()
				List<WeaponInformation> replacementOptions = LargeGridWeaponBlocks.FindAll(x =>
						//x.ModName != "Vanilla" &&
						x.SubtypeId != block.SubtypeId &&
						x.MountPoints.Intersect(mountPoints).Any() &&
						x.SizeX == myWeaponBlock.Size.X &&
						x.SizeY == myWeaponBlock.Size.Y //&&
					//x.SizeZ <= myLargeTurret.Size.Z + 1
				);
				if (replacementOptions.Count == 0) return block;
				block = CreateReplacementWeaponBlock(replacementOptions[Core.Random.Next(0, replacementOptions.Count)], block, size, myWeaponBlock);
				//MyWeaponBlockDefinition myNewLargeTurret = (MyWeaponBlockDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(block.GetId());
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessWeaponBlock", $"Block {block.SubtypeId} has been replaced...");
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessWeaponBlock", $"Exception!\t{e}");
			}
			return block;
		}

		private static MyObjectBuilder_CubeBlock CreateReplacementWeaponBlock(WeaponInformation weaponInformation, MyObjectBuilder_CubeBlock block, MyCubeSize size, MyWeaponBlockDefinition myWeaponBlock)
		{
			//weaponInformation.MyLargeTurret.GeneratedBlockType
			//MyObjectBuilder_CubeBlock myNewWeapon = new MyObjectBuilder_TurretBase()
			Core.GeneralLog.WriteToLog("CreateReplacementWeaponBlock-ProcessWeaponBlock", $"Block {weaponInformation.SubtypeId} chosen as the replacement...");
			MyObjectBuilder_CubeBlock myNewWeapon = MyObjectBuilderSerializer.CreateNewObject(
				weaponInformation.TypeId, weaponInformation.SubtypeName) as MyObjectBuilder_CubeBlock;
			if (myNewWeapon == null)
			{
				Core.GeneralLog.WriteToLog("CreateReplacementWeaponBlock", $"Replacement came up null...");
				return block;
			}
			((MyObjectBuilder_UserControllableGun)myNewWeapon).EntityId = block.EntityId;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).SubtypeName = weaponInformation.SubtypeName;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).IntegrityPercent = block.IntegrityPercent;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).BuildPercent = block.BuildPercent;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).Orientation = block.Orientation;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).BlockOrientation = block.BlockOrientation;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).ColorMaskHSV = block.ColorMaskHSV;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).Min = block.Min;
			//((MyObjectBuilder_UserControllableGun)myNewWeapon).Name = "Retrofit " + ((MyObjectBuilder_UserControllableGun)block).Name;
			//((MyObjectBuilder_UserControllableGun)myNewWeapon).CustomName = "Retrofit " + ((MyObjectBuilder_UserControllableGun)block).CustomName;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).ShowInTerminal = true;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).ShowOnHUD = false;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).Enabled = ((MyObjectBuilder_UserControllableGun)block).Enabled;
			((MyObjectBuilder_UserControllableGun)myNewWeapon).ComponentContainer = new MyObjectBuilder_ComponentContainer()
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
			};
			//((MyObjectBuilder_UserControllableGun)myNewWeapon).Inventory = new MyObjectBuilder_Inventory();
			//((MyObjectBuilder_UserControllableGun)myNewWeapon).GunBase = new MyObjectBuilder_GunBase();

			return myNewWeapon;
		}

		/*
	public static MyObjectBuilder_CubeBlock Upgrade(MyObjectBuilder_CubeBlock cubeBlock, MyObjectBuilderType newType, string newSubType)
	{
	  MyObjectBuilder_CubeBlock newObject = MyObjectBuilderSerializer.CreateNewObject(newType, newSubType) as MyObjectBuilder_CubeBlock;
	  if (newObject == null)
		return (MyObjectBuilder_CubeBlock) null;
	  newObject.EntityId = cubeBlock.EntityId;
	  newObject.Min = cubeBlock.Min;
	  newObject.m_orientation = cubeBlock.m_orientation;
	  newObject.IntegrityPercent = cubeBlock.IntegrityPercent;
	  newObject.BuildPercent = cubeBlock.BuildPercent;
	  newObject.BlockOrientation = cubeBlock.BlockOrientation;
	  newObject.ConstructionInventory = cubeBlock.ConstructionInventory;
	  newObject.ColorMaskHSV = cubeBlock.ColorMaskHSV;
	  return newObject;
	} 
			
			
		Full Code to build something from scratch in OB:
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

		private static MyObjectBuilder_CubeBlock ProcessLargeGatlingTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeGatlingTurret", $"Exception!\t{e}");
			}
			return block;
		}

		private static MyObjectBuilder_CubeBlock ProcessLargeMissileTurret(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // LargeTurretBase
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessLargeMissileTurret", $"Exception!\t{e}");
			}
			return block;
		}

		private static MyObjectBuilder_CubeBlock ProcessSmallGatlingGun(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // WeaponBlock
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessSmallGatlingGun", $"Exception!\t{e}");
			}
			return block;
		}

		private static MyObjectBuilder_CubeBlock ProcessSmallMissileLauncher(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // WeaponBlock
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessSmallMissileLauncher", $"Exception!\t{e}");
			}
			return block;
		}

		private static MyObjectBuilder_CubeBlock ProcessSmallMissileLauncherReload(MyObjectBuilder_CubeBlock block, Options options, MyCubeSize size)
		{ // WeaponBlock
			try
			{

			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("WeaponSwapper-ProcessSmallMissileLauncherReload", $"Exception!\t{e}");
			}
			return block;
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
