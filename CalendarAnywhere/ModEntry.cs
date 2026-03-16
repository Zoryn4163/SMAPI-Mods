using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Zoryn.Common;

namespace CalendarAnywhere;

/// <summary>The main entry point.</summary>
public class ModEntry : Mod
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override void Entry(IModHelper helper)
    {
        CommonHelper.RemoveObsoleteFiles(this, "CalendarAnywhere.pdb");

        helper.Events.Input.ButtonPressed += this.OnButtonPressed;
    }


    /*********
    ** Private methods
    *********/
    /// <inheritdoc cref="IInputEvents.ButtonPressed" />
    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
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
