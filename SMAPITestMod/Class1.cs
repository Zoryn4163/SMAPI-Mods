using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using Object = StardewValley.Object;

namespace SMAPITestMod
{
    public class Class1 : Mod
    {
        public static MyConfig PerSaveConfig { get; set; }

        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            Log.Verbose("SomeMod by Zoryn => Initialized");
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            Console.WriteLine(e.LoadedGame);
            if (e.LoadedGame)
            {
                PerSaveConfig = new MyConfig().InitializeConfig(PerSaveConfigPath);
            }
        }

        public void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            //This stuff will happen every update (60 times a second without lag)
        }
    }

    public class MyConfig : Config
    {
        public List<Object> playerItems { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            playerItems = new List<Object>();
            playerItems.Add(new Object(80, 1));
            return this as T;
        }
    }
}
