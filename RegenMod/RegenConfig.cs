namespace RegenMod
{
    internal class RegenConfig
    {
        public bool RegenStaminaConstant { get; set; }
        public bool RegenStaminaConstantIsNegative { get; set; }
        public float RegenStaminaConstantAmountPerSecond { get; set; }

        public bool RegenStaminaStill { get; set; }
        public bool RegenStaminaStillIsNegative { get; set; }
        public float RegenStaminaStillAmountPerSecond { get; set; }
        public int RegenStaminaStillTimeRequiredMS { get; set; } = 1000;

        public bool RegenHealthConstant { get; set; }
        public bool RegenHealthConstantIsNegative { get; set; }
        public float RegenHealthConstantAmountPerSecond { get; set; }

        public bool RegenHealthStill { get; set; }
        public bool RegenHealthStillIsNegative { get; set; }
        public float RegenHealthStillAmountPerSecond { get; set; }
        public int RegenHealthStillTimeRequiredMS { get; set; } = 1000;
    }
}
