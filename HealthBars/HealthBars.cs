using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Monsters;

namespace HealthBars
{
    public class HealthBars : Mod
    {
        public static SGame TheGame => Program.gamePtr;

        public static HealthBarConfig ModConfig { get; set; }
        public static List<Monster> monsters { get; private set; }

        //public static RenderTarget2D RTarg { get; set; }

        Texture2D texBar;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            ModConfig = helper.ReadConfig<HealthBarConfig>();

            int innerBarWidth = ModConfig.BarWidth - ModConfig.BarBorderWidth * 2;
            int innerBarHeight = ModConfig.BarHeight - ModConfig.BarBorderHeight * 2;

            GameEvents.FirstUpdateTick += (sender, args) =>
            {
                texBar = new Texture2D(Game1.graphics.GraphicsDevice, innerBarWidth, innerBarHeight);
                var data = new uint[innerBarWidth * innerBarHeight];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0xffffffff;
                }
                texBar.SetData<uint>(data);
            };
            GraphicsEvents.OnPreRenderGuiEventNoCheck += GraphicsEvents_DrawTick;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            if (Game1.currentLocation == null)
                return;

            monsters = Game1.currentLocation.characters.OfType<Monster>().ToList();

            if (!monsters.Any())
                return;

            var font = Game1.smallFont;
            var batch = Game1.spriteBatch;
            var viewport = Game1.viewport;

            for (int i = 0; i < monsters.Count; i++)
            {
                var monster = monsters[i];
                if (monster.maxHealth < monster.health)
                {
                    monster.maxHealth = monster.health;
                }

                if (monster.maxHealth == monster.health && !ModConfig.DisplayHealthWhenNotDamaged)
                    continue;

                var animSprite = monster.Sprite;

                var size = new Vector2(animSprite.spriteWidth, animSprite.spriteHeight) * Game1.pixelZoom;

                var screenLoc = monster.Position - new Vector2(viewport.X, viewport.Y);
                screenLoc.X += size.X / 2 - ModConfig.BarWidth / 2.0f;
                screenLoc.Y -= ModConfig.BarHeight;

                var fill = monster.health / (float)monster.maxHealth;

                batch.Draw(texBar, screenLoc + new Vector2(ModConfig.BarBorderWidth, ModConfig.BarBorderHeight), texBar.Bounds, Color.Lerp(ModConfig.LowHealthColor, ModConfig.HighHealthColor, fill), 0.0f, Vector2.Zero, new Vector2(fill, 1.0f), SpriteEffects.None, 0);

                if (ModConfig.DisplayCurrentHealthNumber)
                {
                    var textLeft = monster.health.ToString();
                    var textSizeL = font.MeasureString(textLeft);
                    if (ModConfig.DisplayTextBorder)
                        batch.DrawString(Game1.borderFont, textLeft, screenLoc - new Vector2(-1.0f, textSizeL.Y + 1.65f), ModConfig.TextBorderColor, 0.0f, Vector2.Zero, 0.66f, SpriteEffects.None, 0);
                    batch.DrawString(font, textLeft, screenLoc - new Vector2(0.0f, textSizeL.Y + 1.0f), ModConfig.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }

                if (ModConfig.DisplayMaxHealthNumber)
                {
                    var textRight = monster.maxHealth.ToString();
                    var textSizeR = font.MeasureString(textRight);
                    if (ModConfig.DisplayTextBorder)
                        batch.DrawString(Game1.borderFont, textRight, screenLoc + new Vector2(ModConfig.BarWidth, 0.0f) - new Vector2(textSizeR.X - 1f, textSizeR.Y + 1.65f), ModConfig.TextBorderColor, 0.0f, Vector2.Zero, 0.66f, SpriteEffects.None, 0);
                    batch.DrawString(font, textRight, screenLoc + new Vector2(ModConfig.BarWidth, 0.0f) - new Vector2(textSizeR.X, textSizeR.Y + 1.0f), ModConfig.TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = this.Helper.ReadConfig<HealthBarConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
