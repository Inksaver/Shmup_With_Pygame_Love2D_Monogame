using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
        public static Color GREEN = new Color(0, 255, 0); // Color.Green is (0, 128, 0) half as bright
        public static bool Debug = false;
    }
}
