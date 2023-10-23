class Program
{
    const uint CANVAS_WIDTH = 10;
    const uint CANVAS_HEIGHT = 20;

    static void Main(string[] args)
    {
        var canvas = new ConsoleColor?[CANVAS_WIDTH, CANVAS_HEIGHT];
        var defaultbackgdoundcolor = Console.BackgroundColor;


        canvas[2, 3] = ConsoleColor.White;

        PrintCanvas(canvas, 0, 0, defaultbackgdoundcolor);
    }

    static void PrintCanvas(ConsoleColor?[,] canvas, uint woffset, uint hoffset, ConsoleColor backgdoundColor)
    {
        for(uint hindex = hoffset; hindex < woffset + Console.WindowWidth && hindex < canvas.GetLength(1); hindex+=2)
        {
            for(uint windex = woffset; windex < hoffset + Console.WindowWidth && windex < canvas.GetLength(0); windex++)
            {
                var upper = canvas[windex, hindex] ?? backgdoundColor;
                var lower = canvas[windex, hindex + 1] ?? backgdoundColor;

                if (upper == lower)
                {
                    Console.BackgroundColor = backgdoundColor;
                    Console.Write(' ');
                }
                else
                {
                    Console.ForegroundColor = upper;
                    Console.BackgroundColor = lower;
                    Console.Write('▀');
                }
            }
            Console.WriteLine();
        }
    }
}