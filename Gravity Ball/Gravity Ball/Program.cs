using System;

namespace Blocking_Gravity
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// This is where the game starts. All applications must start
        /// inside this core main function.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
                //This is a comment made by me!
            }
        }
    }
#endif
}

