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

        public override void Entry(params object[] objects)
        {
            ModConfig = new RegenConfig().InitializeConfig(BaseConfigPath);

            GameEvents.UpdateTick += GameEvents_UpdateTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = ModConfig.ReloadConfig();
                Log.Success("Config Reloaded for " + GetType().Name);
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

    public class RegenConfig : Config
    {
        public bool RegenStaminaConstant { get; set; }
        public bool RegenStaminaConstantIsNegative { get; set; }
        public float RegenStaminaConstantAmountPerSecond { get; set; }

        public bool RegenStaminaStill { get; set; }
        public bool RegenStaminaStillIsNegative { get; set; }
        public float RegenStaminaStillAmountPerSecond { get; set; }
        public int RegenStaminaStillTimeRequiredMS { get; set; }



        public bool RegenHealthConstant { get; set; }
        public bool RegenHealthConstantIsNegative { get; set; }
        public float RegenHealthConstantAmountPerSecond { get; set; }

        public bool RegenHealthStill { get; set; }
        public bool RegenHealthStillIsNegative { get; set; }
        public float RegenHealthStillAmountPerSecond { get; set; }
        public int RegenHealthStillTimeRequiredMS { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            RegenStaminaConstant = false;
            RegenStaminaConstantIsNegative = false;
            RegenStaminaConstantAmountPerSecond = 0;

            RegenStaminaStill = false;
            RegenStaminaStillIsNegative = false;
            RegenStaminaStillAmountPerSecond = 0;
            RegenStaminaStillTimeRequiredMS = 1000;



            RegenHealthConstant = false;
            RegenHealthConstantIsNegative = false;
            RegenHealthConstantAmountPerSecond = 0;

            RegenHealthStill = false;
            RegenHealthStillIsNegative = false;
            RegenHealthStillAmountPerSecond = 0;
            RegenHealthStillTimeRequiredMS = 1000;

            return this as T;
        }
    }
}
