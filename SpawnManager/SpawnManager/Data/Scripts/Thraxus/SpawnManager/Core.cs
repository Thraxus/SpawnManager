using Sandbox.ModAPI;
using SpawnManager.Utilities;
using VRage.Game.Components;

namespace SpawnManager.SpawnManager
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
		private static bool _registered;

		// Properties
		public static bool IsServer => MyAPIGateway.Multiplayer.IsServer;

		public static Log GeneralLog { get; private set; }

		public static Log ProfilerLog { get; private set; }

		private static void Register()
		{
			if (!IsServer || _registered) return;
			GeneralLog = new Log(GeneralLogName);
			ProfilerLog = new Log(ProfilerLogName);
			GameSettings.Register();
			Definitions.Register();
			Drones.Register();
			_registered = true;
		}

		private static void Close()
		{
			if (!IsServer) return;
			CubeProcessing.Close();
			Drones.Close();
			Definitions.Close();
			ProfilerLog.Close();
			GeneralLog.Close();
		}

		/// <inheritdoc />
		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!IsServer || _registered) return;
			Register();
		}


		protected override void UnloadData()
		{
			Close();
		}
	}
}
