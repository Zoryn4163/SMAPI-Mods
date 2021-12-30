using RegenMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using SFarmer = StardewValley.Farmer;

namespace RegenMod
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        /// <summary>The health regen carried over from the previous tick.</summary>
        private readonly PerScreen<float> Health = new();

        /// <summary>The stamina regen carried over from the previous tick.</summary>
        private readonly PerScreen<float> Stamina = new();

        /// <summary>The time in milliseconds since the player last moved or used a tool.</summary>
        private readonly PerScreen<double> TimeSinceLastMoved = new();

        private float ElapsedSeconds => (float)(Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds / 1000);


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Input.ButtonsChanged += this.OnButtonChanged;
        }


        /*********
        ** Private methods
        *********/
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

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.paused || Game1.activeClickableMenu != null)
                return;

            SFarmer player = Game1.player;

            //detect movement or tool use
            this.TimeSinceLastMoved.Value += Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds;
            if (player.timerSinceLastMovement == 0)
                this.TimeSinceLastMoved.Value = 0;
            if (player.UsingTool)
                this.TimeSinceLastMoved.Value = 0;

            // health regen
            if (this.Config.RegenHealthConstant)
                this.Health.Value += this.Config.RegenHealthConstantAmountPerSecond * this.ElapsedSeconds;
            if (this.Config.RegenHealthStill)
            {
                if (this.TimeSinceLastMoved.Value > this.Config.RegenHealthStillTimeRequiredMS)
                    this.Health.Value += this.Config.RegenHealthStillAmountPerSecond * this.ElapsedSeconds;
            }
            if (player.health + this.Health.Value >= player.maxHealth)
            {
                player.health = player.maxHealth;
                this.Health.Value = 0;
            }
            else if (this.Health.Value >= 1)
            {
                player.health += 1;
                this.Health.Value -= 1;
            }
            else if (this.Health.Value <= -1)
            {
                player.health -= 1;
                this.Health.Value += 1;
            }

            // stamina regen
            if (this.Config.RegenStaminaConstant)
                this.Stamina.Value += this.Config.RegenStaminaConstantAmountPerSecond * this.ElapsedSeconds;
            if (this.Config.RegenStaminaStill)
            {
                if (this.TimeSinceLastMoved.Value > this.Config.RegenStaminaStillTimeRequiredMS)
                    this.Stamina.Value += this.Config.RegenStaminaStillAmountPerSecond * this.ElapsedSeconds;
            }
            if (player.Stamina + this.Stamina.Value >= player.MaxStamina)
            {
                player.Stamina = player.MaxStamina;
                this.Stamina.Value = 0;
            }
            else if (this.Stamina.Value >= 1)
            {
                player.Stamina += 1;
                this.Stamina.Value -= 1;
            }
            else if (this.Stamina.Value <= -1)
            {
                player.Stamina -= 1;
                this.Stamina.Value += 1;
            }
        }
    }
}