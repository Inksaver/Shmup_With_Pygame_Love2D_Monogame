using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shmup
{
    internal static class Shared
    {
        /// <summary>
        /// This static class is a shared repository of global variables
        /// </summary>
        public static int WIDTH = 480;
        public static int HEIGHT = 600;
        public static Dictionary<string, int> GameStates = new Dictionary<string, int> 
        {
            {"menu", 1},
            {"play", 2},
            {"quit", 3}
        };
        public static int GameState = 1; // default gamestate
        public static bool Debug = false;
        public static int Score = 0;
        public static Color GREEN = new Color(0, 255, 0); // Color.Green is (0, 128, 0) half as bright
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static void DrawString(SpriteBatch spriteBatch, string text, string size, float posX, float posY, Color color, string align = "centre")
        {
            // fontArial is size 64 default
            SpriteFont spriteFont = Fonts[size];

            switch (align) // posX parameter is ignored for centre / right
            {
                case "centre": { posX = Shared.WIDTH / 2f - spriteFont.MeasureString(text).X / 2; break; }
                case "center": { posX = Shared.WIDTH / 2f - spriteFont.MeasureString(text).X / 2; break; }
                case "right": { posX = Shared.WIDTH - spriteFont.MeasureString(text).X; break; }
            }
            spriteBatch.DrawString(spriteFont: spriteFont,
                                    text: text,
                                    position: new Vector2(posX, posY),
                                    color: color);
        }
    }
}
