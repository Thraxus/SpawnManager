using VRage;
using VRage.Game;

namespace SpawnManager.Support
{
	public class Options
	{
	    public enum EnvironmentType
	    {
	        Space, Atmosphere, Lunar
	    }

	    public enum Precision
	    {
	        Precise, Loose, ExtraLoose
	    }

		public bool ClearCargoContainers = false;
		public bool DestructibleBlocks = true;
		public bool DisableDampeners = true;
		public bool ForcePhysics = false;
		public bool ForceStatic = false;
		public bool IdleTurretMovement = true;
		public bool IgnoreCleanup = false;
		public bool IgnoreCollisions = false;
		public bool PowerDownGrid = false;
		public bool PreservePrograms = true;
		public bool Restock = false;
		public bool SetAngularVelocity = false;
		public bool SetFactionShareMode = true;
		public bool SetLinearVelocity = false;
		public bool SpawnAsWireframe = false;
		public bool SpawnDamaged = false;
		public bool UpdateAntennaText = false;
		public bool UpdateBeaconText = false;
		public bool UpdateBeaconBroadcastRadius = false;

		public double SpawmDamagedPercent = 0.5;

		public float BeaconBroadcastRadius = 30000;
		public float GeneralDamageModifier = 1;
		
		public int MaxAmmo = 100;
		public int MaxUranium = 100;

		public long EntityId = 0;
		public long OwnerId = 0;
		public long BuiltBy = 0;

		public SerializableVector3 LinearVelocity = new SerializableVector3();
		public SerializableVector3 AngularVelocity = new SerializableVector3();

		public string AntennaText = "";
		public string BeaconText = "";

		public MyOwnershipShareModeEnum FactionShareMode = MyOwnershipShareModeEnum.Faction;
		public Precision CollisionPrecision = Precision.ExtraLoose;
	    public EnvironmentType Environment = EnvironmentType.Space;

	    /// <inheritdoc />
	    public override string ToString()
	    {
		    return
			    $"ClearCargoContainers: {ClearCargoContainers} | DestructibleBlocks: {DestructibleBlocks} | DisableDampeners: {DisableDampeners} | ForcePhysics: {ForcePhysics} | ForceStatic: {ForceStatic} " +
			    $"| IdleTurretMovement: {IdleTurretMovement} | IgnoreCleanup: {IgnoreCleanup} | IgnoreCollisions: {IgnoreCollisions} | PowerDownGrid: {PowerDownGrid} | PreservePrograms: {PreservePrograms} " + 
			    $"| Restock: {Restock} | SetAngularVelocity: {SetAngularVelocity} | SetFactionShareMode: {SetFactionShareMode} | SetLinearVelocity: {SetLinearVelocity} | SpawnAsWireframe: {SpawnAsWireframe} " + 
			    $"| SpawnDamaged: {SpawnDamaged} | UpdateAntennaText: {UpdateAntennaText} | UpdateBeaconText: {UpdateBeaconText} | UpdateBeaconBroadcastRadius: {UpdateBeaconBroadcastRadius}";
	    }
	}
}