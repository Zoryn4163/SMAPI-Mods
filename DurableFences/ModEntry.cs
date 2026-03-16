using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Zoryn.Common;

namespace DurableFences;

/// <summary>The main entry point.</summary>
public class ModEntry : Mod
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override void Entry(IModHelper helper)
    {
        CommonHelper.RemoveObsoleteFiles(this, "DurableFences.pdb");

        helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
    }


    /*********
    ** Private methods
    *********/
    /// <inheritdoc cref="IGameLoopEvents.OneSecondUpdateTicked" />
    private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    {
        Utility.ForEachLocation(location =>
        {
            foreach (Object obj in location.Objects.Values)
            {
                if (obj is Fence fence)
                    fence.health.Value = fence.maxHealth.Value;
            }

            return true;
        });
    }
}
