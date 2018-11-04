using VRage.Game.Components;

namespace SpawnManager
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	// ReSharper disable once ClassNeverInstantiated.Global
	public class SessionCoreStandin: MySessionComponentBase
	{
		private bool _initialized;

		/// <summary>
		/// Runs every tick before the simulation is updated
		/// </summary>
		public override void UpdateBeforeSimulation()
		{
			if (!_initialized) Initialize();
		}

		private void Initialize()
		{
			//Core.Initialize();
			_initialized = true;
		}

		protected override void UnloadData()
		{
			//Core.Close();
		}
	}
}