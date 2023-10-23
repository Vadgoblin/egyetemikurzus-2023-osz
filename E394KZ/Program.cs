using E394KZ;
using E394KZ.Shapes;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var canvas = new Canvas(1920, 1080);
        var shapeHistory = new List<BaseShape>();

        var lastSize = (Console.WindowWidth, Console.WindowHeight);
        DrawFrame();
        while (true)
        {
            DrawCanvas(canvas, 0, 0, Console.BackgroundColor);
            DrawLastShapes(shapeHistory);
            DrawPrompt();
            var input = Console.ReadLine() ?? "";

            if (input != "")
            {
                if (StartWidth(input.ToLower(), new string[] { "dot", "line", "circle", "rectangle", "triangle" }))
                {
                    var shape = ShapeParser(input);
                    canvas.Draw(shape);
                    shapeHistory.Add(shape);
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
    static BaseShape ShapeParser(string text)
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

        Console.SetCursorPosition(Console.WindowWidth-26, 0);
        Console.Write('┳');
        for(int i = 1;i<Console.WindowHeight - 3; i++)
        {
            Console.SetCursorPosition(Console.WindowWidth - 26, i);
            Console.Write('┃');
        }
        Console.SetCursorPosition(Console.WindowWidth - 26, Console.WindowHeight - 3);
        Console.Write('┻');
        Console.SetCursorPosition(Console.WindowWidth - 25, 1);
        Console.Write("Last shapes:");
        sb.Clear();
        sb.Append('┠');
        for (int i = 0; i < 24; i++) sb.Append('─');
        sb.Append('┨');
        Console.SetCursorPosition(Console.WindowWidth - 26, 2);
        Console.Write(sb.ToString());
    }
    static void DrawPrompt()
    {
        Console.CursorVisible = true;
        Console.SetCursorPosition(1, Console.WindowHeight - 2);
        Console.Write(">");
    }
    static void ClearPrompt(int len)
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(2, Console.WindowHeight - 2);
        for (uint i = 0; i < len; i++) { Console.Write(" "); }
    }
    static void DrawCanvas(Canvas canvas, uint woffset, uint hoffset, ConsoleColor backgdoundColor)
    {
        Console.CursorVisible = false;
        int line = 1;
        Console.SetCursorPosition(1, line++);

        for (uint hindex = hoffset; hindex < hoffset + (Console.WindowHeight - 4) * 2 && hindex < canvas.Height; hindex += 2)
        {
            for (uint windex = woffset; windex < woffset + Console.WindowWidth-30 && windex < canvas.Height; windex++)
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
    static void DrawLastShapes(List<BaseShape> shapeHistory)
    {
        for (int i = 0;i<Console.WindowHeight-6 && i < shapeHistory.Count;i++)
        {
            Console.SetCursorPosition(Console.WindowWidth - 25, 3+i);
            if (i == Console.WindowHeight - 7) Console.Write("...");
            else
            {
                var text = $"{shapeHistory[shapeHistory.Count - 1 - i].Name}";
                if (text.Length > 24) text = text.Substring(0, 24);
                else for (int j = text.Length; j < 24; j++) text += " ";
                Console.Write(text);
            }
        }
    }
}