using E394KZ;
using E394KZ.Shapes;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var canvas = new Canvas(1920, 1080);

        var circle = new Circle("kor", 12, 12, ConsoleColor.Red, 8);
        canvas.Draw(circle);

        var dot = new Dot("pont", 10, 12, ConsoleColor.Blue);
        canvas.Draw(dot);

        var rectangle = new Rectangle("rect", 10, 13, 6, 5, ConsoleColor.Yellow);
        canvas.Draw(rectangle);

        var line = new Line("lineasd", 0, 0, 7, 14, ConsoleColor.Green);
        line.Draw(canvas);

        var triangle = new Triangle("triangle", 10, 0, 0, 10, 25, 14, ConsoleColor.White);
        triangle.Draw(canvas);

        DrawFrame();
        DrawCanvas(canvas, 0, 0, Console.BackgroundColor);
        DrawPrompt();
        Console.ReadLine();
    }

    static void DrawFrame()
    {
        Console.CursorVisible = false;
        var sb = new StringBuilder();
        sb.Append("┏");
        for (int i = 0; i < Console.WindowWidth - 2; i++)
        {
            sb.Append('━');
        }
        sb.Append('┓');
        Console.WriteLine(sb);
        sb.Clear();

        for (int i = 1; i < Console.WindowHeight - 1; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write('┃');
            Console.SetCursorPosition(Console.WindowWidth - 1, i);
            Console.Write('┃');
        }

        sb.Append("┗");
        for (int i = 0; i < Console.WindowWidth - 2; i++)
        {
            sb.Append('━');
        }
        sb.Append('┛');
        Console.Write(sb);
        sb.Clear();


        Console.SetCursorPosition(0, Console.WindowHeight - 3);
        sb.Append("┣");
        for (int i = 0; i < Console.WindowWidth - 2; i++)
        {
            sb.Append('━');
        }
        sb.Append('┫');
        Console.Write(sb);
    }
    static void DrawPrompt()
    {
        Console.CursorVisible = true;
        Console.SetCursorPosition(1, Console.WindowHeight - 2);
        Console.Write(">");
    }
    static void DrawCanvas(Canvas canvas, uint woffset, uint hoffset, ConsoleColor backgdoundColor)
    {
        Console.CursorVisible = false;
        int line = 1;
        Console.SetCursorPosition(1, line++);

        for (uint hindex = hoffset; hindex < hoffset + (Console.WindowHeight - 4) * 2 && hindex < canvas.Height; hindex += 2)
        {
            for (uint windex = woffset; windex < woffset + Console.WindowWidth-2 && windex < canvas.Height; windex++)
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
            Console.ResetColor();
            Console.SetCursorPosition(1, line++);
        }
    }
}