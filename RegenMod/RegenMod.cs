using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
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
        public static float TimeSinceLastMoved { get; private set; }

        public override void Entry(params object[] objects)
        {
            ModConfig = new RegenConfig().InitializeConfig(BaseConfigPath);

            GameEvents.UpdateTick += GameEvents_UpdateTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F4 To Reload Config)");
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F4)
            {
                ModConfig.ReloadConfig();
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

            TimeSinceLastMoved += 1;
            if (Game1.oldKBState.GetPressedKeys().Length > 0 || Game1.oldMouseState.LeftButton == ButtonState.Pressed || Game1.oldMouseState.RightButton == ButtonState.Pressed)
                TimeSinceLastMoved = 0;

            if (ModConfig.RegenHealth)
            {
                if (!ModConfig.RegenHealthOnlyWhileStill || (ModConfig.RegenHealthOnlyWhileStill && TimeSinceLastMoved > ModConfig.FramesUntilBeginHealthRegen))
                {
                    if (Player.health + HealthFloat >= Player.maxHealth)
                    {
                        HealthFloat = 0;
                        Player.health = Player.maxHealth;
                    }
                    else
                    {
                        HealthFloat += ModConfig.RegenHealthPerSecond * (float)(Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000) * (ModConfig.RegenHealthIsNegative ? -1 : 1);

                        if (HealthFloat > 1 && Player.health < Player.maxHealth)
                        {
                            Player.health = Player.health >= Player.maxHealth ? Player.maxHealth : Player.health + 1;
                            HealthFloat -= 1;
                        }
                        else if (HealthFloat < -1 && Player.health > 0)
                        {
                            Player.health = Player.health > 0 ? Player.health - 1 : 0;
                            HealthFloat += 1;
                        }
                    }
                }
            }

            if (ModConfig.RegenStamina)
            {
                if (!ModConfig.RegenStaminaOnlyWhileStill || (ModConfig.RegenStaminaOnlyWhileStill && TimeSinceLastMoved > ModConfig.FramesUntilBeginStaminaRegen))
                {
                    if (Player.stamina + StaminaFloat >= Player.maxStamina)
                    {
                        StaminaFloat = 0;
                        Player.stamina = Player.maxStamina;
                    }
                    else
                    {
                        StaminaFloat += ModConfig.RegenStaminaPerSecond * (float)(Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000) * (ModConfig.RegenStaminaIsNegative ? -1 : 1);

                        if (StaminaFloat > 1 && Player.stamina < Player.maxStamina)
                        {
                            Player.stamina = Player.stamina >= Player.maxStamina ? Player.maxStamina : Player.stamina + 1;
                            StaminaFloat -= 1;
                        }
                        else if (StaminaFloat < -1 && Player.stamina > 0)
                        {
                            Player.stamina = Player.stamina > 0 ? Player.stamina - 1 : 0;
                            StaminaFloat += 1;

                            if (Player.stamina <= 0.1f)
                                Player.exhausted = true;
                        }
                    }
                }
            }
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
