using System;

namespace Platformer
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new Main();
            game.Run();
        }
    }
}
