using System.Collections.Generic;
using Sandbox.ModAPI;
using SpawnManager.Tools;
using VRageMath;

namespace SpawnManager.DebugTools
{
    public static class SpawnTestClass
    {
	    public static void SpawnTestCaseModded(string s)
	    {
		    
		}
		
		public static void SpawnTestCase(string s)
        {

		}

		public static void SpawnEemTradePrefabs(string s)
		{
			List<string> tradePrefabs = new List<string>
			{
				"Dromedary_Trader", "Helios_Wheel_Trader", "Phaeton_Trading_Outpost", "PE_XMC_718_Trading_Outpost", "PE_XMC_603_Factory",
				"PE_XMC_99_Refinery", "PE_IMDC_1781_Service_Platform", "PE_Mahriane_8724_Service_Platform", "PE_Mahriane_34_Trading_Outpost",
				"PE_Mahriane_56_Trading_Outpost"
			};

			MatrixD initialMatrixD = MyAPIGateway.Session.Player.Character.WorldMatrix;
			initialMatrixD.Translation += initialMatrixD.Forward * 50;

			MyAPIGateway.Parallel.Start(delegate
			{
				for (int i = 0; i < (tradePrefabs.Count / 5); i++)
				{
					initialMatrixD.Translation += initialMatrixD.Down * (100 * i);
					for (int j = 0; j < 5; j++)
					{
						if (i % 2 == 0)
							initialMatrixD.Translation += initialMatrixD.Right * (100 * j);
						else
							initialMatrixD.Translation += initialMatrixD.Left * (100 * j);

						PrefabSpawner.SpawnPrefab(tradePrefabs[i], initialMatrixD);
						//MyAPIGateway.Parallel.Sleep(1000);
					}
				}
			});
		}

		public static void SpawnGroup(string group)
		{

		}

		private static void SpawnPrefab(string prefab, MatrixD position)
        {
	        
		}

	    public static void SpawnPrefab(string prefab)
	    {
		    
	    }
	}
}
