using System;

namespace Platformer.src
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
