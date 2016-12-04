namespace MovementMod
{
    public class MovementConfig
    {
        public bool EnableDiagonalMovementSpeedFix { get; set; } = true;

        public bool EnableWalkingSpeedOverride { get; set; }
        public int PlayerWalkingSpeed { get; set; } = 2;

        public bool EnableRunningSpeedOverride { get; set; }
        public int PlayerRunningSpeed { get; set; } = 5;

        public bool EnableHorseSpeedOverride { get; set; }
        public int HorseSpeed { get; set; } = 5;

        public bool EnableSprinting { get; set; }
        public int PlayerSprintingSpeedMultiplier { get; set; } = 2;
        public string SprintKey { get; set; } = "LeftShift";
        public bool SprintingDrainsStamina { get; set; } = true;
        public float SprintingStaminaDrainPerSecond { get; set; } = 15;
    }
}
