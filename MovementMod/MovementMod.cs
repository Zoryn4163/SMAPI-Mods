using System;
using System.Linq;
using System.Reflection;
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

        public override void Entry(params object[] objects)
        {
            ModConfig = new MovementConfig().InitializeConfig(BaseConfigPath);
            SprintKey = KeyFromString();

            GameEvents.UpdateTick += GameEventsOnUpdateTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
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
                ModConfig = ModConfig.ReloadConfig();
                SprintKey = KeyFromString();
                Log.Success("Config Reloaded for " + GetType().Name);
            }
        }

        public static Keys KeyFromString()
        {
            Keys k;
            if (Enum.TryParse(ModConfig.SprintKey, out k))
            {
                Log.Info($"Bound key '{ModConfig.SprintKey}' for sprinting.");
            }
            else
            {
                Log.Error($"Failed to find specified key '{ModConfig.SprintKey}', using default 'LeftShift' for sprinting.");
                k = Keys.LeftShift;
            }

            return k;
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

        public bool EnableSprinting { get; set; }
        public int PlayerSprintingSpeedMultiplier { get; set; }
        public string SprintKey { get; set; }
        public bool SprintingDrainsStamina { get; set; }
        public float SprintingStaminaDrainPerSecond { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            EnableDiagonalMovementSpeedFix = true;

            EnableWalkingSpeedOverride = false;
            PlayerWalkingSpeed = 2;

            EnableRunningSpeedOverride = false;
            PlayerRunningSpeed = 5;

            EnableHorseSpeedOverride = false;
            HorseSpeed = 5;

            EnableSprinting = false;
            PlayerSprintingSpeedMultiplier = 2;
            SprintKey = "LeftShift";
            SprintingDrainsStamina = true;
            SprintingStaminaDrainPerSecond = 15;

            return this as T;
        }
    }
}
