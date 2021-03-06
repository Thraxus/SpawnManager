﻿using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using SpawnManager.DebugTools;
using VRage.Game.ModAPI;

namespace SpawnManager.Networking
{
	public static class ChatHandler
	{
		internal const string SpawnManagerChatCommandPrefix = "/spawner";
		private const string HelpPrefix = "help";
		private const string ShowProfilingLogPrefix = "showprofilinglog";
		private const string ShowGeneralLogPrefix = "showgenerallog";
		private const string SpawnTestCase = "spawntest";
		private const string SpawnModdedTestCase = "spawnmodded";
		private const string SpawnPrefabPrefix = "spawnprefab";
		private const string SpawnGroupPrefix = "spawngroup";
		private const string SpawnEemTradePrefabs = "spawntrade";

		private static readonly Dictionary<string, Action<string>> ChatAction = new Dictionary<string, Action<string>>
		{
			{ HelpPrefix, PrintHelpCommands },
			{ ShowGeneralLogPrefix, ShowGeneralLog },
			{ ShowProfilingLogPrefix, ShowProfilingLog },
			{ SpawnTestCase, SpawnTestClass.SpawnTestCase },
			{ SpawnPrefabPrefix, SpawnTestClass.SpawnPrefab },
			{ SpawnGroupPrefix, SpawnTestClass.SpawnGroup },
			{ SpawnModdedTestCase, SpawnTestClass.SpawnTestCaseModded },
			{ SpawnEemTradePrefabs, SpawnTestClass.SpawnEemTradePrefabs }
		};

		public static void HandleChatMessage(string message)
		{

			IMyPlayer localPlayer = MyAPIGateway.Session.Player;

			if (localPlayer.PromoteLevel < MyPromoteLevel.Admin)
			{
				Messaging.ShowLocalNotification($"You must be an Administrator to invoke SpawnManager Chat Commands.  Current Rank: {localPlayer.PromoteLevel.ToString()}");
				return;
			}

			string[] chatCommand = message.Split(' ');

			if (chatCommand.Length < 2)
			{
				PrintHelpCommands("");
				return;
			}

			Action<string> action;
			string actionText = null;

			if (chatCommand.Length > 2)
				actionText = message.Replace(SpawnManagerChatCommandPrefix, "").Trim();

			if (ChatAction.TryGetValue(chatCommand[1], out action))
				action?.Invoke(actionText);
			else PrintHelpCommands("");
		}

		/// <summary>
		/// Prints a list of available commands
		/// </summary>  
		private static void PrintHelpCommands(string s)
		{
			Messaging.ShowLocalNotification($"'{SpawnManagerChatCommandPrefix} {HelpPrefix}' will show this message");
			Messaging.ShowLocalNotification($"'{SpawnManagerChatCommandPrefix} {ShowProfilingLogPrefix}' will show the last 20 lines of the Profiling Log");
			Messaging.ShowLocalNotification($"'{SpawnManagerChatCommandPrefix} {ShowGeneralLogPrefix}' will show the last 20 lines of the General Log");
			Messaging.ShowLocalNotification($"'{SpawnManagerChatCommandPrefix} {SpawnPrefabPrefix}' spawns selected prefab (prefab name after prefix)");
			Messaging.ShowLocalNotification($"'{SpawnManagerChatCommandPrefix} {SpawnGroupPrefix}' spawns selected spawn group (spawn group name after prefix)");
			Messaging.ShowLocalNotification($"'{SpawnManagerChatCommandPrefix} {SpawnEemTradePrefabs}' spawns all EEM Trade ships");
		}

		private static void ShowGeneralLog(string s)
		{
			Core.GeneralLog?.GetTailMessages();
		}

		private static void ShowProfilingLog(string s)
		{
			Core.ProfilerLog?.GetTailMessages();
		}
	}
}

