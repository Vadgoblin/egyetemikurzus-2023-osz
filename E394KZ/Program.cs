class Program
{
    const uint CANVAS_WIDTH = 1920;
    const uint CANVAS_HEIGHT = 1080;

    required ConsoleColor BACKGROUND_COLOR;

    static void Main(string[] args)
    {
        var canvas = new ConsoleColor?[CANVAS_WIDTH, CANVAS_HEIGHT];
        var defaultbackgdoundcolor = Console.BackgroundColor;
        Fill(canvas, ConsoleColor.DarkGray);
        DrawCircle(canvas, 4, 4, 4, ConsoleColor.Red);
        canvas[2, 3] = ConsoleColor.White;

        PrintCanvas(canvas, 1, 1, defaultbackgdoundcolor);

        Console.ReadLine();
    }

    static void DrawCircle(ConsoleColor?[,] canvas,int centerX, int centerY, int radius, ConsoleColor color)
    {
        for (int y = 0; y < canvas.GetLength(1); y++)
        {
            for (int x = 0; x < canvas.GetLength(0); x++)
            {
                double distance = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));
                if (distance < radius)
                {
                    canvas[x, y] = color;
                }
            }
        }
    }

    static void Fill(ConsoleColor?[,] canvas,ConsoleColor color) 
    {
        for(int i = 0; i <  canvas.GetLength(0); i++)
        {
            for(int j = 0; j < canvas.GetLength(1); j++)
            {
                canvas[i,j] = color;
            }
        }
    }

    static void PrintCanvas(ConsoleColor?[,] canvas, uint woffset, uint hoffset, ConsoleColor backgdoundColor)
    {
        for(uint hindex = hoffset; hindex < hoffset + (Console.WindowHeight - 1) *2 && hindex < canvas.GetLength(1); hindex+=2)
        {
            for(uint windex = woffset; windex < woffset + Console.WindowWidth && windex < canvas.GetLength(0); windex++)
            {
                var upper = canvas[windex, hindex] ?? backgdoundColor;
                var lower = canvas[windex, hindex + 1] ?? backgdoundColor;

                if (upper == lower)
                {
                    Console.BackgroundColor = upper;
                    Console.Write(' ');
                }
                else
                {
                    Console.ForegroundColor = upper;
                    Console.BackgroundColor = lower;
                    Console.Write('▀');
                }
            }
            Console.BackgroundColor = backgdoundColor;
            Console.WriteLine();
        }
    }
}