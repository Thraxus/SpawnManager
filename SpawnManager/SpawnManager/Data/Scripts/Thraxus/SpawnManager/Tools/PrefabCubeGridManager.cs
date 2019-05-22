using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace SpawnManager.Tools
{
	[ProtoContract]
	//[ProtoContract]
	//[Serializable]
	//[XmlRoot("Dictionary")]
	public class PrefabCubeGridManager
	{
		//private readonly string _prefabName;

		//private readonly MyObjectBuilder_CubeGrid[] _cachedCubeGrids;

		//[ProtoMember(187)]
		//private readonly byte[] _serializedCubeGrid;

		//private readonly  deserializedCubeGrid[]

		////[ProtoMember(999)]
		////private MyPrefabDefinition prefab;
		////private MyPrefabDefinition _prefabMaster;

		////private MyPrefabDefinition _prefab;

		////private ImmutableList<ImmutableList<MyObjectBuilder_CubeBlock>> originalBlocks;

		//private string _prefabName;

		//private string serializeToXml;
		//private object ;
		//private byte[] SerializedGrid { get; }

		public PrefabCubeGridManager(string prefabName, MyPrefabDefinition prefab)
		{
			//MyObjectBuilder_PrefabDefinition myPre = (MyObjectBuilder_PrefabDefinition) prefab.GetObjectBuilder();
			//_serializedCubeGrid = MyAPIGateway.Utilities.SerializeToBinary(myPre);


			//SerializableDictionary<string, MyObjectBuilder_CubeGrid[]> serializableDictionary = new SerializableDictionary<string, MyObjectBuilder_CubeGrid[]>();
			//serializableDictionary.Dictionary.Add(prefabName, prefab.CubeGrids);

			//serializeToXml = MyAPIGateway.Utilities.SerializeToXML<SerializableDictionary<string, MyObjectBuilder_CubeGrid[]>>(serializableDictionary);


			//_prefabName = prefabName;
			//_prefab = prefab;
			//_cachedCubeGrids = (MyObjectBuilder_CubeGrid[]) prefab.CubeGrids.Clone();
			//PrefabClass prefabClass = new PrefabClass(prefab);
			//_prefabName = prefabName;
			//SerializableDictionary<string, MyPrefabDefinition> serializableDictionary = new SerializableDictionary<string, MyPrefabDefinition>();
			//serializableDictionary.Dictionary.Add(prefabName, prefab);
			//_serializedCubeGrid = MyAPIGateway.Utilities.SerializeToBinary(serializableDictionary);

			//SerializableDefinitionId serializable = new SerializableDefinitionId(typeof(MyObjectBuilder_CubeGrid), prefab.PrefabPath);

			////serializable.
			////MyCompression.Compress()
			//IMyXmlSerializable myXmlSerializable = new MyDefinitionXmlSerializer(prefab.GetObjectBuilder());

			
			//Core.GeneralLog.WriteToLog("PrefabCubeGridManager", $"Serializing {myXmlSerializable}");
			//MyObjectBuilderSerializer.Serializer.Serialize();

			//byte[] arr = new byte[] { };
			//using (Stream stream = arr)

			//	MemoryStream
			//Core.GeneralLog.WriteToLog("PrefabCubeGridManager", $"Serializing {prefab.PrefabPath}");

			//_serializedCubeGrid = MyAPIGateway.Utilities.SerializeToBinary(new PrefabClass(prefab));
			//_serializedCubeGrid = MyAPIGateway.Utilities.SerializeToBinary(new PrefabClass(prefab));
		}

		public IEnumerable<MyObjectBuilder_CubeGrid> GetCubeGrid()
		{
			return null;
			//MyAPIGateway.Parallel.Start(

			//	delegate { });
			//SerializableDictionary<string, MyObjectBuilder_CubeGrid[]> serializeFromXml = MyAPIGateway.Utilities.SerializeFromXML<SerializableDictionary<string, MyObjectBuilder_CubeGrid[]>>(serializeToXml);
			//Core.GeneralLog.WriteToLog("GetCubeGrid", $"Deserializing {serializeToXml.Length}");
			////SerializableDictionary<string, MyPrefabDefinition> serializeFromBinary = MyAPIGateway.Utilities.SerializeFromBinary<SerializableDictionary<string, MyPrefabDefinition>>(_serializedCubeGrid);
			////Core.GeneralLog.WriteToLog("GetCubeGrid", $"Deserialized {serializeFromBinary.Dictionary[_prefabName].DisplayNameString}");

			//return serializeFromXml.Dictionary[_prefabName];
			//return MyAPIGateway.Utilities.SerializeFromBinary<MyObjectBuilder_PrefabDefinition>(_serializedCubeGrid).CubeGrids;
			//return serializeFromBinary.Dictionary[_prefabName].CubeGrids;
			//Core.GeneralLog.WriteToLog("GetCubeGrid", $"About to clone {_cachedCubeGrids.Length}");
			//int i = 0;
			//List<MyObjectBuilder_CubeGrid> cubeGrids = new List<MyObjectBuilder_CubeGrid>();
			//foreach (MyObjectBuilder_CubeGrid cubeGrid in _cachedCubeGrids)
			//{
			//	List<MyObjectBuilder_CubeBlock> cubeBlocks = new List<MyObjectBuilder_CubeBlock>();
			//	cubeGrids[i].CubeBlocks = new List<MyObjectBuilder_CubeBlock>();
			//	foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrid.CubeBlocks)
			//	{
			//		cubeGrids[i].CubeBlocks.Add((MyObjectBuilder_CubeBlock) cubeBlock);
			//	}
			//	//Core.GeneralLog.WriteToLog("GetCubeGrid", $"adding to clone {cubeBlocks.Count}");
			//	//cubeGrids[i].CubeBlocks.Add(); = cubeBlocks;
			//	Core.GeneralLog.WriteToLog("GetCubeGrid", $"added to clone {cubeGrids.Count} {i}");
			//	i++;
			//}
			//Core.GeneralLog.WriteToLog("GetCubeGrid", $"Returning: {cubeGrids.Count}");
			//return cubeGrids;

			////foreach (MyObjectBuilder_CubeGrid tmpCubeGrid in _cachedCubeGrids)
			////{
			////	ExposedCubeGrids.Add((MyObjectBuilder_CubeGrid)MyObjectBuilderSerializer.Clone(tmpCubeGrid));
			////}


			////MyObjectBuilder_CubeGrid[] deSerializedGrid = MyAPIGateway.Utilities.SerializeFromBinary<MyObjectBuilder_CubeGrid[]>(SerializedGrid);
			////PrefabClass deSerializedGrid = MyAPIGateway.Utilities.SerializeFromBinary<PrefabClass>(SerializedGrid);
			//MyPrefabDefinition decoded = _prefab as MyPrefabDefinition;//deSerializedGrid.Prefab as MyPrefabDefinition;
			//Core.GeneralLog.WriteToLog("GetCubeGrid", $"Well... {decoded?.CubeGrids}");
			//return decoded;
			////int x = 0;
			////MyObjectBuilder_CubeGrid[] exposedCubeGrids = new MyObjectBuilder_CubeGrid[_cachedCubeGrids.Length];
			////foreach (MyObjectBuilder_CubeGrid cachedGrid in _cachedCubeGrids)
			////{
			////	exposedCubeGrids[x] = (MyObjectBuilder_CubeGrid) cachedGrid.Clone();
			////	exposedCubeGrids[x].CubeBlocks.Clear();
			////	foreach (MyObjectBuilder_CubeBlock cachedGridCube in cachedGrid.CubeBlocks)
			////	{
			////		exposedCubeGrids[x].CubeBlocks.Add((MyObjectBuilder_CubeBlock)cachedGridCube.Clone());
			////	}
			////	x++;
			////}
			////return exposedCubeGrids;
			////ExposedCubeGrids.Clone()
			////_cachedCubeGrids.CopyTo(ExposedCubeGrids,0);
			////ExposedCubeGrids = new List<MyObjectBuilder_CubeGrid>();
			////foreach (MyObjectBuilder_CubeGrid tmpCubeGrid in _cachedCubeGrids)
			////{
			////	ExposedCubeGrids.Add((MyObjectBuilder_CubeGrid)MyObjectBuilderSerializer.Clone(tmpCubeGrid));
			////}
		}

		
	}
}



