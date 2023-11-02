using E394KZ;
using E394KZ.Exceptions;
using E394KZ.Shapes;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var canvas = new Canvas(1920, 1080);
        var shapeHistory = new List<BaseShape>();

        var lastSize = (Console.WindowWidth, Console.WindowHeight);
        var needFullRedraw = false;
        Console.Title = $"Offset: {GUI.Xoffset}x{GUI.Yoffset}, Canvas size: {canvas.Width}x{canvas.Height}";
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
                        var shape = ShapeParser(input, shapeHistory, canvas.Width, canvas.Height);
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
                    else if (input == "clear")
                    {
                        canvas.Clear();
                        shapeHistory.Clear();
                        needFullRedraw = true;
                    }
                    else if (input.StartsWith("stat"))
                    {
                        GUI.DrawTextbox(Stat.GetStat(shapeHistory, canvas.Width, canvas.Height), "Stat");
                    }
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
                        Console.Title = $"Offset: {GUI.Xoffset}x{GUI.Yoffset}, Canvas size: {canvas.Width}x{canvas.Height}";
                    }
                    else if (input == "ba")
                    {
                        BA.Start();
                        Console.Title = $"Offset: {GUI.Xoffset}x{GUI.Yoffset}, Canvas size: {canvas.Width}x{canvas.Height}";
                        needFullRedraw = true;
                    }
                    else if (input == "q" || input == "quit" || input == "exit") return;

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
            catch (CoordinateOutOfCanvas)
            {
                GUI.DrawMsgbox("Shape's all point must be within the canvas.", "CoordinateOutOfCanvas");
            }
            catch (LoadException ex)
            {
                GUI.DrawMsgbox(ex.Message, "LoadException");
            }
            catch (InvalidCharacterInNameException ex)
            {
                GUI.DrawMsgbox(ex.Message, "InvalidCharacterInNameException");
            }
        }
    }
    static bool IsStringStartingWidthShape(string text)
    {
        var prefixArray = new string[] { "dot ", "line ", "circle ", "rectangle ", "triangle " };
        foreach (var prefix in prefixArray)
        {
            if (text.StartsWith(prefix)) return true;
        }
        return false;
    }
    static bool ContainsInvalidCharacter(string text)
    {
        string validCharsPattern = @"^[a-zA-Z0-9_.-]+$";
        return !Regex.IsMatch(text, validCharsPattern);
    }
    static BaseShape ShapeParser(string text, List<BaseShape> shapeHistory, uint canvasWidth, uint canvasHeight)
    {
        static string NameChecker(string name)
        {
            if (IsStringStartingWidthShape(name.ToLower()))
            {
                var tmp = name.Replace("dot", "").Replace("line", "").Replace("circle", "").Replace("rectangle", "").Replace("triangle", "");
                if (tmp.Length > 0 && char.IsDigit(tmp[0])) throw new InvalidNameException(name);
            }
            if (ContainsInvalidCharacter(name)) throw new InvalidCharacterInNameException("shape nickname");
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
        static uint StrToUint(string str,uint upperBound)
        {
            var isSuccesful = UInt32.TryParse(str, out var result);
            if (!isSuccesful || result >= upperBound) throw new CoordinateOutOfCanvas();
            return result;
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
                x = StrToUint(textSplit[1], canvasWidth);
                y = StrToUint(textSplit[2], canvasWidth);
                color = ConsoleColorParser(textSplit[3]);
                name = (textSplit.Length == 5) ? NameChecker(textSplit[4]) : GetAutoName(shapeHistory, "dot");
                return new Dot(name, x, y, color);

            case "line":
                if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("line", textSplit.Length);
                x = StrToUint(textSplit[1], canvasWidth);
                y = StrToUint(textSplit[2], canvasHeight);
                var x2 = StrToUint(textSplit[3], canvasWidth);
                var y2 = StrToUint(textSplit[4], canvasHeight);
                color = ConsoleColorParser(textSplit[5]);
                name = (textSplit.Length == 7) ? NameChecker(textSplit[6]) : GetAutoName(shapeHistory, "line");
                return new Line(name, x, y, x2, y2, color);

            case "rectangle":
                if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("rectangle", textSplit.Length);
                x = StrToUint(textSplit[1], canvasWidth);
                y = StrToUint(textSplit[2], canvasHeight);
                var width = StrToUint(textSplit[3], canvasWidth - x);
                var height = StrToUint(textSplit[4], canvasHeight - y);
                color = ConsoleColorParser(textSplit[5]);
                name = (textSplit.Length == 7) ? NameChecker(textSplit[6]) : GetAutoName(shapeHistory, "rectangle");
                return new Rectangle(name, x, y, width, height, color);

            case "circle":
                if (textSplit.Length != 6 && textSplit.Length != 5) throw new InvalidArgumentumCountException("circle", textSplit.Length);
                x = StrToUint(textSplit[1], canvasWidth);
                y = StrToUint(textSplit[2], canvasHeight);
                var r = Convert.ToUInt32(textSplit[3]);
                color = ConsoleColorParser(textSplit[4]);
                name = (textSplit.Length == 6) ? NameChecker(textSplit[5]) : GetAutoName(shapeHistory, "circle");
                return new Circle(name, x, y, color, r);

            case "triangle":
                if (textSplit.Length != 9 && textSplit.Length != 8) throw new InvalidArgumentumCountException("triangle", textSplit.Length);
                x = StrToUint(textSplit[1], canvasWidth);
                y = StrToUint(textSplit[2], canvasHeight);
                var v2x = StrToUint(textSplit[3], canvasWidth);
                var v2y = StrToUint(textSplit[4], canvasHeight);
                var v3x = StrToUint(textSplit[5], canvasWidth);
                var v3y = StrToUint(textSplit[6], canvasHeight);
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
        if (ContainsInvalidCharacter(saveName)) throw new InvalidCharacterInNameException("save name");
        try
        {
            if (!Directory.Exists("saves/")) Directory.CreateDirectory("saves/");

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText($"saves/{saveName}.json", JsonSerializer.Serialize(shapeHistory, jsonOptions));

            GUI.DrawMsgbox("Save succesfull.", "Save",false);
        }
        catch//io problem maybe? idk
        {
            GUI.DrawMsgbox("Save failed.", "Save error");
        }
    }
    static List<BaseShape> Load(string saveName)
    {
        if (ContainsInvalidCharacter(saveName)) throw new InvalidCharacterInNameException("load name");
        if (!File.Exists($"saves/{saveName}.json")) throw new LoadException($"There is no save named \"{saveName}\".");
        else
        {
            var jsonTExt = File.ReadAllText($"saves/{saveName}.json");

            var loadedShapeHistory = JsonSerializer.Deserialize<List<BaseShape>>(jsonTExt);

            return loadedShapeHistory ?? throw new Exception();
        }
    }
}