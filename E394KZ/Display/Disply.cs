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

            for (int y = 0; y < frame.Height && y < Console.WindowHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < frame.Width && x < Console.WindowWidth; x++)
                {
                    Console.ForegroundColor = frame[x, y].foregroundColor;
                    Console.BackgroundColor = frame[x, y].backgroundColor;
                    Console.Write(frame[x, y].letter);
                }
            }
        }
    }
}
