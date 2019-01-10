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
        /// <summary>The mod configuration.</summary>
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

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.paused || Game1.activeClickableMenu != null)
                return;

            if (Game1.currentLocation.currentEvent != null)
            {
                this.CurrentSpeed = 0;
                return;
            }

            SFarmer player = Game1.player;
            if (this.Config.HorseSpeed != 0 && player.mount != null)
                this.CurrentSpeed = this.Config.HorseSpeed;
            if (this.Config.PlayerRunningSpeed != 0 && player.running)
                this.CurrentSpeed = this.Config.PlayerRunningSpeed;
            else
                this.CurrentSpeed = 0;

            if (Game1.oldKBState.IsKeyDown(this.SprintKey))
            {
                if (this.Config.SprintingStaminaDrainPerSecond != 0 && player.position != this.PrevPosition)
                {
                    float loss = this.Config.SprintingStaminaDrainPerSecond * this.ElapsedSeconds;
                    if (player.stamina - loss > 0)
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

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F5)
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.SprintKey = this.Config.GetSprintKey(this.Monitor);
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
