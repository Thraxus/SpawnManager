using VRage.Game.Components;
// ReSharper disable once ClassNeverInstantiated.Global
namespace SpawnManager
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
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
			//Core.Register();
			_initialized = true;
		}

		protected override void UnloadData()
		{
			//Core.Close();
			//Core.Close();
		}
	}
}