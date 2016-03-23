using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewModdingAPI.Inheritance.Menus;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace FishingMod
{
    public class FishingMod : Mod
    {
        public static Farmer Player => Game1.player;
        public static IClickableMenu ActiveMenu => Game1.activeClickableMenu;
        public static BobberBar BaseBobber => ActiveMenu as BobberBar;

        public static SBobberBar Bobber { get; protected set; }

        public static bool BeganFishingGame { get; protected set; }
        public static int UpdateIndex { get; protected set; }

        public static FishConfig ModConfig { get; protected set; }

        public override void Entry(params object[] objects)
        {
            ModConfig = new FishConfig().InitializeConfig(BaseConfigPath);

            GameEvents.UpdateTick += GameEventsOnUpdateTick;
            GameEvents.OneSecondTick += GameEvents_OneSecondTick;
            MenuEvents.MenuChanged += MenuEvents_MenuChanged;

            Log.Info("FishingMod by Zoryn => Initialized");
        }

        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {
            if (e.NewMenu is BobberBar)
            {
                Bobber = SBobberBar.ConstructFromBaseClass((BobberBar)e.NewMenu);
            }
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (ModConfig.InfiniteBait || ModConfig.InfiniteTackle)
            {
                if (Player?.CurrentTool is FishingRod && Player?.CurrentTool?.attachments?.Length > 0 && Player.CurrentTool.attachments[0] != null)
                {
                    if (ModConfig.InfiniteBait)
                        Player.CurrentTool.attachments[0].Stack = Player.CurrentTool.attachments[0].maximumStackSize();

                    if (Player.CurrentTool?.attachments?.Length > 1 && Player.CurrentTool.attachments[1] != null)
                    {
                        if (ModConfig.InfiniteTackle)
                        {
                            Player.CurrentTool.attachments[1].Stack = Player.CurrentTool.attachments[1].maximumStackSize();
                            Player.CurrentTool.attachments[1].scale = new Vector2(Player.CurrentTool.attachments[1].scale.X, 1.1f);
                        }
                    }
                }
            }
        }

        private void GameEventsOnUpdateTick(object sender, EventArgs e)
        {
            if (ActiveMenu is BobberBar && Bobber != null)
            {
                //Begin fishing game
                if (!BeganFishingGame && UpdateIndex > 15)
                {
                    //Do these things once per fishing minigame, 1/4 second after it updates
                    Bobber.difficulty *= ModConfig.FishDifficultyMultiplier;
                    Bobber.difficulty += ModConfig.FishDifficultyAdditive;

                    if (ModConfig.AlwaysFindTreasure)
                        Bobber.treasure = true;

                    if (ModConfig.InstantCatchFish)
                        Bobber.distanceFromCatching += 100;

                    if (ModConfig.InstantCatchTreasure)
                        if (Bobber.treasure || ModConfig.AlwaysFindTreasure)
                            Bobber.treasureCaught = true;

                    if (ModConfig.EasierFishing)
                    {
                        Bobber.difficulty = Math.Max(15, Math.Max(Bobber.difficulty, 60));
                        Bobber.motionType = 2;
                    }

                    BeganFishingGame = true;
                }

                if (UpdateIndex < 20)
                    UpdateIndex++;

                if (ModConfig.AlwaysPerfect)
                    Bobber.perfect = true;

                if (!Bobber.bobberInBar)
                    Bobber.distanceFromCatching += ModConfig.LossAdditive;
            }
            else
            {
                //End fishing game
                BeganFishingGame = false;
                UpdateIndex = 0;
            }
        }
    }

    public class FishConfig : Config
    {
        public bool AlwaysPerfect { get; set; }
        public bool AlwaysFindTreasure { get; set; }
        public bool InstantCatchFish { get; set; }
        public bool InstantCatchTreasure { get; set; }
        public bool EasierFishing { get; set; }
        public float FishDifficultyMultiplier { get; set; }
        public float FishDifficultyAdditive { get; set; }
        public float LossAdditive { get; set; }

        public bool InfiniteTackle { get; set; }
        public bool InfiniteBait { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            AlwaysPerfect = false;
            AlwaysFindTreasure = false;
            InstantCatchFish = false;
            InstantCatchTreasure = false;
            EasierFishing = false;
            FishDifficultyMultiplier = 1;
            FishDifficultyAdditive = 0;
            LossAdditive = 2 / 1000f;

            InfiniteTackle = true;
            InfiniteBait = true;
            return this as T;
        }
    }
}
