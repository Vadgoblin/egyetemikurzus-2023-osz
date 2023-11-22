
namespace E394KZ.Display
{
    internal static class Disply
    {
        private static int lastScreenWidth = Console.WindowWidth;
        private static int lastScreenHeight = Console.WindowHeight;

        private static bool IsConsoleResized()
        {
            if(lastScreenWidth != Console.WindowWidth || lastScreenHeight != Console.WindowHeight)
            {
                lastScreenWidth = Console.WindowWidth;
                lastScreenHeight = Console.WindowHeight;
                return true;
            }
            return false;
        }

        public static void Draw(Frame frame)
        {
            if(IsConsoleResized())
            {
                Console.Clear();
            }

            //for()
        }
    }
}
