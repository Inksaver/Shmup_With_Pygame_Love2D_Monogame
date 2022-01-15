using System.Collections.Generic;
namespace Shmup
{
    /// <summary>
    /// Public static class as a repository for global variables
    /// </summary>
    internal static class Shared
    {
        public static int WIDTH = 480;
        public static int HEIGHT = 600;
        public static Dictionary<string, int> gameStates = new Dictionary<string, int>
        {
            {"menu", 1},
            {"play", 2},
            {"quit", 3}
        };
        public static int gameState = 1; // default gamestate (menu)
    }
}
