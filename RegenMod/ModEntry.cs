using RegenMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
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
        private ModConfig Config;
        private float Health;
        private float Stamina;

        private double TimeSinceLastMoved;

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
            this.TimeSinceLastMoved += Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds;
            if (player.timerSinceLastMovement == 0)
                this.TimeSinceLastMoved = 0;
            if (player.UsingTool)
                this.TimeSinceLastMoved = 0;

            // health regen
            if (this.Config.RegenHealthConstant)
                this.Health += this.Config.RegenHealthConstantAmountPerSecond * this.ElapsedSeconds;
            if (this.Config.RegenHealthStill)
            {
                if (this.TimeSinceLastMoved > this.Config.RegenHealthStillTimeRequiredMS)
                    this.Health += this.Config.RegenHealthStillAmountPerSecond * this.ElapsedSeconds;
            }
            if (player.health + this.Health >= player.maxHealth)
            {
                player.health = player.maxHealth;
                this.Health = 0;
            }
            else if (this.Health >= 1)
            {
                player.health += 1;
                this.Health -= 1;
            }
            else if (this.Health <= -1)
            {
                player.health -= 1;
                this.Health += 1;
            }

            // stamina regen
            if (this.Config.RegenStaminaConstant)
                this.Stamina += this.Config.RegenStaminaConstantAmountPerSecond * this.ElapsedSeconds;
            if (this.Config.RegenStaminaStill)
            {
                if (this.TimeSinceLastMoved > this.Config.RegenStaminaStillTimeRequiredMS)
                    this.Stamina += this.Config.RegenStaminaStillAmountPerSecond * this.ElapsedSeconds;
            }
            if (player.Stamina + this.Stamina >= player.MaxStamina)
            {
                player.Stamina = player.MaxStamina;
                this.Stamina = 0;
            }
            else if (this.Stamina >= 1)
            {
                player.Stamina += 1;
                this.Stamina -= 1;
            }
            else if (this.Stamina <= -1)
            {
                player.Stamina -= 1;
                this.Stamina += 1;
            }
        }
    }
}