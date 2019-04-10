using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace SpawnManager.Drones
{
    public static class Drones
    {
        private static readonly Random DroneRandom = new Random();

	    private static bool _registered;


		private static void CallHelpRegister(IMyTerminalBlock obj)
        {
            if (!obj.IsFunctional) return;
            
            // Logic here needed to summon help
            
            //Definitions.PirateAntennaDefinition pirateAntennaDefinition;
            //if (!Definitions.PirateAntennaDefinitions.TryGetValue(obj.DisplayName, out pirateAntennaDefinition)) return;
            //Vector3D spawnLocation = obj.GetPosition() + pirateAntennaDefinition.SpawnDistance;
            // need to finish this part once it can be tested
        }

        private static void OnEntityAdd(IMyEntity entity)
        {
			try
            {
                IMyCubeGrid spawnedGrid = entity as IMyCubeGrid;
                if (spawnedGrid == null || spawnedGrid.IsRespawnGrid || spawnedGrid.Physics == null) return;

                List<IMySlimBlock> slimBlocks = new List<IMySlimBlock>();

                spawnedGrid.GetBlocks(slimBlocks, x => x.FatBlock is IMyTerminalBlock);
                foreach (IMySlimBlock block in slimBlocks.ToList())
                {
                    if (block.FatBlock.GetType() != typeof(MyObjectBuilder_RadioAntenna)) continue;
                    IMyRadioAntenna antenna = block.FatBlock as IMyRadioAntenna;
                    if (antenna == null || antenna.Enabled) return;
                    antenna.EnabledChanged += CallHelpRegister;
                }
            }
            catch (Exception e)
            {
                Core.GeneralLog.WriteToLog("Drones: OnEntityAdd", $"OnEntityAdd interrupted - exception:\t{e}");
            }
        }

        public static void Register()
        {
	        if (_registered) return;
            MyAPIGateway.Entities.OnEntityAdd += OnEntityAdd;
	        _registered = true;
	        Core.GeneralLog.WriteToLog("Drones", "Online...");
		}

        public static void Close()
        {
            MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdd;
	        Core.GeneralLog.WriteToLog("Drones", "Offline...");
		}

    }
}
