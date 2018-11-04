using Sandbox.ModAPI;
using SpawnManager.Networking;
using SpawnManager.Support;
using SpawnManager.Utilities;
using VRage.Game.Components;

namespace SpawnManager
{
	// ReSharper disable once ClassNeverInstantiated.Global
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class Core : MySessionComponentBase
	{
		// Constants
		private const string GeneralLogName = "SpawnManager";
		private const string ProfilerLogName = "SpawnManagerProfiler";
		public const ushort NetworkId = 11487;

		// Fields
		private static bool _registerEarly;
		private static bool _registerLate;

		// Properties
		public static bool IsServer => MyAPIGateway.Multiplayer.IsServer;

		public static Log GeneralLog { get; private set; }

		public static Log ProfilerLog { get; private set; }

		private void RegisterEarly()
		{
			if (!IsServer || _registerEarly) return;
			GeneralLog = new Log(GeneralLogName);
			ProfilerLog = new Log(ProfilerLogName);
			Messaging.Register();
			GameSettings.Register();
			MyAPIGateway.Utilities.InvokeOnGameThread(() => SetUpdateOrder(MyUpdateOrder.BeforeSimulation));
			GeneralLog.WriteToLog("Core", $"RegisterEarly Complete... {UpdateOrder}");
			_registerEarly = true;
		}

		private void RegisterLate()
		{
			if (!IsServer || _registerLate) return;
			Definitions.Register();
			Drones.Drones.Register();
			MyAPIGateway.Utilities.InvokeOnGameThread(() => SetUpdateOrder(MyUpdateOrder.NoUpdate));
			GeneralLog.WriteToLog("Core", $"RegisterLate Complete... {UpdateOrder}");
			_registerLate = true;
		}

		private static void Close()
		{
			if (!IsServer) return;
			GeneralLog.WriteToLog("Core", "Unloading...");
			CubeProcessing.Close();
			Drones.Drones.Close();
			Definitions.Close();
			Messaging.Close();
			ProfilerLog.Close();
			GeneralLog.Close();
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!IsServer || _registerEarly) return;
			RegisterEarly();
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!IsServer || _registerLate) return;
			RegisterLate();
		}

		protected override void UnloadData()
		{
			Close();
		}
	}
}
