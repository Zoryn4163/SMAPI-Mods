using StardewModdingAPI;
using StardewValley.Menus;

namespace FishingMod.Framework
{
    /// <summary>Handles access to a bobber bar's private fields.</summary>
    internal class SBobberBar
    {
        /*********
        ** Fields
        *********/
        /// <summary>The underlying field for <see cref="DistanceFromCatching"/>.</summary>
        private readonly IReflectedField<float> DistanceFromCatchingField;

        /// <summary>The underlying field for <see cref="Difficulty"/>.</summary>
        private readonly IReflectedField<float> DifficultyField;

        /// <summary>The underlying field for <see cref="MotionType"/>.</summary>
        private readonly IReflectedField<int> MotionTypeField;

        /// <summary>The underlying field for <see cref="BobberInBar"/>.</summary>
        private readonly IReflectedField<bool> BobberInBarField;

        /// <summary>The underlying field for <see cref="Treasure"/>.</summary>
        private readonly IReflectedField<bool> TreasureField;

        /// <summary>The underlying field for <see cref="TreasureCaught"/>.</summary>
        private readonly IReflectedField<bool> TreasureCaughtField;

        /// <summary>The underlying field for <see cref="Perfect"/>.</summary>
        private readonly IReflectedField<bool> PerfectField;


        /*********
        ** Accessors
        *********/
        /// <summary>The underlying bobber bar.</summary>
        public BobberBar Instance { get; set; }

        /// <summary>
        ///     The green bar on the right. How close to catching the fish you are
        ///     Range: 0 - 1 | 1 = catch, 0 = fail
        /// </summary>
        public float DistanceFromCatching
        {
            get => this.DistanceFromCatchingField.GetValue();
            set => this.DistanceFromCatchingField.SetValue(value);
        }

        public float Difficulty
        {
            get => this.DifficultyField.GetValue();
            set => this.DifficultyField.SetValue(value);
        }

        public int MotionType
        {
            get => this.MotionTypeField.GetValue();
            set => this.MotionTypeField.SetValue(value);
        }

        public bool BobberInBar
        {
            get => this.BobberInBarField.GetValue();
        }

        /// <summary>
        ///     Whether or not a treasure chest appears
        /// </summary>
        public bool Treasure
        {
            get => this.TreasureField.GetValue();
            set => this.TreasureField.SetValue(value);
        }

        public bool TreasureCaught
        {
            get => this.TreasureCaughtField.GetValue();
            set => this.TreasureCaughtField.SetValue(value);
        }

        public bool Perfect
        {
            get => this.PerfectField.GetValue();
            set => this.PerfectField.SetValue(value);
        }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="instance">The underlying bobber bar.</param>
        /// <param name="reflection">Simplifies access to private code.</param>
        public SBobberBar(BobberBar instance, IReflectionHelper reflection)
        {
            this.Instance = instance;

            this.DistanceFromCatchingField = reflection.GetField<float>(instance, "distanceFromCatching");
            this.DifficultyField = reflection.GetField<float>(instance, "difficulty");
            this.MotionTypeField = reflection.GetField<int>(instance, "motionType");
            this.BobberInBarField = reflection.GetField<bool>(instance, "bobberInBar");
            this.TreasureField = reflection.GetField<bool>(instance, "treasure");
            this.TreasureCaughtField = reflection.GetField<bool>(instance, "treasureCaught");
            this.PerfectField = reflection.GetField<bool>(instance, "perfect");
        }
    }
}
