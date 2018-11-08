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

		public bool DisableDampners = true;
		public bool DisableIdleTurretMovement = false;
		public bool ForcePhysics = false;
		public bool ForceStatic = false;
		public bool IgnoreCleanup = false;
		public bool IgnoreCollisions = false;
		public bool PowerDownGrid = false;
		public bool PreservePrograms = true;
		public bool RenameBeacon = false;
		public bool Restock = false;
		public bool SetAngularVelocity = false;
		public bool SetFactionShareMode = true;
		public bool SetLinearVelocity = false;
		public bool SpawnAsWireframe = false;
		public bool SpawnDamaged = false;

		public double SpawmDamagedPercent = 0.5;

		public float BeaconBroadcastRadius = 30000;

		public long EntityId = 0;
		public long OwnerId = 0;
		public long BuiltBy = 0;

		public SerializableVector3 LinearVelocity = new SerializableVector3();
		public SerializableVector3 AngularVelocity = new SerializableVector3();

		public string BeaconText = "";
		
		public MyOwnershipShareModeEnum FactionShareMode = MyOwnershipShareModeEnum.Faction;
		public Precision CollisionPrecision = Precision.ExtraLoose;
	    public EnvironmentType Environment = EnvironmentType.Space;
	    
	}
}