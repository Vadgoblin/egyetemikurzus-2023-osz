using E394KZ;
using E394KZ.Shapes;

class Program
{
    const uint CANVAS_WIDTH = 1920;
    const uint CANVAS_HEIGHT = 1080;

    static void Main(string[] args)
    {
        var canvas = new Canvas(CANVAS_WIDTH, CANVAS_HEIGHT);
        canvas.Fill(ConsoleColor.DarkGray);

        var circle = new Circle("kor", 12, 12, ConsoleColor.Red, 8);
        circle.Draw(canvas);

        var dot = new Dot("pont", 10, 12, ConsoleColor.Blue);
        dot.Draw(canvas);

        PrintCanvas(canvas, 0, 0, Console.BackgroundColor);
        Console.ReadLine();
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