using Sandbox.ModAPI;
using VRageMath;

namespace SpawnManager.SpawnManager
{
    public static class SpawnTestClass
    {
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
	        PrefabSpawner.SpawnPrefab(prefab, position); 
		}

	    public static void SpawnPrefab(string prefab)
	    {
		    if (!prefab.StartsWith("/spawner")) return;
		    const string prefix = "/spawner spawnprefab ";
		    MatrixD initialMatrixD = MyAPIGateway.Session.Player.Character.WorldMatrix;
		    initialMatrixD.Translation += initialMatrixD.Forward * 250;
			PrefabSpawner.SpawnPrefab(prefab.Replace(prefix, "").Trim(), initialMatrixD);
	    }
	}
}
