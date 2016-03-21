using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Menus;

namespace CalendarAnywhere
{
    public class CalendarAnywhere : Mod
    {
        public static SGame TheGame => Program.gamePtr;
        public static MouseState MState { get; set; }
        public static GamePadState GState { get; set; }

        public static string OpenPath { get; set; }
        public static Texture2D OpenTexture { get; set; }

        public static int TargX => (TheGame.GraphicsDevice.Viewport.Width - 300) + 108;
        public static int TargY => (Game1.tileSize / 8) + 20;
        public static int TargW = 160;
        public static int TargH = 41;

        public static Rectangle TargRect => new Rectangle(TargX, TargY, TargW, TargH);
        public static Rectangle MousePointRect => new Rectangle(Game1.oldMouseState.X, Game1.oldMouseState.Y, 1, 1);
        public static Rectangle MouseRect => new Rectangle(Game1.oldMouseState.X, Game1.oldMouseState.Y, 64, 64);
        public static Rectangle ClickRect => new Rectangle(MState.X, MState.Y, 1, 1);

        public override void Entry(params object[] objects)
        {
            OpenPath = PathOnDisk + "\\open.png";

            StardewModdingAPI.Events.ControlEvents.MouseChanged += ControlEvents_MouseChanged;
            StardewModdingAPI.Events.ControlEvents.ControllerButtonPressed += ControlEvents_ControllerButtonPressed;
            StardewModdingAPI.Events.GraphicsEvents.DrawTick += GraphicsEvents_DrawTick;

            StardewModdingAPI.Events.GameEvents.FirstUpdateTick += (sender, args) =>
            {
                if (File.Exists(OpenPath))
                {
                    FileStream fs = File.OpenRead(OpenPath);
                    OpenTexture = Texture2D.FromStream(TheGame.GraphicsDevice, fs);
                    fs.Close();
                }
            };

            Log.Info("CalendarAnywhere by Zoryn => Initialization Completed");
        }

        private void ControlEvents_MouseChanged(object sender, StardewModdingAPI.Events.EventArgsMouseStateChanged e)
        {
            MState = e.NewState;

            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            if (Game1.didPlayerJustLeftClick())
            {
                if (ClickRect.Intersects(TargRect))
                {
                    Game1.activeClickableMenu = new Billboard();
                }
            }
        }

        private void ControlEvents_ControllerButtonPressed(object sender, StardewModdingAPI.Events.EventArgsControllerButtonPressed e)
        {
            GState = Game1.oldPadState;
            MState = Game1.oldMouseState;

            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            if ((e.ButtonPressed & Buttons.A) != 0)
                if (ClickRect.Intersects(TargRect))
                    Game1.activeClickableMenu = new Billboard();
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null || OpenTexture == null)
                return;

            if (MousePointRect.Intersects(TargRect))
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                Game1.spriteBatch.Draw(OpenTexture, TargRect, Color.White);
                Game1.spriteBatch.DrawString(Game1.smallFont, "Calendar", new Vector2(TargX, TargY), Color.Black, 0, new Vector2(-3, -5), 1.4f, SpriteEffects.None, 0.001f);
                Game1.spriteBatch.Draw(Game1.mouseCursors, MouseRect, new Rectangle(0, 0, 16, 16), Color.White);
                Game1.spriteBatch.End();
            }
        }
    }
}
