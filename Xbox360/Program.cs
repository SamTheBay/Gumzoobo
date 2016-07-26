using System;

namespace BubbleGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BubbleGame game = new BubbleGame())
            {
                game.Run();
            }
        }
    }
}
