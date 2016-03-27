using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace MovementMod
{
    public class MovementMod : Mod
    {
        public static MovementConfig ModConfig { get; private set; }

        public static SGame TheGame => Program.gamePtr;
        public static Farmer Player => Game1.player;

        public override void Entry(params object[] objects)
        {
            ModConfig = new MovementConfig().InitializeConfig(BaseConfigPath);

            GameEvents.UpdateTick += GameEventsOnUpdateTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            Log.AsyncY(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
        }

        private void GameEventsOnUpdateTick(object sender, EventArgs eventArgs)
        {
            if (TheGame == null || Game1.player == null || Game1.currentLocation == null)
                return;

            if (!TheGame.IsActive || Game1.paused || Game1.activeClickableMenu != null)
                return;

            if (Game1.currentLocation.currentEvent != null)
            {
                Player.addedSpeed = 0;
            }
            else
            {
                if (ModConfig.EnableHorseSpeedOverride && Player.getMount() != null)
                    Player.addedSpeed = ModConfig.HorseSpeed;
                if (ModConfig.EnableRunningSpeedOverride && Player.running)
                    Player.addedSpeed = ModConfig.PlayerRunningSpeed;
                else if (ModConfig.EnableWalkingSpeedOverride && !Player.running)
                    Player.addedSpeed = ModConfig.PlayerWalkingSpeed;
                else
                    Player.addedSpeed = 0;

                if (ModConfig.EnableDiagonalMovementSpeedFix)
                    Player.movementDirections?.Clear();
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = ModConfig.ReloadConfig();
                Log.AsyncG("Config Reloaded for " + GetType().Name);
            }
        }
    }

    public class MovementConfig : Config
    {
        public bool EnableDiagonalMovementSpeedFix { get; set; }

        public bool EnableWalkingSpeedOverride { get; set; }
        public int PlayerWalkingSpeed { get; set; }

        public bool EnableRunningSpeedOverride { get; set; }
        public int PlayerRunningSpeed { get; set; }

        public bool EnableHorseSpeedOverride { get; set; }
        public int HorseSpeed { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            EnableDiagonalMovementSpeedFix = true;

            EnableWalkingSpeedOverride = false;
            PlayerWalkingSpeed = 2;

            EnableRunningSpeedOverride = false;
            PlayerRunningSpeed = 5;

            EnableHorseSpeedOverride = false;
            HorseSpeed = 5;

            return this as T;
        }
    }
}
