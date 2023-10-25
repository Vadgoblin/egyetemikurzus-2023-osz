using E394KZ;
using E394KZ.Shapes;
using System.Data;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var canvas = new Canvas(1920, 1080);
        var shapeHistory = new List<BaseShape>();

        var lastSize = (Console.WindowWidth, Console.WindowHeight);
        var needFullRedraw = false;
        GUI.DrawFrame();
        while (true)
        {
            try
            {
                if (needFullRedraw)
                {
                    GUI.RedrawScreen(canvas, shapeHistory);
                }
                else
                {
                    GUI.DrawCanvas(canvas);
                    GUI.DrawLastShapes(shapeHistory);
                    GUI.DrawPrompt();
                }

                var input = Console.ReadLine() ?? "";

                if (input != "")
                {
                    if (IsStringStartingWidthShape(input.ToLower()))
                    {
                        var shape = ShapeParser(input, shapeHistory);
                        canvas.Draw(shape);
                        shapeHistory.Add(shape);
                    }
                    else if (input == "undo")
                    {
                        if (shapeHistory.Count > 0)
                        {
                            shapeHistory.RemoveAt(shapeHistory.Count - 1);
                            canvas.Clear();
                            canvas.Draw(shapeHistory);
                            needFullRedraw = true;
                        }
                        else
                        {
                            GUI.DrawMsgbox("There is nothing to undo.", "Error!");
                        }
                    }
                    else if (input == "help")
                    {
                        GUI.ShowHelp();
                        needFullRedraw = true;
                    }
                    else if (input.StartsWith("stat")) ;
                    else if (input.StartsWith("save"))
                    {
                        if (shapeHistory.Count == 0)
                        {
                            GUI.DrawMsgbox("There is nothing to save.", "Error?");
                        }
                        else
                        {
                            var inputSplit = input.Split(' ');
                            if (inputSplit.Length != 2) throw new InvalidArgumentumCountException("save", inputSplit.Length);

                            Save(shapeHistory, inputSplit[1]);
                        }
                    }
                    else if (input.StartsWith("load"))
                    {
                        var inputSplit = input.Split(' ');
                        if (inputSplit.Length != 2) throw new InvalidArgumentumCountException("load", inputSplit.Length);

                        shapeHistory = Load(inputSplit[1]);
                        canvas.Clear();
                        canvas.Draw(shapeHistory);
                        needFullRedraw = true;

                    }
                    else if (input.StartsWith("offset"))
                    {
                        var inputSplit = input.Split(' ');
                        if (inputSplit.Length != 3) throw new InvalidArgumentumCountException("offset", inputSplit.Length);

                        var x = Convert.ToUInt32(inputSplit[1]);
                        var y = Convert.ToUInt32(inputSplit[2]);

                        if (canvas.Width <= Console.WindowWidth - 27) x = 0;
                        else if (canvas.Width < x + Console.WindowWidth - 27) x = (uint)canvas.Width - ((uint)Console.WindowWidth - 27);

                        if (canvas.Height <= Console.WindowHeight - 4) y = 0;
                        else if (canvas.Height < y + Console.WindowHeight - 4) y = (uint)canvas.Height - ((uint)Console.WindowHeight - 4);

                        GUI.ChangeOffset(x, y);
                    }


                    else
                    {
                        GUI.DrawMsgbox($"Unknown command: \"{input}\"", "Input error");
                    }

                    if (input.Length >= Console.WindowWidth - 3) needFullRedraw = true;
                }
                else if (input == "" || lastSize != (Console.WindowWidth, Console.WindowHeight))
                {
                    lastSize = (Console.WindowWidth, Console.WindowHeight);
                    needFullRedraw = true;
                }
            }
            catch (WindowsTooSmallException)
            {
                Console.Clear();
                Console.WriteLine("Window is too small!");
                Console.WriteLine("It must be at least 11 character high and 50 character wide");
                while (GUI.IsWindowTooSmall()) Thread.Sleep(50);
                needFullRedraw = true;
            }
            catch (ShapeException ex)
            {
                GUI.DrawMsgbox(ex.Message, "InvalidArgumentumCountException");
            }
        }
    }
    static bool IsStringStartingWidthShape(string text)
    {
        var prefixArray = new string[] { "dot", "line", "circle", "rectangle", "triangle" };
        foreach (var prefix in prefixArray)
        {
            if (text.StartsWith(prefix)) return true;
        }
        return false;
    }
    static BaseShape ShapeParser(string text, List<BaseShape> shapeHistory)
    {
        static string NameChecker(string name)
        {
            if (IsStringStartingWidthShape(name.ToLower()))
            {
                var tmp = name.Replace("dot", "").Replace("line", "").Replace("circle", "").Replace("rectangle", "").Replace("triangle", "");
                if (tmp.Length > 0 && char.IsDigit(tmp[0])) throw new InvalidNameException(name);
            }
            return name;
        }
        static string GetAutoName(List<BaseShape> shapeHistory, string shapename)
        {
            int countOfThatTypeOfShape = shapeHistory.Count(shape => shape.GetShapeName() == shapename);
            var name = new String(
                new[] { char.ToUpper(shapename.First()) }
                .Concat(shapename.Skip(1))
                .ToArray()
            );
            return $"{name}{countOfThatTypeOfShape + 1}";
        }

        var textSplit = text.Split(' ');

        uint x;
        uint y;
        ConsoleColor color;
        string name;

        switch (textSplit[0].ToLower())
        {
            case "dot":
                if (textSplit.Length != 5 && textSplit.Length != 4) throw new InvalidArgumentumCountException("dot", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                color = ConsoleColorParser(textSplit[3]);
                name = (textSplit.Length == 5) ? NameChecker(textSplit[4]) : GetAutoName(shapeHistory, "dot");
                return new Dot(name, x, y, color);

            case "line":
                if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("line", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var x2 = Convert.ToUInt32(textSplit[3]);
                var y2 = Convert.ToUInt32(textSplit[4]);
                color = ConsoleColorParser(textSplit[5]);
                name = (textSplit.Length == 7) ? NameChecker(textSplit[6]) : GetAutoName(shapeHistory, "line");
                return new Line(name, x, y, x2, y2, color);

            case "rectangle":
                if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("rectangle", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var width = Convert.ToUInt32(textSplit[3]);
                var height = Convert.ToUInt32(textSplit[4]);
                color = ConsoleColorParser(textSplit[5]);
                name = (textSplit.Length == 7) ? NameChecker(textSplit[6]) : GetAutoName(shapeHistory, "rectangle");
                return new Rectangle(name, x, y, width, height, color);

            case "circle":
                if (textSplit.Length != 6 && textSplit.Length != 5) throw new InvalidArgumentumCountException("circle", textSplit.Length);
                x = Convert.ToUInt32(textSplit[1]);
                y = Convert.ToUInt32(textSplit[2]);
                var r = Convert.ToUInt32(textSplit[3]);
                color = ConsoleColorParser(textSplit[4]);
                name = (textSplit.Length == 6) ? NameChecker(textSplit[5]) : GetAutoName(shapeHistory, "circle");
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
                name = (textSplit.Length == 9) ? NameChecker(textSplit[8]) : GetAutoName(shapeHistory, "triangle");
                return new Triangle(name, x, y, v2x, v2y, v3x, v3y, color);
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
            throw new InvalidColorException(colorString);
        }
    }

    static void Save(List<BaseShape> shapeHistory, string saveName)
    {
        try
        {
            if (!Directory.Exists("saves/")) Directory.CreateDirectory("saves/");

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText($"saves/{saveName}.json", JsonSerializer.Serialize(shapeHistory, jsonOptions));

            GUI.DrawMsgbox("Save succesfull.", "Save",false);
        }
        catch(Exception ex)//io problem maybe? idk
        {
            GUI.DrawMsgbox("Save failed.", "Save");
        }
    }
    static List<BaseShape> Load(string saveName)
    {
        try
        {
            if (!File.Exists($"saves/{saveName}.json")) throw new LoadException($"There is no save named {saveName}.");
            else
            {
                var jsonTExt = File.ReadAllText($"saves/{saveName}.json");

                var loadedShapeHistory = JsonSerializer.Deserialize< List<BaseShape>> (jsonTExt);

                return loadedShapeHistory == null ? throw new Exception() : (List<BaseShape>)loadedShapeHistory;
            }
        }
        catch(Exception e)
        {
            throw new Exception();
        }
    }
}