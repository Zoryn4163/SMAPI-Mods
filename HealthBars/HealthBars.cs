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
        private Texture2D TextureBar;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<HealthBarConfig>();

            GameEvents.FirstUpdateTick += this.GameEvents_FirstUpdateTick;
            GraphicsEvents.OnPostRenderEvent += this.GraphicsEvents_OnPostRenderEvent;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        private void GameEvents_FirstUpdateTick(object sender, EventArgs e)
        {
            int innerBarWidth = this.Config.BarWidth - this.Config.BarBorderWidth * 2;
            int innerBarHeight = this.Config.BarHeight - this.Config.BarBorderHeight * 2;

            this.TextureBar = new Texture2D(Game1.graphics.GraphicsDevice, innerBarWidth, innerBarHeight);
            var data = new uint[innerBarWidth * innerBarHeight];
            for (int i = 0; i < data.Length; i++)
                data[i] = 0xffffffff;
            this.TextureBar.SetData(data);
        }

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
                if (monster.maxHealth < monster.health)
                    monster.maxHealth = monster.health;

                if (monster.maxHealth == monster.health && !this.Config.DisplayHealthWhenNotDamaged)
                    continue;

                var size = new Vector2(monster.Sprite.spriteWidth, monster.Sprite.spriteHeight) * Game1.pixelZoom;

                Vector2 screenLoc = monster.Position - new Vector2(viewport.X, viewport.Y);
                screenLoc.X += size.X / 2 - this.Config.BarWidth / 2.0f;
                screenLoc.Y -= this.Config.BarHeight;

                float fill = monster.health / (float)monster.maxHealth;

                batch.Draw(this.TextureBar, screenLoc + new Vector2(this.Config.BarBorderWidth, this.Config.BarBorderHeight), this.TextureBar.Bounds, Color.Lerp(this.Config.LowHealthColor, this.Config.HighHealthColor, fill), 0.0f, Vector2.Zero, new Vector2(fill, 1.0f), SpriteEffects.None, 0);

                if (this.Config.DisplayCurrentHealthNumber)
                {
                    string textLeft = monster.health.ToString();
                    Vector2 textSizeL = font.MeasureString(textLeft);
                    if (this.Config.DisplayTextBorder)
                        batch.DrawString(Game1.smallFont, textLeft, screenLoc - new Vector2(-1.0f, textSizeL.Y + 1.65f), this.Config.TextBorderColor, 0.0f, Vector2.Zero, 0.66f, SpriteEffects.None, 0);
                    batch.DrawString(font, textLeft, screenLoc - new Vector2(0.0f, textSizeL.Y + 1.0f), this.Config.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }

                if (this.Config.DisplayMaxHealthNumber)
                {
                    string textRight = monster.maxHealth.ToString();
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
    }
}
