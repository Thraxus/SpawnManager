using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace SpawnManager.Support
{
	public static class PostProcessing
	{
		private const string ModuleName = "PostProcessing";
		private static bool _registered;

		private static void OnEntityAdd(IMyEntity entity)
		{
			Core.GeneralLog.WriteToLog($"{ModuleName}: OnEntityAdd", $"OnEntityAdd Processing Entity {entity?.EntityId}");
			try
			{
				List<CubeProcessing.PbReplacement> pbReplacement;
				if (!CubeProcessing.PbPrograms.TryGetValue(entity.EntityId, out pbReplacement)) return;
				List<IMyProgrammableBlock> myProgrammableBlocks = new List<IMyProgrammableBlock>();
				IMyGridTerminalSystem myTerminalActionsHelper = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(entity as IMyCubeGrid);
				myTerminalActionsHelper.GetBlocksOfType(myProgrammableBlocks);

				foreach (CubeProcessing.PbReplacement tmpPbReplacement in pbReplacement)
				{
					Core.GeneralLog.WriteToLog($"{ModuleName}: OnEntityAdd", $"OnEntityAdd DebugCheck {tmpPbReplacement.NewName}");
				}

				foreach (IMyProgrammableBlock myProgrammableBlock in myProgrammableBlocks)
				{
					Core.GeneralLog.WriteToLog($"{ModuleName}: OnEntityAdd", $"OnEntityAdd DebugCheckLoop {myProgrammableBlock.CustomName}");
					CubeProcessing.PbReplacement tempPbReplacement = pbReplacement.FirstOrDefault(x => x.NewName == myProgrammableBlock.CustomName);
					myProgrammableBlock.CustomName = tempPbReplacement.OriginalName;
					myProgrammableBlock.ProgramData = tempPbReplacement.Program;
					pbReplacement.Remove(tempPbReplacement);
					if (pbReplacement.Count != 0) continue;
					Core.GeneralLog.WriteToLog($"{ModuleName}: OnEntityAdd", $"OnEntityAdd DebugCheckRemoveDictionaryEntry {entity.EntityId}");
					CubeProcessing.PbPrograms.Remove(entity.EntityId);
				}
			}
			catch (Exception e)
			{
				Core.GeneralLog.WriteToLog($"{ModuleName}: OnEntityAdd", $"OnEntityAdd interrupted - exception:\t{e}");
			}
		}

		public static void Register()
		{
			if (_registered) return;
			MyAPIGateway.Entities.OnEntityAdd += OnEntityAdd;
			_registered = true;
			Core.GeneralLog.WriteToLog($"{ModuleName}", "Online...");
		}

		public static void Close()
		{
			MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdd;
			Core.GeneralLog.WriteToLog($"{ModuleName}", "Offline...");
		}
	}
}
