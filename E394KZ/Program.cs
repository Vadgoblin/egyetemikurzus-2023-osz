using E394KZ;
using E394KZ.Shapes;

class Program
{
    static void Main()
    {
        var canvas = new Canvas(1920, 1080);
        canvas.Fill(ConsoleColor.DarkGray);

        var circle = new Circle("kor", 12, 12, ConsoleColor.Red, 8);
        canvas.Draw(circle);

        var dot = new Dot("pont", 10, 12, ConsoleColor.Blue);
        canvas.Draw(dot);

        var rectangle = new Rectangle("rect", 10, 13, ConsoleColor.Yellow, 6, 5);
        canvas.Draw(rectangle);

        PrintCanvas(canvas, 0, 0, Console.BackgroundColor);
        Console.ReadLine();
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