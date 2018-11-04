using Sandbox.ModAPI;

namespace SpawnManager.SpawnManager
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
        }
    }
}