//int i = 0;
//MyObjectBuilder_CubeGrid[] prefabs = new MyObjectBuilder_CubeGrid[originalPrefab.CubeGrids.Length];
//foreach (MyObjectBuilder_CubeGrid cubeGrid in originalPrefab.CubeGrids)
//{
//	List<MyObjectBuilder_CubeBlock> cubeBlocks = new List<MyObjectBuilder_CubeBlock>();
//	//prefabs[i].CubeBlocks = new List<MyObjectBuilder_CubeBlock>();
//	foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrid.CubeBlocks)
//	{
//		cubeBlocks.Add((MyObjectBuilder_CubeBlock)cubeBlock.Clone());
//	}
//	prefabs[i].CubeBlocks = cubeBlocks;
//	i++;
//}




//Core.GeneralLog.WriteToLog("SpawnPrefab", $"Checking the dictionary...");
//List<MyObjectBuilder_CubeGrid> prefabs = Definitions.PrefabDictionary[prefabToSpawn].GetCubeGrid().ToList();




//MyPrefabDefinition prefabs = Definitions.PrefabDictionary[prefabToSpawn].GetCubeGrid();// .GetCubeGrid();

//List<ImmutableList<MyObjectBuilder_CubeBlock>> prefabs = Definitions.PrefabDictionary[prefabToSpawn];

