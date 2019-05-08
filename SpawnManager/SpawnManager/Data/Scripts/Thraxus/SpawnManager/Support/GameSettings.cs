using Sandbox.ModAPI;

namespace SpawnManager.Support
{
    public static class GameSettings
    {
        public static void Register()
        {
            MyAPIGateway.Session.SessionSettings.CargoShipsEnabled = false;
            MyAPIGateway.Session.SessionSettings.EnableIngameScripts = true;
            MyAPIGateway.Session.SessionSettings.EnableEncounters = false;
            MyAPIGateway.Session.SessionSettings.EnableDrones = false;
            if (MyAPIGateway.Session.SessionSettings.SyncDistance <= 3000)
                MyAPIGateway.Session.SessionSettings.SyncDistance = 10000;
            if (MyAPIGateway.Session.SessionSettings.PiratePCU <= 50000)
	            MyAPIGateway.Session.SessionSettings.PiratePCU = 100000;

            if (MyAPIGateway.Session.SessionSettings.TotalPCU <= 100000 && MyAPIGateway.Session.SessionSettings.TotalPCU != 0)
	            MyAPIGateway.Session.SessionSettings.TotalPCU = 200000;
		}
    }
}
