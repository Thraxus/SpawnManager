using System;
using System.Collections.Generic;
using System.Threading;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpawnManager.Support;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace SpawnManager.Tools
{

	public static class PrefabSpawner
	{ // Remaarn#0887 [Discord] is responsible for the orientation / location matrix code.  Made my life a lot easier!  Thanks!

		public static void SpawnPrefab(MyPrefabDefinition prefab)
		{
			MyAPIGateway.Parallel.Start(delegate
			{
				MyAPIGateway.Entities.RemapObjectBuilderCollection(prefab.CubeGrids);
				foreach (MyObjectBuilder_CubeGrid grid in prefab.CubeGrids)
					MyAPIGateway.Entities.CreateFromObjectBuilderParallel(grid, true);
			});
		}

		public static void SpawnPrefab(MyObjectBuilder_CubeGrid[] grids)
		{
			MyAPIGateway.Parallel.Start(delegate
			{
				MyAPIGateway.Entities.RemapObjectBuilderCollection(grids);
				foreach (MyObjectBuilder_CubeGrid grid in grids)
					MyAPIGateway.Entities.CreateFromObjectBuilderParallel(grid, true);
			});
		}

		public static void SpawnSpawmGroup(string spawnGroup, MatrixD spawnOrigin, Options options = null)
		{
			MyAPIGateway.Parallel.Start(delegate
			{
				List<MySpawnGroupDefinition> spawnGroupDefinitions = new List<MySpawnGroupDefinition>(MyDefinitionManager.Static.GetSpawnGroupDefinitions());
				MySpawnGroupDefinition spawnGroupDefinition = spawnGroupDefinitions.Find(x => x.Id.SubtypeName == spawnGroup);
				foreach (MySpawnGroupDefinition.SpawnGroupPrefab spawnGroupPrefab in spawnGroupDefinition.Prefabs)
				{
					Vector3D prefabSpawnPos = Vector3D.Transform(spawnGroupPrefab.Position, spawnOrigin);
					MatrixD prefabSpawnMatrix = spawnOrigin;
					prefabSpawnMatrix.Translation = prefabSpawnPos;
					SpawnPrefab(spawnGroupPrefab.SubtypeId, prefabSpawnMatrix, options);
					MyAPIGateway.Parallel.Sleep(1000);
				}
			});
		}

		public static void SpawnPrefab(string prefabToSpawn, MatrixD spawnOrigin, Options options = null)
		{
			if (options == null)
				options = new Options();

			Core.GeneralLog.WriteToLog("SpawnPrefab",$"Spawning... {prefabToSpawn} {options.PreservePrograms}");

			List<MyObjectBuilder_EntityBase> tempList = new List<MyObjectBuilder_EntityBase>();
			try
			{
		
				MyAPIGateway.Parallel.Start(delegate
				{
					List<MyObjectBuilder_Cockpit> myCockpitList = new List<MyObjectBuilder_Cockpit>();
					List<MyObjectBuilder_RemoteControl> myRemoteControlList = new List<MyObjectBuilder_RemoteControl>();

					//WeaponSwapper.ProcessPrefab(prefab, options);
					
					bool cubeGridZero = true;
					MatrixD mainGridInverseMatrix = MatrixD.Identity;
					MatrixD mainGridSpawnMatrix = MatrixD.Identity;

					MyPrefabDefinition prefabs = MyDefinitionManager.Static.GetPrefabDefinition(prefabToSpawn);
					if (prefabs.CubeGrids[0] == null)
					return;


					MyAPIGateway.Entities.RemapObjectBuilderCollection(prefabs.CubeGrids);

					foreach (MyObjectBuilder_CubeGrid gridBuilderOrig in prefabs.CubeGrids)
					{
						MyObjectBuilder_CubeGrid gridBuilder = (MyObjectBuilder_CubeGrid) gridBuilderOrig.Clone(); // TODO adds lag, need to fix?  problem is all cube blocks need to be cloned.  

						//MyObjectBuilderSerializer

						tempList.Add(gridBuilder);

						options.EntityId = gridBuilder.EntityId;
						CubeProcessing.GeneralGridSettings(gridBuilder, options);

						foreach (MyObjectBuilder_CubeBlock block in gridBuilder.CubeBlocks)
						{
							if (block == null)
								continue;
							CubeProcessing.GeneralBlockSettings(block, options);

							Action<MyObjectBuilder_CubeBlock, Options, MyCubeSize> action;
							CubeProcessing.CubeBlockProcessing.TryGetValue(block.GetType(), out action);
							action?.Invoke(block, options, gridBuilder.GridSizeEnum);

							if (!cubeGridZero) continue;
							if (block.GetType() == typeof(MyObjectBuilder_Cockpit)) myCockpitList.Add(block as MyObjectBuilder_Cockpit);
							if (block.GetType() == typeof(MyObjectBuilder_RemoteControl)) myRemoteControlList.Add(block as MyObjectBuilder_RemoteControl);
						}

						MyPositionAndOrientation gridPositionAndOrientation = gridBuilder.PositionAndOrientation ?? MyPositionAndOrientation.Default;
						MatrixD gridTransform = gridPositionAndOrientation.GetMatrix();
						MatrixD subGridOffset = MatrixD.Identity;

						if (cubeGridZero)
						{
							mainGridInverseMatrix = MatrixD.Invert(ref gridTransform);
							mainGridSpawnMatrix = spawnOrigin;

							MyObjectBuilder_CubeBlock mainOrientationBlock = GetMainOrientationBlock(myCockpitList, myRemoteControlList);

							if (mainOrientationBlock != null)
							{
								Matrix blockMatrix;
								((MyBlockOrientation)mainOrientationBlock.BlockOrientation).GetMatrix(out blockMatrix);
								blockMatrix.Translation = ((Vector3I)mainOrientationBlock.Min) * (gridBuilder.GridSizeEnum == MyCubeSize.Large ? 2.5f : 0.5f);

								Matrix inverseBlockMatrix = Matrix.Invert(ref blockMatrix);
								mainGridSpawnMatrix = inverseBlockMatrix * mainGridSpawnMatrix;
							}
						}
						else
						{
							subGridOffset = gridTransform * mainGridInverseMatrix;
						}

						if (gridBuilder.PositionAndOrientation != null)
						{
							MatrixD gridSpawnMatrix = subGridOffset * mainGridSpawnMatrix;
							gridBuilder.PositionAndOrientation = new MyPositionAndOrientation(ref gridSpawnMatrix);
						}
						cubeGridZero = false;
					}
					tempList.ForEach(item => MyAPIGateway.Entities.CreateFromObjectBuilderParallel(item, true));
					//foreach (MyObjectBuilder_EntityBase item in tempList) MyAPIGateway.Entities.CreateFromObjectBuilderParallel(item, true);
					//MyAPIGateway.Multiplayer.SendEntitiesCreated(tempList); // may need to uncomment this if entities aren't syncing
					tempList.Clear();
					myCockpitList.Clear();
					myRemoteControlList.Clear();
				});
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("prefabToSpawn", $"Prefab Processing Exception! {e}");
			}
		}

		private static MyObjectBuilder_PrefabDefinition GetClone(MyPrefabDefinition prefab)
		{
			try
			{

				MyObjectBuilder_PrefabDefinition prefabDefinitionBase = (MyObjectBuilder_PrefabDefinition) MyObjectBuilderSerializer.Clone(((MyObjectBuilder_PrefabDefinition)prefab.GetObjectBuilder()));
				
				List<MyObjectBuilder_CubeGrid> tmpCubes = new List<MyObjectBuilder_CubeGrid>();
				foreach (MyObjectBuilder_CubeGrid tmpCubeGrid in prefab.CubeGrids)
				{
					tmpCubes.Add((MyObjectBuilder_CubeGrid) MyObjectBuilderSerializer.Clone(tmpCubeGrid));
				}

				Core.GeneralLog.WriteToLog("GetClone", $"tmpCubes {tmpCubes.Count}");
				foreach (MyObjectBuilder_CubeGrid prefabX in tmpCubes)
				{
					Core.GeneralLog.WriteToLog("GetClone", $"prefabX: {prefabX.DisplayName}");
				}

				//MyObjectBuilder_PrefabDefinition prefabDefinitionBase = (MyObjectBuilder_PrefabDefinition) prefab.GetObjectBuilder().Clone();
				//Core.GeneralLog.WriteToLog("GetClone", $"Results: {prefabDefinitionBase} {prefabDefinitionBase.SubtypeName} {prefabDefinitionBase.SubtypeId} {prefabDefinitionBase.TypeId} {prefabDefinitionBase.GetType()}");
				//Core.GeneralLog.WriteToLog("GetClone", $"Cloned: {prefabDefinitionBase.PrefabPath}");


				//MyObjectBuilder_PrefabDefinition myObjectBuilderBase = MyObjectBuilderSerializer.Clone(prefab.GetObjectBuilder()) as MyObjectBuilder_PrefabDefinition;
				//Core.GeneralLog.WriteToLog("GetClone", $"Cloned {myObjectBuilderBase == null}");
				//Core.GeneralLog.WriteToLog("GetClone", $"Cloned {myObjectBuilderBase.}");
				return null;
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog("GetClone", $"Prefab Processing Exception! {e}");
				return null;
			}
		}

		private static MyObjectBuilder_CubeBlock GetMainOrientationBlock(IReadOnlyList<MyObjectBuilder_Cockpit> myCockpitList, IReadOnlyList<MyObjectBuilder_RemoteControl> myRemoteControlList)
		{
			MyObjectBuilder_CubeBlock mainOrientationBlock = null;

			if (myCockpitList.Count > 0)
			{
				foreach (MyObjectBuilder_Cockpit cockpit in myCockpitList)
				{
					if (!cockpit.IsMainCockpit) continue;
					mainOrientationBlock = cockpit;
					break;
				}

				if (mainOrientationBlock == null)
					mainOrientationBlock = myCockpitList[0];
			}
			else if (myRemoteControlList.Count > 0)
			{
				foreach (MyObjectBuilder_RemoteControl remoteControl in myRemoteControlList)
				{
					if (!remoteControl.IsMainRemoteControl) continue;
					mainOrientationBlock = remoteControl;
					break;
				}

				if (mainOrientationBlock == null)
					mainOrientationBlock = myRemoteControlList[0];
			}
			return mainOrientationBlock;
		}
	}
}

