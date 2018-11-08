using Sandbox.ModAPI;
using SpawnManager.Support;
using SpawnManager.Tools;
using VRageMath;

namespace SpawnManager.DebugTools
{
    public static class SpawnTestClass
    {
	    public static void SpawnTestCaseModded(string s)
	    {
		    MatrixD initialMatrixD = MyAPIGateway.Session.Player.Character.WorldMatrix;
			initialMatrixD.Translation += initialMatrixD.Forward * 50;
		    SpawnPrefab("SubGridTestHell-Modded", initialMatrixD);
		}
		
		public static void SpawnTestCase(string s)
        {
	        MatrixD initialMatrixD = MyAPIGateway.Session.Player.Character.WorldMatrix;
			
			initialMatrixD.Translation += initialMatrixD.Forward * 250;
			SpawnPrefab("SubGridTestHell", initialMatrixD);
			initialMatrixD.Translation += initialMatrixD.Up * 100;
			SpawnPrefab("SubGridTestHell-UpsideDownCockpit", initialMatrixD);
			initialMatrixD.Translation += initialMatrixD.Left * 100;
			SpawnPrefab("SubGridTestHell-ModifiedConnected", initialMatrixD);
			initialMatrixD.Translation += initialMatrixD.Down * 100;
			SpawnPrefab("SubGridTestHell-Modified", initialMatrixD);
			initialMatrixD.Translation += initialMatrixD.Down * 100;
		}

		private static void SpawnPrefab(string prefab, MatrixD position)
        {
	        Options options = new Options { Restock = true};
			PrefabSpawner.SpawnPrefab(prefab, position, options); 
		}

	    public static void SpawnPrefab(string prefab)
	    {
		    const string prefix = "spawnprefab";
			if (!prefab.StartsWith(prefix)) return;
		    Options options = new Options { Restock = true };
			MatrixD initialMatrixD = MyAPIGateway.Session.Player.Character.WorldMatrix;
		    initialMatrixD.Translation += initialMatrixD.Forward * 250;
			PrefabSpawner.SpawnPrefab(prefab.Replace(prefix, "").Trim(), initialMatrixD, options);
	    }
	}
}
