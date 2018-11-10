using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using SpawnManager.Support;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace SpawnManager.Tools
{
	public static class PrefabSpawner
	{ // Remaarn#0887 [Discord] is responsible for the orientation / location matrix code.  Made my life a lot easier!  Thanks!
		public static void SpawnPrefab(string prefabToSpawn, MatrixD spawnOrigin, Options options = null)
		{
			if (options == null)
				options = new Options();

			List<MyObjectBuilder_EntityBase> tempList = new List<MyObjectBuilder_EntityBase>();

			try
			{
				MyAPIGateway.Parallel.Start(delegate
				{
					List<MyObjectBuilder_Cockpit> myCockpitList = new List<MyObjectBuilder_Cockpit>();
					List<MyObjectBuilder_RemoteControl> myRemoteControlList = new List<MyObjectBuilder_RemoteControl>();
					MyPrefabDefinition prefab = MyDefinitionManager.Static.GetPrefabDefinition(prefabToSpawn);

					WeaponSwapper.ProcessPrefab(prefab, options);

					if (prefab.CubeGrids[0] == null)
						return;

					bool cubeGridZero = true;
					MatrixD mainGridInverseMatrix = MatrixD.Identity;
					MatrixD mainGridSpawnMatrix = MatrixD.Identity;

					MyAPIGateway.Entities.RemapObjectBuilderCollection(prefab.CubeGrids);

					foreach (MyObjectBuilder_CubeGrid gridBuilder in prefab.CubeGrids)
					{
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