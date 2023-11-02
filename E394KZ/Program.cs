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
        var shapeHistory = new ShapeHistory();

        while (true)
        {
            Console.Title = $"Offset: {GUI.Xoffset}x{GUI.Yoffset}, Canvas size: {canvas.Width}x{canvas.Height}";
            try
            {
                GUI.RedrawScreen(canvas, shapeHistory);
                var input = Console.ReadLine() ?? "";

                if (input != "")
                {
                    if (ShapeParser.IsStringStartingWidthShape(input.ToLower()))
                    {
                        var shape = ShapeParser.Parse(input, shapeHistory, canvas.Width, canvas.Height);
                        canvas.Draw(shape);
                        shapeHistory.Add(shape);
                    }
                    else if (input == "undo")
                    {
                        if (shapeHistory.Count > 0)
                        {
                            shapeHistory.RemoveLast();
                        }
                        else
                        {
                            GUI.DrawMsgbox("There is nothing to undo.", "Error!");
                        }
                    }
                    else if (input == "help")
                    {
                        GUI.ShowHelp();
                    }
                    else if (input == "clear")
                    {
                        canvas.Clear();
                        shapeHistory.Clear();
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

                            shapeHistory.Save(inputSplit[1]);
                        }
                    }
                    else if (input.StartsWith("load"))
                    {
                        var inputSplit = input.Split(' ');
                        if (inputSplit.Length != 2) throw new InvalidArgumentumCountException("load", inputSplit.Length);

                        shapeHistory.Load(inputSplit[1]);
                        canvas.Clear();
                        canvas.Draw(shapeHistory);

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
                    }
                    else if (input == "q" || input == "quit" || input == "exit") return;

                    else
                    {
                        GUI.DrawMsgbox($"Unknown command: \"{input}\"", "Input error");
                    }
                }
            }
            catch (WindowsTooSmallException)
            {
                Console.Clear();
                Console.WriteLine("Window is too small!");
                Console.WriteLine("It must be at least 11 character high and 50 character wide");
                while (GUI.IsWindowTooSmall()) Thread.Sleep(50);
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

}