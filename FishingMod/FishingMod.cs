using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
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
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
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
                    {
                        if (Bobber.treasure)
                            Bobber.treasureCaught = true;
                        Bobber.distanceFromCatching += 100;
                    }

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

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = ModConfig.ReloadConfig();
                Log.Success("Config Reloaded for " + GetType().Name);
            }
        }
    }
}
