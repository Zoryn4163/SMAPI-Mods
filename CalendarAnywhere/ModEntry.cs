using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CalendarAnywhere
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (Context.IsPlayerFree && e.Button.IsUseToolButton() && this.GetTarget().Contains((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y))
                Game1.activeClickableMenu = new Billboard();
        }

        /// <summary>Get the clickable screen area that should open the billboard menu.</summary>
        private Rectangle GetTarget()
        {
            return new Rectangle(
                x: (Game1.viewport.Width - 300) + 108,
                y: (Game1.tileSize / 8) + 20,
                width: 160,
                height: 41
            );
        }
    }
}
