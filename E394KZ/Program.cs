using E394KZ;
using E394KZ.Shapes;

class Program
{
    const uint CANVAS_WIDTH = 1920;
    const uint CANVAS_HEIGHT = 1080;

    static void Main(string[] args)
    {
        var canvas = new Canvas(CANVAS_WIDTH, CANVAS_HEIGHT);

        var dot = new Dot("pont",10,12,ConsoleColor.Blue);
        dot.Draw(canvas);

        //var defaultbackgdoundcolor = Console.BackgroundColor;
        //Fill(canvas, ConsoleColor.DarkGray);
        DrawCircle(canvas, 4, 4, 10, ConsoleColor.Red);
        //canvas[2, 3] = ConsoleColor.White;
        //shape.Draw(canvas);


        PrintCanvas(canvas, 0, 0, Console.BackgroundColor);
        Console.ReadLine();
    }

    static void DrawCircle(Canvas canvas, int centerX, int centerY, int radius, ConsoleColor color)
    {
        for (uint y = 0; y < canvas.Height; y++)
        {
            for (uint x = 0; x < canvas.Width; x++)
            {
                double distance = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));
                if (distance < radius)
                {
                    canvas[x, y] = color;
                }
            }
        }
    }

    static void Fill(ConsoleColor?[,] canvas, ConsoleColor color)
    {
        for (int i = 0; i < canvas.GetLength(0); i++)
        {
            for (int j = 0; j < canvas.GetLength(1); j++)
            {
                canvas[i, j] = color;
            }
        }
    }

    static void PrintCanvas(Canvas canvas, uint woffset, uint hoffset, ConsoleColor backgdoundColor)
    {
        for (uint hindex = hoffset; hindex < hoffset + (Console.WindowHeight - 1) * 2 && hindex < canvas.Height; hindex += 2)
        {
            for (uint windex = woffset; windex < woffset + Console.WindowWidth && windex < canvas.Height; windex++)
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