//// Decompiled with JetBrains decompiler
//// Type: Sandbox.Definitions.MySpawnGroupDefinition
//// Assembly: Sandbox.Game, Version=0.1.7058.25453, Culture=neutral, PublicKeyToken=null
//// MVID: 78A37FCC-88C1-4EF0-8C3B-0CF1E189CCDA
//// Assembly location: E:\SteamLibrary\steamapps\common\SpaceEngineers\Bin64\Sandbox.Game.dll

//using System;
//using System.Collections.Generic;
//using VRage.Game;
//using VRage.Game.Definitions;
//using VRageMath;

//namespace Sandbox.Definitions
//{
//	[MyDefinitionType(typeof(MyObjectBuilder_SpawnGroupDefinition), null)]
//	public class MySpawnGroupDefinition : MyDefinitionBase
//	{
//		public List<MySpawnGroupDefinition.SpawnGroupPrefab> Prefabs = new List<MySpawnGroupDefinition.SpawnGroupPrefab>();
//		public List<MySpawnGroupDefinition.SpawnGroupVoxel> Voxels = new List<MySpawnGroupDefinition.SpawnGroupVoxel>();
//		public float Frequency;
//		private float m_spawnRadius;
//		private bool m_initialized;
//		public bool IsPirate;
//		public bool IsEncounter;
//		public bool IsCargoShip;
//		public bool ReactorsOn;

