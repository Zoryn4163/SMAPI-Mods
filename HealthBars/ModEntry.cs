using System.Linq;
using HealthBars.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using Rectangle = xTile.Dimensions.Rectangle;

namespace HealthBars
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        private Monster[] Monsters;
        private Texture2D BarTexture;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // read config
            Config = helper.ReadConfig<ModConfig>();

            // build bar texture
            BarTexture = GetBarTexture();

            // hook events
            helper.Events.Display.Rendered += OnRendered;
            helper.Events.Input.ButtonPressed += OnButtonPressed;

            Monitor.Log("Initialized (press F5 to reload config)");
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRendered(object sender, RenderedEventArgs e)
        {
            if (!Context.IsWorldReady) return;

            Monsters = Game1.currentLocation.characters.OfType<Monster>().ToArray();
            if (!Monsters.Any()) return;

            SpriteFont font = Game1.smallFont;
            SpriteBatch batch = Game1.spriteBatch;
            Rectangle viewport = Game1.uiViewport;

            foreach (Monster monster in Monsters)
            {
                if (monster.MaxHealth < monster.Health) monster.MaxHealth = monster.Health;

                if (monster.MaxHealth == monster.Health && !Config.DisplayHealthWhenNotDamaged) continue;

                Vector2 size = new Vector2(monster.Sprite.SpriteWidth, monster.Sprite.SpriteHeight) * Game1.pixelZoom;
                size *= Game1.options.zoomLevel / Game1.options.uiScale;
                Vector2 screenLoc = monster.Position - new Vector2(viewport.X, viewport.Y);
                screenLoc.X += size.X / 2 - Config.BarWidth / 2.0f;
                screenLoc.Y -= Config.BarHeight;

                float fill = monster.Health / (float) monster.MaxHealth;

                batch.Draw(BarTexture, screenLoc + new Vector2(Config.BarBorderWidth, Config.BarBorderHeight),
                    BarTexture.Bounds, Color.Lerp(Config.LowHealthColor, Config.HighHealthColor, fill), 0.0f,
                    Vector2.Zero, new Vector2(fill, 1.0f), SpriteEffects.None, 0);

                if (Config.DisplayCurrentHealthNumber)
                {
                    string textLeft = monster.Health.ToString();
                    Vector2 textSizeL = font.MeasureString(textLeft);
                    batch.DrawString(font, textLeft, screenLoc - new Vector2(0.0f, textSizeL.Y + 1.0f),
                        Config.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }

                if (Config.DisplayMaxHealthNumber)
                {
                    string textRight = monster.MaxHealth.ToString();
                    Vector2 textSizeR = font.MeasureString(textRight);
                    batch.DrawString(font, textRight,
                        screenLoc + new Vector2(Config.BarWidth, 0.0f) - new Vector2(textSizeR.X, textSizeR.Y + 1.0f),
                        Config.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F5)
            {
                Config = Helper.ReadConfig<ModConfig>();
                Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }

        /// <summary>Get a health bar texture.</summary>
        private Texture2D GetBarTexture()
        {
            // calculate size
            int innerBarWidth = Config.BarWidth - Config.BarBorderWidth * 2;
            int innerBarHeight = Config.BarHeight - Config.BarBorderHeight * 2;

            // get pixels
            var data = new uint[innerBarWidth * innerBarHeight];
            for (int i = 0; i < data.Length; i++) data[i] = 0xffffffff;

            // build texture
            var texture = new Texture2D(Game1.graphics.GraphicsDevice, innerBarWidth, innerBarHeight);
            texture.SetData(data);
            return texture;
        }
    }
}