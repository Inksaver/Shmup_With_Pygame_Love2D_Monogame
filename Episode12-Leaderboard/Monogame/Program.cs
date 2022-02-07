using System;

namespace Shmup
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ShmupMain())
                game.Run();
        }
    }
}
