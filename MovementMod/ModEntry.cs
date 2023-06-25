using Microsoft.Xna.Framework;
using MovementMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using Zoryn.Common;

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

        /// <summary>The current speed boost applied to the player.</summary>
        private readonly PerScreen<int> CurrentSpeed = new();

        /// <summary>The last known player position.</summary>
        private readonly PerScreen<Vector2> PrevPosition = new();

        private float ElapsedSeconds => (float)Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000f;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            CommonHelper.RemoveObsoleteFiles(this, "MovementMod.pdb");

            this.Config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Input.ButtonsChanged += this.OnButtonChanged;

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
                this.CurrentSpeed.Value = 0;
                return;
            }

            Farmer player = Game1.player;
            if (this.Config.HorseSpeed != 0 && player.mount != null)
                this.CurrentSpeed.Value = this.Config.HorseSpeed;
            if (this.Config.PlayerRunningSpeed != 0 && player.running)
                this.CurrentSpeed.Value = this.Config.PlayerRunningSpeed;
            else
                this.CurrentSpeed.Value = 0;

            if (this.Config.SprintKey.IsDown())
            {
                if (this.Config.SprintingStaminaDrainPerSecond != 0 && player.position != this.PrevPosition.Value)
                {
                    float loss = this.Config.SprintingStaminaDrainPerSecond * this.ElapsedSeconds;
                    if (player.stamina - loss > 0)
                    {
                        player.stamina -= loss;
                        this.CurrentSpeed.Value *= this.Config.PlayerSprintingSpeedMultiplier;
                    }
                }
                else
                    this.CurrentSpeed.Value *= this.Config.PlayerSprintingSpeedMultiplier;
            }

            player.addedSpeed = this.CurrentSpeed.Value;

            this.PrevPosition.Value = player.position;
        }

        /// <inheritdoc cref="IInputEvents.ButtonsChanged"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (this.Config.ReloadKey.JustPressed())
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
