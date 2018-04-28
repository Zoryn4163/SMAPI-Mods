using System.Reflection;
using Microsoft.Xna.Framework;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace FishingMod
{
    internal class SBobberBar : BobberBar
    {
        /// <summary>
        ///     DO NOT CONSTRUCT THIS CLASS
        ///     To retrieve an instance of SBobberBar, use SBobberBar.ConstructFromBaseClass()
        /// </summary>
        public SBobberBar(int whichFish, float fishSize, bool treasure, int bobber) : base(whichFish, fishSize, treasure, bobber)
        {
        }

        public BobberBar BaseBobberBar { get; private set; }

        /// <summary>
        ///     The green rectangle bar that moves up and down
        /// </summary>
        public float bobberPosition
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberPosition").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberPosition").SetValue(this.BaseBobberBar, value); }
        }

        /// <summary>
        ///     The green bar on the right. How close to catching the fish you are
        ///     Range: 0 - 1 | 1 = catch, 0 = fail
        /// </summary>
        public float distanceFromCatching
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("distanceFromCatching").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("distanceFromCatching").SetValue(this.BaseBobberBar, value); }
        }

        public float difficulty
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("difficulty").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("difficulty").SetValue(this.BaseBobberBar, value); }
        }

        public int motionType
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("motionType").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("motionType").SetValue(this.BaseBobberBar, value); }
        }

        public int whichFish
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("whichFish").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("whichFish").SetValue(this.BaseBobberBar, value); }
        }

        public float bobberSpeed
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberSpeed").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberSpeed").SetValue(this.BaseBobberBar, value); }
        }

        public float bobberAcceleration
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberAcceleration").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberAcceleration").SetValue(this.BaseBobberBar, value); }
        }

        public float bobberTargetPosition
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberTargetPosition").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberTargetPosition").SetValue(this.BaseBobberBar, value); }
        }

        public float scale
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("scale").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("scale").SetValue(this.BaseBobberBar, value); }
        }

        public float everythingShakeTimer
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("everythingShakeTimer").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("everythingShakeTimer").SetValue(this.BaseBobberBar, value); }
        }

        public float floaterSinkerAcceleration
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("floaterSinkerAcceleration").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("floaterSinkerAcceleration").SetValue(this.BaseBobberBar, value); }
        }

        public float treasurePosition
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("treasurePosition").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasurePosition").SetValue(this.BaseBobberBar, value); }
        }

        public float treasureCatchLevel
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("treasureCatchLevel").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasureCatchLevel").SetValue(this.BaseBobberBar, value); }
        }

        public float treasureAppearTimer
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("treasureAppearTimer").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasureAppearTimer").SetValue(this.BaseBobberBar, value); }
        }

        public float treasureScale
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("treasureScale").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasureScale").SetValue(this.BaseBobberBar, value); }
        }

        public bool bobberInBar
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("bobberInBar").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberInBar").SetValue(this.BaseBobberBar, value); }
        }

        public bool buttonPressed
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("buttonPressed").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("buttonPressed").SetValue(this.BaseBobberBar, value); }
        }

        public bool flipBubble
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("flipBubble").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("flipBubble").SetValue(this.BaseBobberBar, value); }
        }

        public bool fadeIn
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("fadeIn").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("fadeIn").SetValue(this.BaseBobberBar, value); }
        }

        public bool fadeOut
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("fadeOut").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberPfadeOutosition").SetValue(this.BaseBobberBar, value); }
        }

        /// <summary>
        ///     Whether or not a treasure chest appears
        /// </summary>
        public bool treasure
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("treasure").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasure").SetValue(this.BaseBobberBar, value); }
        }

        public bool treasureCaught
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("treasureCaught").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasureCaught").SetValue(this.BaseBobberBar, value); }
        }

        public bool perfect
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("perfect").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("perfect").SetValue(this.BaseBobberBar, value); }
        }

        public bool bossFish
        {
            get { return (bool)SBobberBar.GetBaseFieldInfo("bossFish").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bossFish").SetValue(this.BaseBobberBar, value); }
        }

        public int bobberBarHeight
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("bobberBarHeight").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberBarHeight").SetValue(this.BaseBobberBar, value); }
        }

        public int fishSize
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("fishSize").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("fishSize").SetValue(this.BaseBobberBar, value); }
        }

        public int fishQuality
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("fishQuality").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("fishQuality").SetValue(this.BaseBobberBar, value); }
        }

        public int minFishSize
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("minFishSize").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("minFishSize").SetValue(this.BaseBobberBar, value); }
        }

        public int maxFishSize
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("maxFishSize").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("maxFishSize").SetValue(this.BaseBobberBar, value); }
        }

        public int fishSizeReductionTimer
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("fishSizeReductionTimer").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("fishSizeReductionTimer").SetValue(this.BaseBobberBar, value); }
        }

        public int whichBobber
        {
            get { return (int)SBobberBar.GetBaseFieldInfo("whichBobber").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("whichBobber").SetValue(this.BaseBobberBar, value); }
        }

        public Vector2 barShake
        {
            get { return (Vector2)SBobberBar.GetBaseFieldInfo("barShake").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("barShake").SetValue(this.BaseBobberBar, value); }
        }

        public Vector2 fishShake
        {
            get { return (Vector2)SBobberBar.GetBaseFieldInfo("fishShake").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("fishShake").SetValue(this.BaseBobberBar, value); }
        }

        public Vector2 everythingShake
        {
            get { return (Vector2)SBobberBar.GetBaseFieldInfo("everythingShake").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("everythingShake").SetValue(this.BaseBobberBar, value); }
        }

        public Vector2 treasureShake
        {
            get { return (Vector2)SBobberBar.GetBaseFieldInfo("treasureShake").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("treasureShake").SetValue(this.BaseBobberBar, value); }
        }

        public float reelRotation
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("reelRotation").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("reelRotation").SetValue(this.BaseBobberBar, value); }
        }

        public SparklingText sparkleText
        {
            get { return (SparklingText)SBobberBar.GetBaseFieldInfo("sparkleText").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("sparkleText").SetValue(this.BaseBobberBar, value); }
        }

        public float bobberBarPos
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberBarPos").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberBarPos").SetValue(this.BaseBobberBar, value); }
        }

        public float bobberBarSpeed
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberBarSpeed").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberBarSpeed").SetValue(this.BaseBobberBar, value); }
        }

        public float bobberBarAcceleration
        {
            get { return (float)SBobberBar.GetBaseFieldInfo("bobberBarAcceleration").GetValue(this.BaseBobberBar); }
            set { SBobberBar.GetBaseFieldInfo("bobberBarAcceleration").SetValue(this.BaseBobberBar, value); }
        }

        public static FieldInfo[] PrivateFields => SBobberBar.GetPrivateFields();

        public static SBobberBar ConstructFromBaseClass(BobberBar baseClass)
        {
            var b = new SBobberBar(0, 0, false, 0) { BaseBobberBar = baseClass };
            return b;
        }

        public static FieldInfo[] GetPrivateFields()
        {
            return typeof(BobberBar).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static FieldInfo GetBaseFieldInfo(string name)
        {
            return typeof(BobberBar).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
