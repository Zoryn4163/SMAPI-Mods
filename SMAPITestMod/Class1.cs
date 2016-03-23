using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

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

        private void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            PerSaveConfig = new MyConfig().InitializeConfig(PerSaveConfigPath);
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            //This stuff will happen every update (60 times a second without lag)
        }
    }

    public class MyConfig : Config
    {
        public string SomeValue { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            SomeValue = "something";
            return this as T;
        }
    }
}
