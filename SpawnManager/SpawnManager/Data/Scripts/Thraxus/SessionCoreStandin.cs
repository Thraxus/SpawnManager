using VRage.Game.Components;
// ReSharper disable once ClassNeverInstantiated.Global
namespace SpawnManager
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
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
			_initialized = true;
		}

		protected override void UnloadData()
		{

		}
	}
}