using ProtoBuf;
using Sandbox.Definitions;

namespace SpawnManager.Common.Structs
{
	//[ProtoInclude(999, typeof(MyObjectBuilder_Thrust))]
	//[MyObjectBuilderDefinition(null, null)]
	[ProtoContract]
	public class PrefabClass
	{
		//[ProtoMember(999)]
		//public readonly MyObjectBuilder_CubeGrid[] Prefab;

		//[ProtoMember(500, Options = MemberSerializationOptions.AsReferenceHasValue)]
		public readonly MyPrefabDefinition Prefab;

		public PrefabClass(MyPrefabDefinition prefab)
		{
			//Prefab = prefab.CubeGrids;
			Prefab = prefab;

		}
	}
}
