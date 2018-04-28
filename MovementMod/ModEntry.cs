using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovementMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using SFarmer = StardewValley.Farmer;

namespace MovementMod
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private ModConfig Config;
        private Keys SprintKey;

        private int CurrentSpeed;

        private Vector2 PrevPosition;
        private float ElapsedSeconds => (float)Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000f;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            this.SprintKey = this.Config.GetSprintKey(this.Monitor);

            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.paused || Game1.activeClickableMenu != null)
                return;

            if (Game1.currentLocation.currentEvent != null)
            {
                this.CurrentSpeed = 0;
                return;
            }

            SFarmer player = Game1.player;
            if (this.Config.EnableHorseSpeedOverride && player.getMount() != null)
                this.CurrentSpeed = this.Config.HorseSpeed;
            if (this.Config.EnableRunningSpeedOverride && player.running)
                this.CurrentSpeed = this.Config.PlayerRunningSpeed;
            else if (this.Config.EnableWalkingSpeedOverride && !player.running)
                this.CurrentSpeed = this.Config.PlayerWalkingSpeed;
            else
                this.CurrentSpeed = 0;

            if (Game1.oldKBState.IsKeyDown(this.SprintKey))
            {
                if (this.Config.SprintingDrainsStamina)
                {
                    float loss = this.Config.SprintingStaminaDrainPerSecond * this.ElapsedSeconds;
                    if (player.position != this.PrevPosition && player.stamina - loss > 0)
                    {
                        player.stamina -= loss;
                        this.CurrentSpeed *= this.Config.PlayerSprintingSpeedMultiplier;
                    }
                }
                else
                    this.CurrentSpeed *= this.Config.PlayerSprintingSpeedMultiplier;
            }

            player.addedSpeed = this.CurrentSpeed;

            this.PrevPosition = player.position;
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.SprintKey = this.Config.GetSprintKey(this.Monitor);
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
