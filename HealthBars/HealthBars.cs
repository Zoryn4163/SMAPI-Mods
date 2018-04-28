using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using Rectangle = xTile.Dimensions.Rectangle;

namespace HealthBars
{
    /// <summary>The main entry point.</summary>
    public class HealthBars : Mod
    {
        /*********
        ** Properties
        *********/
        private HealthBarConfig Config;
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
            this.Config = helper.ReadConfig<HealthBarConfig>();

            // build bar texture
            this.BarTexture = this.GetBarTexture();

            // hook events
            GraphicsEvents.OnPostRenderEvent += this.GraphicsEvents_OnPostRenderEvent;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        private void GraphicsEvents_OnPostRenderEvent(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
                return;

            this.Monsters = Game1.currentLocation.characters.OfType<Monster>().ToArray();
            if (!this.Monsters.Any())
                return;

            SpriteFont font = Game1.smallFont;
            SpriteBatch batch = Game1.spriteBatch;
            Rectangle viewport = Game1.viewport;

            foreach (Monster monster in this.Monsters)
            {
                if (monster.MaxHealth < monster.Health)
                    monster.MaxHealth = monster.Health;

                if (monster.MaxHealth == monster.Health && !this.Config.DisplayHealthWhenNotDamaged)
                    continue;

                Vector2 size = new Vector2(monster.Sprite.SpriteWidth, monster.Sprite.SpriteHeight) * Game1.pixelZoom;

                Vector2 screenLoc = monster.Position - new Vector2(viewport.X, viewport.Y);
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

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                this.Config = this.Helper.ReadConfig<HealthBarConfig>();
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
