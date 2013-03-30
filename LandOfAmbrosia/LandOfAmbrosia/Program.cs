using System;

namespace LandOfAmbrosia
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LandOfAmbrosiaGame game = new LandOfAmbrosiaGame())
            {
                game.Run();
            }
        }
    }
#endif
}

