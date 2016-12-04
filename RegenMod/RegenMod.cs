using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace RegenMod
{
    public class RegenMod : Mod
    {
        public static RegenConfig ModConfig { get; private set; }
        public static float HealthFloat { get; private set; }
        public static float StaminaFloat { get; private set; }

        public static Game1 TheGame => Program.gamePtr;
        public static Farmer Player => Game1.player;

        public static int UpdateIndex { get; private set; }
        public static double TimeSinceLastMoved { get; private set; }

        public static float ElapsedFloat => (float)(Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000);

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            ModConfig = helper.ReadConfig<RegenConfig>();

            GameEvents.UpdateTick += GameEvents_UpdateTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = this.Helper.ReadConfig<RegenConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (TheGame == null || Player == null)
                return;

            if (!TheGame.IsActive || Game1.paused || Game1.activeClickableMenu != null)
                return;

            if (UpdateIndex <= 60)
            {
                UpdateIndex += 1;
                return;
            }

            TimeSinceLastMoved += Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds;
            if (Game1.oldKBState.GetPressedKeys().Any() || Game1.oldMouseState.LeftButton == ButtonState.Pressed || Game1.oldMouseState.RightButton == ButtonState.Pressed)
                TimeSinceLastMoved = 0;

            #region Health Regen 

            if (ModConfig.RegenHealthConstant)
                HealthFloat += (ModConfig.RegenHealthConstantIsNegative ? -ModConfig.RegenHealthConstantAmountPerSecond : ModConfig.RegenHealthConstantAmountPerSecond) * ElapsedFloat;

            if (ModConfig.RegenHealthStill)
            {
                if (TimeSinceLastMoved > ModConfig.RegenHealthStillTimeRequiredMS)
                {
                    HealthFloat += (ModConfig.RegenHealthStillIsNegative ? -ModConfig.RegenHealthStillAmountPerSecond : ModConfig.RegenHealthStillAmountPerSecond) * ElapsedFloat;
                }
            }

            if (Player.health + HealthFloat >= Player.maxHealth)
            {
                Player.health = Player.maxHealth;
                HealthFloat = 0;
            }
            else if (HealthFloat >= 1)
            {
                Player.health += 1;
                HealthFloat -= 1;
            }
            else if (HealthFloat <= -1)
            {
                Player.health -= 1;
                HealthFloat += 1;
            }

            #endregion

            #region Stamina Regen 

            if (ModConfig.RegenStaminaConstant)
                StaminaFloat += (ModConfig.RegenStaminaConstantIsNegative ? -ModConfig.RegenStaminaConstantAmountPerSecond : ModConfig.RegenStaminaConstantAmountPerSecond) * ElapsedFloat;

            if (ModConfig.RegenStaminaStill)
            {
                if (TimeSinceLastMoved > ModConfig.RegenStaminaStillTimeRequiredMS)
                {
                    StaminaFloat += (ModConfig.RegenStaminaStillIsNegative ? -ModConfig.RegenStaminaStillAmountPerSecond : ModConfig.RegenStaminaStillAmountPerSecond) * ElapsedFloat;
                }
            }

            if (Player.Stamina + StaminaFloat >= Player.maxStamina)
            {
                Player.Stamina = Player.maxStamina;
                StaminaFloat = 0;
            }
            else if (StaminaFloat >= 1)
            {
                Player.Stamina += 1;
                StaminaFloat -= 1;
            }
            else if (StaminaFloat <= -1)
            {
                Player.Stamina -= 1;
                StaminaFloat += 1;
            }

            #endregion

            SGame.QueueDebugMessage("H: " + HealthFloat + " | S: " + StaminaFloat);
        }
    }
}
