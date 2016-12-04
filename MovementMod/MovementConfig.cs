using StardewModdingAPI;

namespace MovementMod
{
    public class MovementConfig : Config
    {
        public bool EnableDiagonalMovementSpeedFix { get; set; }

        public bool EnableWalkingSpeedOverride { get; set; }
        public int PlayerWalkingSpeed { get; set; }

        public bool EnableRunningSpeedOverride { get; set; }
        public int PlayerRunningSpeed { get; set; }

        public bool EnableHorseSpeedOverride { get; set; }
        public int HorseSpeed { get; set; }

        public bool EnableSprinting { get; set; }
        public int PlayerSprintingSpeedMultiplier { get; set; }
        public string SprintKey { get; set; }
        public bool SprintingDrainsStamina { get; set; }
        public float SprintingStaminaDrainPerSecond { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            EnableDiagonalMovementSpeedFix = true;

            EnableWalkingSpeedOverride = false;
            PlayerWalkingSpeed = 2;

            EnableRunningSpeedOverride = false;
            PlayerRunningSpeed = 5;

            EnableHorseSpeedOverride = false;
            HorseSpeed = 5;

            EnableSprinting = false;
            PlayerSprintingSpeedMultiplier = 2;
            SprintKey = "LeftShift";
            SprintingDrainsStamina = true;
            SprintingStaminaDrainPerSecond = 15;

            return this as T;
        }
    }
}