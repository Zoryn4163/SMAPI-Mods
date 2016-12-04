using StardewModdingAPI;

namespace RegenMod
{
    public class RegenConfig : Config
    {
        public bool RegenStaminaConstant { get; set; }
        public bool RegenStaminaConstantIsNegative { get; set; }
        public float RegenStaminaConstantAmountPerSecond { get; set; }

        public bool RegenStaminaStill { get; set; }
        public bool RegenStaminaStillIsNegative { get; set; }
        public float RegenStaminaStillAmountPerSecond { get; set; }
        public int RegenStaminaStillTimeRequiredMS { get; set; }



        public bool RegenHealthConstant { get; set; }
        public bool RegenHealthConstantIsNegative { get; set; }
        public float RegenHealthConstantAmountPerSecond { get; set; }

        public bool RegenHealthStill { get; set; }
        public bool RegenHealthStillIsNegative { get; set; }
        public float RegenHealthStillAmountPerSecond { get; set; }
        public int RegenHealthStillTimeRequiredMS { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            RegenStaminaConstant = false;
            RegenStaminaConstantIsNegative = false;
            RegenStaminaConstantAmountPerSecond = 0;

            RegenStaminaStill = false;
            RegenStaminaStillIsNegative = false;
            RegenStaminaStillAmountPerSecond = 0;
            RegenStaminaStillTimeRequiredMS = 1000;



            RegenHealthConstant = false;
            RegenHealthConstantIsNegative = false;
            RegenHealthConstantAmountPerSecond = 0;

            RegenHealthStill = false;
            RegenHealthStillIsNegative = false;
            RegenHealthStillAmountPerSecond = 0;
            RegenHealthStillTimeRequiredMS = 1000;

            return this as T;
        }
    }
}