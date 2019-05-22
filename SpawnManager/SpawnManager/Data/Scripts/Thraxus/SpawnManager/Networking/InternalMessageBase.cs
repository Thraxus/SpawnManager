using System;
using ProtoBuf;
using SpawnManager.Common.Structs;

namespace SpawnManager.Networking
{
	[ProtoInclude(1, typeof(PrefabClass))]
	[Serializable, ProtoContract]
	public abstract class InternalMessageBase
	{

		
	}
}