//		public float SpawnRadius
//		{
//			get
//			{
//				if (!this.m_initialized)
//					this.ReloadPrefabs();
//				return this.m_spawnRadius;
//			}
//			private set
//			{
//				this.m_spawnRadius = value;
//			}
//		}

//		public bool IsValid
//		{
//			get
//			{
//				if ((double)this.Frequency != 0.0 && (double)this.m_spawnRadius != 0.0)
//					return (uint)this.Prefabs.Count > 0U;
//				return false;
//			}
//		}

//		protected override void Init(MyObjectBuilder_DefinitionBase baseBuilder)
//		{
//			base.Init(baseBuilder);
//			MyObjectBuilder_SpawnGroupDefinition spawnGroupDefinition = baseBuilder as MyObjectBuilder_SpawnGroupDefinition;
//			this.Frequency = spawnGroupDefinition.Frequency;
//			if ((double)this.Frequency == 0.0)
//			{
//				MySandboxGame.Log.WriteLine("Spawn group initialization: spawn group has zero frequency");
//			}
//			else
//			{
//				this.SpawnRadius = 0.0f;
//				BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
//				this.Prefabs.Clear();
//				foreach (MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroupDefinition.Prefabs)
//				{
//					MySpawnGroupDefinition.SpawnGroupPrefab spawnGroupPrefab = new MySpawnGroupDefinition.SpawnGroupPrefab();
//					spawnGroupPrefab.Position = prefab.Position;
//					spawnGroupPrefab.SubtypeId = prefab.SubtypeId;
//					spawnGroupPrefab.BeaconText = prefab.BeaconText;
//					spawnGroupPrefab.Speed = prefab.Speed;
//					spawnGroupPrefab.ResetOwnership = prefab.ResetOwnership;
//					spawnGroupPrefab.PlaceToGridOrigin = prefab.PlaceToGridOrigin;
//					spawnGroupPrefab.Behaviour = prefab.Behaviour;
//					spawnGroupPrefab.BehaviourActivationDistance = prefab.BehaviourActivationDistance;
//					if (MyDefinitionManager.Static.GetPrefabDefinition(spawnGroupPrefab.SubtypeId) == null)
//					{
//						MySandboxGame.Log.WriteLine("Spawn group initialization: Could not get prefab " + spawnGroupPrefab.SubtypeId);
//						return;
//					}
//					this.Prefabs.Add(spawnGroupPrefab);
//				}
//				this.Voxels.Clear();
//				if (spawnGroupDefinition.Voxels != null)
//				{
//					foreach (MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel voxel in spawnGroupDefinition.Voxels)
//						this.Voxels.Add(new MySpawnGroupDefinition.SpawnGroupVoxel()
//						{
//							Offset = voxel.Offset,
//							StorageName = voxel.StorageName,
//							CenterOffset = voxel.CenterOffset
//						});
//				}
//				this.SpawnRadius = boundingSphere.Radius + 5f;
//				this.IsEncounter = spawnGroupDefinition.IsEncounter;
//				this.IsCargoShip = spawnGroupDefinition.IsCargoShip;
//				this.IsPirate = spawnGroupDefinition.IsPirate;
//				this.ReactorsOn = spawnGroupDefinition.ReactorsOn;
//			}
//		}

