using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CalendarAnywhere
{
    /// <summary>The main entry point.</summary>
    public class CalendarAnywhere : Mod
    {
        /*********
        ** Properties
        *********/
        private MouseState MState;

        private string OpenPath;
        private Texture2D OpenTexture;

        private readonly Rectangle Target = new Rectangle(x: (Game1.graphics.GraphicsDevice.Viewport.Width - 300) + 108, y: (Game1.tileSize / 8) + 20, width: 160, height: 41);

        private Rectangle MouseRect => new Rectangle(Game1.oldMouseState.X, Game1.oldMouseState.Y, 64, 64);
        private Point ClickPoint => new Point(this.MState.X, this.MState.Y);


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.OpenPath = Path.Combine(helper.DirectoryPath, "open.png");

            ControlEvents.MouseChanged += this.ControlEvents_MouseChanged;
            ControlEvents.ControllerButtonPressed += this.ControlEvents_ControllerButtonPressed;
            GraphicsEvents.OnPostRenderEvent += this.GraphicsEvents_OnPostRenderEvent;

            GameEvents.FirstUpdateTick += (sender, args) =>
            {
                if (File.Exists(OpenPath))
                {
                    using (FileStream fs = File.OpenRead(OpenPath))
                        OpenTexture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, fs);
                }
            };
        }


        /*********
        ** Private methods
        *********/
        private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            MState = e.NewState;

            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            if (Game1.didPlayerJustLeftClick())
            {
                if (this.Target.Contains(this.ClickPoint))
                    Game1.activeClickableMenu = new Billboard();
            }
        }

        private void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed e)
        {
            MState = Game1.oldMouseState;

            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            if (e.ButtonPressed == Buttons.A && this.Target.Contains(this.ClickPoint))
                Game1.activeClickableMenu = new Billboard();
        }

        private void GraphicsEvents_OnPostRenderEvent(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null || OpenTexture == null)
                return;

            if (this.Target.Contains(Game1.getOldMouseX(), Game1.getOldMouseX()))
            {
                Game1.spriteBatch.Draw(OpenTexture, this.Target, Color.White);
                Game1.spriteBatch.DrawString(Game1.smallFont, "Calendar", new Vector2(this.Target.X, this.Target.Y), Color.Black, 0, new Vector2(-3, -5), 1.4f, SpriteEffects.None, 0.001f);
                Game1.spriteBatch.Draw(Game1.mouseCursors, MouseRect, new Rectangle(0, 0, 16, 16), Color.White);
            }
        }
    }
}
