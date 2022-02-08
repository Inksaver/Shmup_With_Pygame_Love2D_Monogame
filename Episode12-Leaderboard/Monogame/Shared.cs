using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Shmup
{
    internal static class Shared
    {
        /// <summary>
        /// This static class is a shared repository of global variables
        /// </summary>
        public static readonly int WIDTH = 480;
        public static readonly int HEIGHT = 600;
        public static Dictionary<string, int> GameStates = new Dictionary<string, int> 
        {
            {"menu", 1},
            {"play", 2},
            {"leaderboard",3},
            {"quit", 4}
        };
        public static readonly Color GREEN = new Color(0, 255, 0);
        public static readonly Color DARKBLUE = new Color(0, 0, 12);
        public static bool Debug = false;
        public static int GameState = 1; // default gamestate
        public static int Score = 0;
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static string InputText = "";
        public static bool PlayMusic = false;
        public static void DrawString(SpriteBatch spriteBatch, string text, string size, float posX, float posY, Color color, string align = "centre", float scale = 1.0f)
        {
            // fontArial is size 64 default
            SpriteFont spriteFont = Fonts[size];

            switch (align) // posX parameter is ignored for centre / right
            {
                case "centre": { posX = Shared.WIDTH / 2f - spriteFont.MeasureString(text).X * scale / 2; break; }
                case "center": { posX = Shared.WIDTH / 2f - spriteFont.MeasureString(text).X * scale / 2; break; }
                case "right":  { posX = Shared.WIDTH - spriteFont.MeasureString(text).X * scale; break; }
            }
            if (scale == 1.0f)
            {
                spriteBatch.DrawString(spriteFont: spriteFont,
                                        text: text,
                                        position: new Vector2(posX, posY),
                                        color: color);
            }
            else
            {
                spriteBatch.DrawString(spriteFont: spriteFont,
                                        text: text,
                                        position: new Vector2(posX, posY),
                                        color: color,
                                        rotation: 0f,
                                        origin: Vector2.Zero,
                                        scale: scale,
                                        effects: SpriteEffects.None,
                                        layerDepth: 0);
            }
        }
        public static void DisplayBox(SpriteBatch spriteBatch, string text, string size, RectangleF rect, Color lineColour, Color backColour, Color textColour)
        {
            spriteBatch.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height + 2, backColour);
            spriteBatch.DrawRectangle(rect, lineColour);
            if (text != "")
                DrawString(spriteBatch, text, size, rect.X + 1, rect.Y -1, textColour, "left");
        }
    }
}