//		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
//		{
//			MyObjectBuilder_SpawnGroupDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SpawnGroupDefinition;
//			objectBuilder.Frequency = this.Frequency;
//			objectBuilder.Prefabs = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[this.Prefabs.Count];
//			int index1 = 0;
//			foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in this.Prefabs)
//			{
//				objectBuilder.Prefabs[index1] = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab();
//				objectBuilder.Prefabs[index1].BeaconText = prefab.BeaconText;
//				objectBuilder.Prefabs[index1].SubtypeId = prefab.SubtypeId;
//				objectBuilder.Prefabs[index1].Position = prefab.Position;
//				objectBuilder.Prefabs[index1].Speed = prefab.Speed;
//				objectBuilder.Prefabs[index1].ResetOwnership = prefab.ResetOwnership;
//				objectBuilder.Prefabs[index1].PlaceToGridOrigin = prefab.PlaceToGridOrigin;
//				objectBuilder.Prefabs[index1].Behaviour = prefab.Behaviour;
//				objectBuilder.Prefabs[index1].BehaviourActivationDistance = prefab.BehaviourActivationDistance;
//				++index1;
//			}
//			objectBuilder.Voxels = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[this.Voxels.Count];
//			int index2 = 0;
//			foreach (MySpawnGroupDefinition.SpawnGroupVoxel voxel in this.Voxels)
//			{
//				objectBuilder.Voxels[index2] = new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel();
//				objectBuilder.Voxels[index2].Offset = voxel.Offset;
//				objectBuilder.Voxels[index2].CenterOffset = voxel.CenterOffset;
//				objectBuilder.Voxels[index2].StorageName = voxel.StorageName;
//				++index2;
//			}
//			objectBuilder.IsCargoShip = this.IsCargoShip;
//			objectBuilder.IsEncounter = this.IsEncounter;
//			objectBuilder.IsPirate = this.IsPirate;
//			objectBuilder.ReactorsOn = this.ReactorsOn;
//			return (MyObjectBuilder_DefinitionBase)objectBuilder;
//		}

//		public void ReloadPrefabs()
//		{
//			BoundingSphere boundingSphere1 = new BoundingSphere(Vector3.Zero, float.MinValue);
//			float val1 = 0.0f;
//			foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in this.Prefabs)
//			{
//				MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefab.SubtypeId);
//				if (prefabDefinition == null)
//				{
//					MySandboxGame.Log.WriteLine("Spawn group initialization: Could not get prefab " + prefab.SubtypeId);
//					return;
//				}
//				BoundingSphere boundingSphere2 = prefabDefinition.BoundingSphere;
//				boundingSphere2.Center += prefab.Position;
//				boundingSphere1.Include(boundingSphere2);
//				if (prefabDefinition.CubeGrids != null)
//				{
//					foreach (MyObjectBuilder_CubeGrid cubeGrid in prefabDefinition.CubeGrids)
//					{
//						float cubeSize = MyDefinitionManager.Static.GetCubeSize(cubeGrid.GridSizeEnum);
//						val1 = Math.Max(val1, 2f * cubeSize);
//					}
//				}
//			}
//			this.SpawnRadius = boundingSphere1.Radius + val1;
//			this.m_initialized = true;
//		}

//		public struct SpawnGroupPrefab
//		{
//			public Vector3 Position;
//			public string SubtypeId;
//			public string BeaconText;
//			public float Speed;
//			public bool ResetOwnership;
//			public bool PlaceToGridOrigin;
//			public string Behaviour;
//			public float BehaviourActivationDistance;
//		}

//		public struct SpawnGroupVoxel
//		{
//			public Vector3 Offset;
//			public bool CenterOffset;
//			public string StorageName;
//		}
//	}
//}
