using System;

namespace SpawnManager.SpawnManager
{
    public struct CustomPrefabConfiguration
    {
        public enum PrefabType
        {
            Fighter, Freighter, Station, None
        }

        [Flags]
        public enum PrefabPreset
        {
            Police, Military, Grinder, Salvage, LongRange, ShortRange, LargeGrid, SmallGrid, None
        }

        [Flags]
        public enum PrefabSafeSpawnLocations
        {
            Atmosphere, Lunar, Space
        }

        public string PrefabName;
        public string DisplayName;
        public string FactionTag;
        public PrefabType Type;
        public bool CallHelpOnDamage;
        public int ActivationDistance;
        public bool AmbushMode;
        public int CallHelpProbability;
        public bool DelayedAi;
        public bool FleeOnlyWhenDamaged;
        public int FleeSpeedCap;
        public int FleeTriggerDistance;
        public int PlayerPriority;
        public PrefabPreset Preset;
        public PrefabSafeSpawnLocations SafeSpawnLocations;
        public int SeekDistance;

        public CustomPrefabConfiguration(string prefabName, string displayName, PrefabType type, string factionTag = "SPRT", int activationDistance = 0, int callHelpProbability = 0, int fleeSpeedCap = 0, int fleeTriggerDistance = 0, int playerPriority = 0,
            int seekDistance = 0, bool ambushMode = false, bool callHelpOnDamage = false, bool delayedAi = false, bool fleeOnlyWhenDamaged = false, PrefabPreset preset = PrefabPreset.None, 
            PrefabSafeSpawnLocations safeSpawnLocations = PrefabSafeSpawnLocations.Space)
        {
            DisplayName = displayName;
            PrefabName = prefabName;
            Type = type;
            FactionTag = factionTag;
            ActivationDistance = activationDistance;
            CallHelpProbability = callHelpProbability;
            FleeSpeedCap = fleeSpeedCap;
            FleeTriggerDistance = fleeTriggerDistance;
            PlayerPriority = playerPriority;
            SeekDistance = seekDistance;
            AmbushMode = ambushMode;
            CallHelpOnDamage = callHelpOnDamage;
            DelayedAi = delayedAi;
            FleeOnlyWhenDamaged = fleeOnlyWhenDamaged;
            Preset = preset;
            SafeSpawnLocations = safeSpawnLocations;
        }
    }
}