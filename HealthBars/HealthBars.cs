using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewModdingAPI.Inheritance.Monsters;
using StardewValley;
using StardewValley.Monsters;

//Does not work until further notice D:
namespace HealthBars
{
    public class HealthBars : Mod
    {
        public static SGame TheGame => Program.gamePtr;

        public static HealthBarConfig ModConfig { get; set; }
        public static List<SMonster> monsters = new List<SMonster>();

        Texture2D texBar;

        public override void Entry(params object[] objects)
        {
            ModConfig = (HealthBarConfig)Config.InitializeConfig(Config.GetBasePath(this), new HealthBarConfig());

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
            GraphicsEvents.DrawTick += GraphicsEvents_DrawTick;
            LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
        }

        private void LocationEvents_CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            var gameLoc = Game1.currentLocation;
            if (gameLoc == null)
                return;

            try
            {
                monsters.Clear();
                for (int i = 0; i < gameLoc.characters.Count; i++)
                {
                    var v = gameLoc.characters[i];
                    if (v is Monster && !(v is SMonster))
                    {
                        SMonster s = SMonster.ConstructFromBaseClass((Monster) v);
                        s.BeforeDraw += S_BeforeDraw;
                        gameLoc.characters[i] = s;
                        monsters.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            return;

            if (monsters.Count < 1)
                return;

            var font = Game1.smallFont;
            var batch = Game1.spriteBatch;
            var viewport = Game1.viewport;

            
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);


            foreach (var monster in monsters)
            {
                Vector2 screenLoc = monster.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize * 3 / 4 + Game1.pixelZoom * 2), (float) (Game1.tileSize / 4 + monster.yJumpOffset));

                //batch.Draw(monster.Sprite.Texture, monster.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize * 3 / 4 + Game1.pixelZoom * 2), (float)(Game1.tileSize / 4 + monster.yJumpOffset)), new Rectangle?(monster.Sprite.SourceRect), Color.White, monster.rotation, new Vector2(16f, 16f), Math.Max(0.2f, monster.scale) * (float)Game1.pixelZoom, monster.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, monster.drawOnTop ? 0.991f : ((float)monster.getStandingY() / 10000f)));

                if (monster.maxHealth < monster.health)
                {
                    monster.maxHealth = monster.health;
                }

                if (monster.maxHealth == monster.health && !ModConfig.DisplayHealthWhenNotDamaged)
                    continue;

                var animSprite = monster.Sprite;

                var size = new Vector2(animSprite.spriteWidth, animSprite.spriteHeight) * Game1.pixelZoom;
                /*
                var screenLoc = monster.Position - new Vector2(viewport.X, viewport.Y);
                screenLoc.X += size.X / 2 - ModConfig.BarWidth / 2.0f;
                screenLoc.Y -= ModConfig.BarHeight;
                */

                //var innerBarWidth = ModConfig.BarWidth - ModConfig.BarBorderWidth * 2;
                //var innerBarHeight = ModConfig.BarHeight - ModConfig.BarBorderHeight * 2;

                

                var fill = monster.health / (float)monster.maxHealth;

                //batch.Draw(tex, screenLoc, tex.Bounds, colWhite, 0.0f, Vector2.Zero, new Vector2((float) ModConfig.BarWidth / tex.Bounds.Width, (float) ModConfig.BarHeight / tex.Bounds.Height), SpriteEffects.None, 0);

                //Console.WriteLine(typeof (Game1).GetBaseFieldInfo("screen"));

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

            batch.End();
        }

        private void S_BeforeDraw(object sender, EventArgs e)
        {
            return;
            if (sender is SMonster)
            {
                SMonster s = sender as SMonster;
                Console.WriteLine(s);
            }
        }
    }

    public class HealthBarConfig : Config
    {
        public bool DisplayHealthWhenNotDamaged { get; set; }

        public bool DisplayMaxHealthNumber { get; set; }
        public bool DisplayCurrentHealthNumber { get; set; }

        public bool DisplayTextBorder { get; set; }

        public Color TextColor { get; set; }
        public Color TextBorderColor { get; set; }

        public Color LowHealthColor { get; set; }
        public Color HighHealthColor { get; set; }

        public int BarWidth { get; set; }
        public int BarHeight { get; set; }

        public int BarBorderWidth { get; set; }
        public int BarBorderHeight { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            DisplayHealthWhenNotDamaged = false;

            DisplayMaxHealthNumber = true;
            DisplayCurrentHealthNumber = true;

            DisplayTextBorder = true;

            TextColor = Color.White;
            TextBorderColor = Color.Black;

            LowHealthColor = Color.DarkRed;
            HighHealthColor = Color.LimeGreen;

            BarWidth = 90;
            BarHeight = 15;

            BarBorderWidth = 2;
            BarBorderHeight = 2;
            return this;
        }
    }
}
