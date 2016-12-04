using System;
using System.Linq;
using Microsoft.Xna.Framework;
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

        public static Keys SprintKey { get; private set; }
        public static bool SprintKeyDown => Program.gamePtr.CurrentlyPressedKeys.Contains(SprintKey);

        public static int CurrentSpeed { get; private set; }

        public static Vector2 PrevPosition { get; private set; }
        public static float TickSecondMult => (float)Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000f;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            ModConfig = helper.ReadConfig<MovementConfig>();
            SprintKey = KeyFromString();

            GameEvents.UpdateTick += GameEventsOnUpdateTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }

        private void GameEventsOnUpdateTick(object sender, EventArgs e)
        {
            if (TheGame == null || Game1.player == null || Game1.currentLocation == null)
                return;

            if (!TheGame.IsActive || Game1.paused || Game1.activeClickableMenu != null)
                return;

            if (Game1.currentLocation.currentEvent != null)
            {
                CurrentSpeed = 0;
            }
            else
            {

                if (ModConfig.EnableHorseSpeedOverride && Player.getMount() != null)
                    CurrentSpeed = ModConfig.HorseSpeed;
                if (ModConfig.EnableRunningSpeedOverride && Player.running)
                    CurrentSpeed = ModConfig.PlayerRunningSpeed;
                else if (ModConfig.EnableWalkingSpeedOverride && !Player.running)
                    CurrentSpeed = ModConfig.PlayerWalkingSpeed;
                else
                    CurrentSpeed = 0;

                if (SprintKeyDown)
                {
                    if (ModConfig.SprintingDrainsStamina)
                    {
                        float loss = ModConfig.SprintingStaminaDrainPerSecond * TickSecondMult;
                        if (Player.position != PrevPosition && Player.stamina - loss > 0)
                        {
                            Player.stamina -= loss;
                            CurrentSpeed *= ModConfig.PlayerSprintingSpeedMultiplier;
                        }
                    }
                    else
                    {
                        CurrentSpeed *= ModConfig.PlayerSprintingSpeedMultiplier;
                    }
                }

                Player.addedSpeed = CurrentSpeed;

                SGame.QueueDebugMessage($"CSpeed: {CurrentSpeed} - ASpeed: {Player.addedSpeed}");

                if (ModConfig.EnableDiagonalMovementSpeedFix)
                    Player.movementDirections?.Clear();
            }

            PrevPosition = Player.position;
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = this.Helper.ReadConfig<MovementConfig>();
                SprintKey = KeyFromString();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }

        public Keys KeyFromString()
        {
            Keys k;
            if (Enum.TryParse(ModConfig.SprintKey, out k))
                this.Monitor.Log($"Bound key '{ModConfig.SprintKey}' for sprinting.");
            else
            {
                this.Monitor.Log($"Failed to find specified key '{ModConfig.SprintKey}', using default 'LeftShift' for sprinting.", LogLevel.Error);
                k = Keys.LeftShift;
            }

            return k;
        }
    }
}
