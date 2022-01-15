using System.Collections.Generic;

namespace Shmup
{
    internal static class Shared
    {
        /// <summary>
        /// This static class is a shared repository of global variables
        /// </summary>
        public static int WIDTH = 480;
        public static int HEIGHT = 600;
        public static Dictionary<string, int> gameStates = new Dictionary<string, int>
        {
            {"menu", 1},
            {"play", 2},
            {"quit", 3}
        };
        public static int gameState = 1; // default gamestate
    }
}
