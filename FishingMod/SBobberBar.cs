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
            get { return (float)GetBaseFieldInfo("bobberPosition").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberPosition").SetValue(BaseBobberBar, value); }
        }

        /// <summary>
        ///     The green bar on the right. How close to catching the fish you are
        ///     Range: 0 - 1 | 1 = catch, 0 = fail
        /// </summary>
        public float distanceFromCatching
        {
            get { return (float)GetBaseFieldInfo("distanceFromCatching").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("distanceFromCatching").SetValue(BaseBobberBar, value); }
        }

        public float difficulty
        {
            get { return (float)GetBaseFieldInfo("difficulty").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("difficulty").SetValue(BaseBobberBar, value); }
        }

        public int motionType
        {
            get { return (int)GetBaseFieldInfo("motionType").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("motionType").SetValue(BaseBobberBar, value); }
        }

        public int whichFish
        {
            get { return (int)GetBaseFieldInfo("whichFish").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("whichFish").SetValue(BaseBobberBar, value); }
        }

        public float bobberSpeed
        {
            get { return (float)GetBaseFieldInfo("bobberSpeed").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberSpeed").SetValue(BaseBobberBar, value); }
        }

        public float bobberAcceleration
        {
            get { return (float)GetBaseFieldInfo("bobberAcceleration").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberAcceleration").SetValue(BaseBobberBar, value); }
        }

        public float bobberTargetPosition
        {
            get { return (float)GetBaseFieldInfo("bobberTargetPosition").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberTargetPosition").SetValue(BaseBobberBar, value); }
        }

        public float scale
        {
            get { return (float)GetBaseFieldInfo("scale").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("scale").SetValue(BaseBobberBar, value); }
        }

        public float everythingShakeTimer
        {
            get { return (float)GetBaseFieldInfo("everythingShakeTimer").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("everythingShakeTimer").SetValue(BaseBobberBar, value); }
        }

        public float floaterSinkerAcceleration
        {
            get { return (float)GetBaseFieldInfo("floaterSinkerAcceleration").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("floaterSinkerAcceleration").SetValue(BaseBobberBar, value); }
        }

        public float treasurePosition
        {
            get { return (float)GetBaseFieldInfo("treasurePosition").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasurePosition").SetValue(BaseBobberBar, value); }
        }

        public float treasureCatchLevel
        {
            get { return (float)GetBaseFieldInfo("treasureCatchLevel").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasureCatchLevel").SetValue(BaseBobberBar, value); }
        }

        public float treasureAppearTimer
        {
            get { return (float)GetBaseFieldInfo("treasureAppearTimer").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasureAppearTimer").SetValue(BaseBobberBar, value); }
        }

        public float treasureScale
        {
            get { return (float)GetBaseFieldInfo("treasureScale").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasureScale").SetValue(BaseBobberBar, value); }
        }

        public bool bobberInBar
        {
            get { return (bool)GetBaseFieldInfo("bobberInBar").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberInBar").SetValue(BaseBobberBar, value); }
        }

        public bool buttonPressed
        {
            get { return (bool)GetBaseFieldInfo("buttonPressed").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("buttonPressed").SetValue(BaseBobberBar, value); }
        }

        public bool flipBubble
        {
            get { return (bool)GetBaseFieldInfo("flipBubble").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("flipBubble").SetValue(BaseBobberBar, value); }
        }

        public bool fadeIn
        {
            get { return (bool)GetBaseFieldInfo("fadeIn").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("fadeIn").SetValue(BaseBobberBar, value); }
        }

        public bool fadeOut
        {
            get { return (bool)GetBaseFieldInfo("fadeOut").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberPfadeOutosition").SetValue(BaseBobberBar, value); }
        }

        /// <summary>
        ///     Whether or not a treasure chest appears
        /// </summary>
        public bool treasure
        {
            get { return (bool)GetBaseFieldInfo("treasure").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasure").SetValue(BaseBobberBar, value); }
        }

        public bool treasureCaught
        {
            get { return (bool)GetBaseFieldInfo("treasureCaught").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasureCaught").SetValue(BaseBobberBar, value); }
        }

        public bool perfect
        {
            get { return (bool)GetBaseFieldInfo("perfect").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("perfect").SetValue(BaseBobberBar, value); }
        }

        public bool bossFish
        {
            get { return (bool)GetBaseFieldInfo("bossFish").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bossFish").SetValue(BaseBobberBar, value); }
        }

        public int bobberBarHeight
        {
            get { return (int)GetBaseFieldInfo("bobberBarHeight").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberBarHeight").SetValue(BaseBobberBar, value); }
        }

        public int fishSize
        {
            get { return (int)GetBaseFieldInfo("fishSize").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("fishSize").SetValue(BaseBobberBar, value); }
        }

        public int fishQuality
        {
            get { return (int)GetBaseFieldInfo("fishQuality").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("fishQuality").SetValue(BaseBobberBar, value); }
        }

        public int minFishSize
        {
            get { return (int)GetBaseFieldInfo("minFishSize").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("minFishSize").SetValue(BaseBobberBar, value); }
        }

        public int maxFishSize
        {
            get { return (int)GetBaseFieldInfo("maxFishSize").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("maxFishSize").SetValue(BaseBobberBar, value); }
        }

        public int fishSizeReductionTimer
        {
            get { return (int)GetBaseFieldInfo("fishSizeReductionTimer").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("fishSizeReductionTimer").SetValue(BaseBobberBar, value); }
        }

        public int whichBobber
        {
            get { return (int)GetBaseFieldInfo("whichBobber").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("whichBobber").SetValue(BaseBobberBar, value); }
        }

        public Vector2 barShake
        {
            get { return (Vector2)GetBaseFieldInfo("barShake").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("barShake").SetValue(BaseBobberBar, value); }
        }

        public Vector2 fishShake
        {
            get { return (Vector2)GetBaseFieldInfo("fishShake").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("fishShake").SetValue(BaseBobberBar, value); }
        }

        public Vector2 everythingShake
        {
            get { return (Vector2)GetBaseFieldInfo("everythingShake").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("everythingShake").SetValue(BaseBobberBar, value); }
        }

        public Vector2 treasureShake
        {
            get { return (Vector2)GetBaseFieldInfo("treasureShake").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("treasureShake").SetValue(BaseBobberBar, value); }
        }

        public float reelRotation
        {
            get { return (float)GetBaseFieldInfo("reelRotation").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("reelRotation").SetValue(BaseBobberBar, value); }
        }

        public SparklingText sparkleText
        {
            get { return (SparklingText)GetBaseFieldInfo("sparkleText").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("sparkleText").SetValue(BaseBobberBar, value); }
        }

        public float bobberBarPos
        {
            get { return (float)GetBaseFieldInfo("bobberBarPos").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberBarPos").SetValue(BaseBobberBar, value); }
        }

        public float bobberBarSpeed
        {
            get { return (float)GetBaseFieldInfo("bobberBarSpeed").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberBarSpeed").SetValue(BaseBobberBar, value); }
        }

        public float bobberBarAcceleration
        {
            get { return (float)GetBaseFieldInfo("bobberBarAcceleration").GetValue(BaseBobberBar); }
            set { GetBaseFieldInfo("bobberBarAcceleration").SetValue(BaseBobberBar, value); }
        }

        public static FieldInfo[] PrivateFields => GetPrivateFields();

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
