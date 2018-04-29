namespace RegenMod.Framework
{
    internal class ModConfig
    {
        /// <summary>Whether to constantly regenerate stamina.</summary>
        public bool RegenStaminaConstant { get; set; }

        /// <summary>The amount of stamina to constantly regenerate per second.</summary>
        public float RegenStaminaConstantAmountPerSecond { get; set; }

        /// <summary>Whether to regenerate stamina while standing still.</summary>
        public bool RegenStaminaStill { get; set; }

        /// <summary>The amount of stamina to regenerate per second while standing still.</summary>
        public float RegenStaminaStillAmountPerSecond { get; set; }

        /// <summary>The amount of time the player must stand still to regenerate stamina.</summary>
        public int RegenStaminaStillTimeRequiredMS { get; set; } = 1000;

        /// <summary>Whether to constantly regenerate health.</summary>
        public bool RegenHealthConstant { get; set; }

        /// <summary>The amount of stamina to constantly regenerate per second.</summary>
        public float RegenHealthConstantAmountPerSecond { get; set; }

        /// <summary>Whether to regenerate health while standing still.</summary>
        public bool RegenHealthStill { get; set; }

        /// <summary>The amount of health to regenerate per second while standing still.</summary>
        public float RegenHealthStillAmountPerSecond { get; set; }

        /// <summary>The amount of time the player must stand still to regenerate health.</summary>
        public int RegenHealthStillTimeRequiredMS { get; set; } = 1000;
    }
}
