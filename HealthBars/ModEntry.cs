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

        /// <summary>The cached health bar texture.</summary>
        private Texture2D BarTexture;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // read config
            this.Config = helper.ReadConfig<ModConfig>();

            // build bar texture
            this.BarTexture = this.GetBarTexture();

            // hook events
            helper.Events.Display.RenderedWorld += this.OnRenderedWorld;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <inheritdoc cref="IDisplayEvents.RenderedWorld"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            Monster[] monsters = Game1.currentLocation.characters.OfType<Monster>().ToArray();
            if (!monsters.Any())
                return;

            SpriteFont font = Game1.smallFont;
            SpriteBatch batch = Game1.spriteBatch;

            foreach (Monster monster in monsters)
            {
                if (monster.MaxHealth < monster.Health)
                    monster.MaxHealth = monster.Health;

                if (monster.MaxHealth == monster.Health && !this.Config.DisplayHealthWhenNotDamaged)
                    continue;

                Vector2 size = new Vector2(monster.Sprite.SpriteWidth, monster.Sprite.SpriteHeight) * Game1.pixelZoom;

                Vector2 screenLoc = Game1.GlobalToLocal(monster.position);
                screenLoc.X += size.X / 2 - this.Config.BarWidth / 2.0f;
                screenLoc.Y -= this.Config.BarHeight;

                float fill = monster.Health / (float)monster.MaxHealth;

                batch.Draw(this.BarTexture, screenLoc + new Vector2(this.Config.BarBorderWidth, this.Config.BarBorderHeight), this.BarTexture.Bounds, Color.Lerp(this.Config.LowHealthColor, this.Config.HighHealthColor, fill), 0.0f, Vector2.Zero, new Vector2(fill, 1.0f), SpriteEffects.None, 0);

                if (this.Config.DisplayCurrentHealthNumber)
                {
                    string textLeft = monster.Health.ToString();
                    Vector2 textSizeL = font.MeasureString(textLeft);
                    if (this.Config.DisplayTextBorder)
                        batch.DrawString(Game1.smallFont, textLeft, screenLoc - new Vector2(-1.0f, textSizeL.Y + 1.65f), this.Config.TextBorderColor, 0.0f, Vector2.Zero, 0.66f, SpriteEffects.None, 0);
                    batch.DrawString(font, textLeft, screenLoc - new Vector2(0.0f, textSizeL.Y + 1.0f), this.Config.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }

                if (this.Config.DisplayMaxHealthNumber)
                {
                    string textRight = monster.MaxHealth.ToString();
                    Vector2 textSizeR = font.MeasureString(textRight);
                    if (this.Config.DisplayTextBorder)
                        batch.DrawString(Game1.smallFont, textRight, screenLoc + new Vector2(this.Config.BarWidth, 0.0f) - new Vector2(textSizeR.X - 1f, textSizeR.Y + 1.65f), this.Config.TextBorderColor, 0.0f, Vector2.Zero, 0.66f, SpriteEffects.None, 0);
                    batch.DrawString(font, textRight, screenLoc + new Vector2(this.Config.BarWidth, 0.0f) - new Vector2(textSizeR.X, textSizeR.Y + 1.0f), this.Config.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        /// <inheritdoc cref="IInputEvents.ButtonsChanged"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (this.Config.ReloadKey.JustPressed())
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }

        /// <summary>Get a health bar texture.</summary>
        private Texture2D GetBarTexture()
        {
            // calculate size
            int innerBarWidth = this.Config.BarWidth - this.Config.BarBorderWidth * 2;
            int innerBarHeight = this.Config.BarHeight - this.Config.BarBorderHeight * 2;

            // get pixels
            var data = new uint[innerBarWidth * innerBarHeight];
            for (int i = 0; i < data.Length; i++)
                data[i] = 0xffffffff;

            // build texture
            var texture = new Texture2D(Game1.graphics.GraphicsDevice, innerBarWidth, innerBarHeight);
            texture.SetData(data);
            return texture;
        }
    }
}
