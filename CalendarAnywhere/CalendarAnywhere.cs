using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CalendarAnywhere
{
    /// <summary>The main entry point.</summary>
    public class CalendarAnywhere : Mod
    {
        /*********
        ** Properties
        *********/
        private MouseState MState;

        private Rectangle MouseRect => new Rectangle(Game1.oldMouseState.X, Game1.oldMouseState.Y, 64, 64);
        private Point ClickPoint => new Point(this.MState.X, this.MState.Y);


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ControlEvents.MouseChanged += this.ControlEvents_MouseChanged;
            ControlEvents.ControllerButtonPressed += this.ControlEvents_ControllerButtonPressed;
        }


        /*********
        ** Private methods
        *********/
        private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            MState = e.NewState;

            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            if (Game1.didPlayerJustLeftClick())
            {
                Rectangle target = this.GetTarget();
                if (target.Contains(this.ClickPoint))
                    Game1.activeClickableMenu = new Billboard();
            }
        }

        private void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed e)
        {
            MState = Game1.oldMouseState;

            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            if (e.ButtonPressed == Buttons.A && this.GetTarget().Contains(this.ClickPoint))
                Game1.activeClickableMenu = new Billboard();
        }

        private Rectangle GetTarget()
        {
            return new Rectangle(
                x: (Game1.graphics.GraphicsDevice.Viewport.Width - 300) + 108,
                y: (Game1.tileSize / 8) + 20,
                width: 160,
                height: 41
            );
        }
    }
}