//if (prefabs.Length == 0)
//{
//	Core.GeneralLog.WriteToLog("SpawnPrefab", $"Dictionary Lookup Failed...");
//	return;
//}





//Core.GeneralLog.WriteToLog("SpawnPrefab", $"Dictionary Lookup Passed...");
//ImmutableList<MyObjectBuilder_CubeGrid> prefabs = originalPrefab.CubeGrids.ToImmutableList();

//List<MyObjectBuilder_CubeGrid> prefabs = new List<MyObjectBuilder_CubeGrid>();
//foreach (MyObjectBuilder_CubeGrid tmpCubeGrid in originalPrefab.CubeGrids)
//{
//	prefabs.Add((MyObjectBuilder_CubeGrid)MyObjectBuilderSerializer.Clone(tmpCubeGrid));
//}


//MyDefinitionManager.Static.ReloadPrefabsFromFile(MyDefinitionManager.Static.GetPrefabDefinition(prefabToSpawn).PrefabPath);



//Definitions.PrefabDictionary[prefabToSpawn].GetCubeGrid();
//MyDefinitionManager.Static.ReloadPrefabsFromFile(MyDefinitionManager.Static.GetPrefabDefinition(prefabToSpawn).PrefabPath);



//MyObjectBuilder_PrefabDefinition prefabClone = GetClone(prefab);



//PrefabDictionary = new Dictionary<string, PrefabCubeGridManager>();

//foreach (KeyValuePair<string, MyPrefabDefinition> prefab in MyDefinitionManager.Static.GetPrefabDefinitions())
//{
//PrefabDictionary.Add(prefab.Key, new PrefabCubeGridManager(prefab.Key, prefab.Value));
//}

//foreach (KeyValuePair<string, MyPrefabDefinition> prefab in MyDefinitionManager.Static.GetPrefabDefinitions())
//{
//	List<ImmutableList<MyObjectBuilder_CubeBlock>> tmpList = new List<ImmutableList<MyObjectBuilder_CubeBlock>>();
//	foreach (MyObjectBuilder_CubeGrid cubeGrid in prefab.Value.CubeGrids)
//	{
//		ImmutableList<MyObjectBuilder_CubeBlock>.Builder listBuilder = ImmutableList.CreateBuilder<MyObjectBuilder_CubeBlock>();
//		listBuilder.AddRange(cubeGrid.CubeBlocks);
//		tmpList.Add(listBuilder.ToImmutable());
//	}
//	PrefabDictionary.Add(prefab.Key, tmpList);
//}

//ImmutableDictionary<string, ImmutableList<MyObjectBuilder_CubeGrid>>.Builder builder = ImmutableDictionary.CreateBuilder<string, ImmutableList<MyObjectBuilder_CubeGrid>>();

//foreach (KeyValuePair<string, MyPrefabDefinition> prefab in MyDefinitionManager.Static.GetPrefabDefinitions())
//{
//	builder.Add(prefab.Key, new List<MyObjectBuilder_CubeGrid>(prefab.Value.CubeGrids).ToImmutableList());
//}

//PrefabDictionary2 = builder.ToImmutable();