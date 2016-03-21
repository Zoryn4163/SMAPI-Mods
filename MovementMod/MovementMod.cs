using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ModConfig = new MovementConfig();
            ModConfig = (MovementConfig)Config.InitializeConfig(Config.GetBasePath(this), ModConfig);

            GameEvents.UpdateTick += GameEventsOnUpdateTick;

            Console.WriteLine("MovementModifier by Zoryn => Initialized");
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
    }

    public class MovementConfig : Config
    {
        public bool EnableDiagonalMovementSpeedFix { get; set; }
        public bool EnableWalkingSpeedOverride { get; set; }
        public int PlayerWalkingSpeed { get; set; }
        public bool EnableRunningSpeedOverride { get; set; }
        public int PlayerRunningSpeed { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            EnableDiagonalMovementSpeedFix = true;
            EnableWalkingSpeedOverride = false;
            PlayerWalkingSpeed = 2;
            EnableRunningSpeedOverride = false;
            PlayerRunningSpeed = 5;
            return this;
        }
    }
}
