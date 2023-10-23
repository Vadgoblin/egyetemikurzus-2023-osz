using E394KZ;
using E394KZ.Shapes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var canvas = new Canvas(1920, 1080);

        var lastSize = (Console.WindowWidth, Console.WindowHeight);
        DrawFrame();
        while (true)
        {

            DrawCanvas(canvas, 0, 0, Console.BackgroundColor);
            DrawPrompt();
            var input = Console.ReadLine() ?? "";

            if (input != "")
            {
                if (StartWidth(input.ToLower(), new string[] { "dot", "line", "circle", "rectangle", "triangle" }))
                {
                    ShapeParser(input).Draw(canvas);
                }

                ClearPrompt(input.Length);
            }
            else if (input == "" || lastSize != (Console.WindowWidth, Console.WindowHeight))
            {
                lastSize = (Console.WindowWidth, Console.WindowHeight);
                Console.Clear();
                DrawFrame();
            }
        }
    }
    static bool StartWidth(string text, string[] prefixArray)
    {
        foreach(var prefix in prefixArray)
        {
            if (text.StartsWith(prefix)) return true;
        }
        return false;
    }
    static BaseShape? ShapeParser(string text)
    {
        var textSplit = text.Split(' ');

        uint x;
        uint y;
        ConsoleColor color;
        string name;

        switch (textSplit[0].ToLower())
        {
            case "dot":
                if (textSplit.Length != 5 && textSplit.Length != 4) throw new InvalidArgumentumCountException("dot",textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                color = ConsoleColorParser(textSplit[3]);
                name = (textSplit.Length == 5) ? textSplit[4] : "";
                return new Dot(name,x,y,color);

            case "line":
                if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("line", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var x2 = Convert.ToUInt32(textSplit[3]);
                var y2 = Convert.ToUInt32(textSplit[4]);
                color = ConsoleColorParser(textSplit[5]);
                name = (textSplit.Length == 7) ? textSplit[6] : "";
                return new Line(name,x,y,color,x2,y2);

            case "rectangle":
                if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("rectangle", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var width = Convert.ToUInt32(textSplit[3]);
                var height = Convert.ToUInt32(textSplit[4]);
                color = ConsoleColorParser(textSplit[5]);
                name = (textSplit.Length == 7) ? textSplit[6] : "";
                return new Rectangle(name,x,y,width,height,color);

            case "circle":
                if (textSplit.Length != 6 && textSplit.Length != 5) throw new InvalidArgumentumCountException("circle", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var r = Convert.ToUInt32(textSplit[3]);
                color = ConsoleColorParser(textSplit[4]);
                name = (textSplit.Length == 6) ? textSplit[5] : "";
                return new Circle(name, x, y, color, r);

            case "triangle":
                if (textSplit.Length != 9 && textSplit.Length != 8) throw new InvalidArgumentumCountException("triangle", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var v2x = Convert.ToUInt32(textSplit[3]);
                var v2y = Convert.ToUInt32(textSplit[4]);
                var v3x = Convert.ToUInt32(textSplit[5]);
                var v3y = Convert.ToUInt32(textSplit[6]);
                color = ConsoleColorParser(textSplit[7]);
                name = (textSplit.Length == 9) ? textSplit[8] : "";
                return new Triangle(name, x, y, color, v2x, v2y, v3x, v3y);
            default:
                throw new Exception("this should never be called");
        }
    }
    static ConsoleColor ConsoleColorParser(string colorString)
    {
        if (Enum.TryParse<ConsoleColor>(colorString, true, out ConsoleColor color))
        {
            return color;
        }
        else
        {
            throw new InvalidConsoleColorException(colorString);
        }
    }


    static void DrawFrame()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
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
    static void ClearPrompt(int len)
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(2, Console.WindowHeight - 2);
        for(uint i = 0; i < len; i++) { Console.Write(" "); }
    }
}