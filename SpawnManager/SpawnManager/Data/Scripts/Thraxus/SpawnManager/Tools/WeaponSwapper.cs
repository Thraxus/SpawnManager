using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using SpawnManager.Support;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SpawnManager.Tools
{
	internal class WeaponSwapper
	{
		/* Rules:
		 *	x and y must be the same, z can be any size up to max of original block
		 *	mount points must be on the same side
		 *
		 *	MyObjectBuilder_WeaponBlockDefinition
		 *	MyObjectBuilder_LargeTurretBaseDefinition
		 */

		public void Junk()
		{
			//MyWeaponBlockDefinition myWeapon = new MyWeaponBlockDefinition();
			//MyLargeTurretBaseDefinition myLargeTurret = new MyLargeTurretBaseDefinition();
			//MyCubeBlockDefinition myCube = new MyCubeBlockDefinition();
			//myCube.
			//myCube.Skeleton
			//MyCubeBlockDefinition.MountPoint mountPoint = new MyCubeBlockDefinition.MountPoint();
			//MyObjectBuilder_CubeBlockDefinition.MountPoint objectBuilder = myCube.MountPoints[0].GetObjectBuilder(myCube.MountPoints[0].Normal);
			//BlockSideEnum objectBuilderSide =  objectBuilder.Side;
			//myWeapon.MountPoints[0].
			
		}

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
