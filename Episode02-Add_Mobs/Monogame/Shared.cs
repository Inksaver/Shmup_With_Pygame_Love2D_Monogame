using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Shmup
{
    internal static class Shared
    {
        /// <summary>
        /// This static class is a shared repository of global variables
        /// </summary>
        public static int WIDTH = 480;      // Window width set here
        public static int HEIGHT = 600;     // Window height set here
        public static Dictionary<string, int> gameStates = new Dictionary<string, int>
        {
            {"menu", 1},
            {"play", 2},
            {"quit", 3}
        };
        public static int gameState = 1;    // default gamestate (menu)
        public static Color GREEN = new Color(0,255,0); // Color.Green is (0, 128, 0) half as bright
    }
}
