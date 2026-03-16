using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Zoryn.Common;

namespace JunimoDepositAnywhere;

/// <summary>The main entry point.</summary>
public class ModEntry : Mod
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override void Entry(IModHelper helper)
    {
        CommonHelper.RemoveObsoleteFiles(this, "JunimoDepositAnywhere.pdb");

        helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
    }


    /*********
    ** Protected methods
    *********/
    /// <inheritdoc cref="IGameLoopEvents.UpdateTicked" />
    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady || !e.IsMultipleOf(15)) // quarter-second
            return;

        if (Game1.activeClickableMenu is JunimoNoteMenu menu)
        {
            foreach (Bundle bundle in menu.bundles)
                bundle.depositsAllowed = true;
        }
    }
}
