using System.Text;

namespace E394KZ
{
    internal static class GUI
    {
        private static int lastWindowWidth = -1;
        private static int lastWindowHeight = -1;

        public static uint Xoffset { get; private set; } = 0 ;
        public static uint Yoffset { get; private set; } = 0 ;
        public static ConsoleColor BackgroundColor { get; private set; } = ConsoleColor.Black;
        public static void ChangeOffset(uint x_offset, uint y_offset)
        {
            Xoffset = x_offset;
            Yoffset = y_offset;
        }
        public static void RedrawScreen(Canvas canvas, ShapeHistory shapeHistory)
        {
            if(lastWindowWidth != Console.WindowWidth || lastWindowHeight != Console.WindowHeight)
            {
                lastWindowWidth = Console.WindowWidth;
                lastWindowHeight = Console.WindowHeight;
                Console.Clear();
            }
            DrawFrame();
            DrawLastShapes(shapeHistory);
            DrawCanvas(canvas);
            DrawPrompt();
        }
        private static void DrawFrame()
        {
            if (IsWindowTooSmall()) throw new Exceptions.WindowsTooSmallException();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            var sb = new StringBuilder();
            sb.Append('┏');
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

            sb.Append('┗');
            for (int i = 0; i < Console.WindowWidth - 2; i++)
            {
                sb.Append('━');
            }
            sb.Append('┛');
            Console.Write(sb);
            sb.Clear();


            Console.SetCursorPosition(0, Console.WindowHeight - 3);
            sb.Append('┣');
            for (int i = 0; i < Console.WindowWidth - 2; i++)
            {
                sb.Append('━');
            }
            sb.Append('┫');
            Console.Write(sb);

            Console.SetCursorPosition(Console.WindowWidth - 26, 0);
            Console.Write('┳');
            for (int i = 1; i < Console.WindowHeight - 3; i++)
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
        private static void DrawPrompt()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, Console.WindowHeight - 2);

            var sb = new StringBuilder();
            for (uint i = 0; i < Console.WindowWidth - 3; i++) sb.Append(' ');

            Console.Write(">" + sb.ToString());

            Console.SetCursorPosition(2, Console.WindowHeight - 2);
            Console.CursorVisible = true;
        }
        private static void DrawCanvas(Canvas canvas)
        {
            Console.CursorVisible = false;
            int line = 1;
            
            for(uint y = 0 ; y < Console.WindowHeight - 4; y++)
            {
                Console.SetCursorPosition(1, line++);
                for (uint x = 0; x < Console.WindowWidth - 27; x++)
                {
                    ConsoleColor upper = BackgroundColor;
                    ConsoleColor lower = BackgroundColor;

                    if (Xoffset + x < canvas.Width)
                    {
                        if(Yoffset + y * 2 + 1 < canvas.Width) lower = canvas[Xoffset + x, Yoffset + y * 2 + 1] ?? BackgroundColor;
                        if (Yoffset + y * 2 < canvas.Width) upper = canvas[Xoffset + x, Yoffset + y * 2] ?? BackgroundColor;
                    }

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

                    Console.ResetColor();
                }
            }
        }
        private static void DrawLastShapes(ShapeHistory shapeHistory)
        {
            for (int i = 0; i < Console.WindowHeight - 6; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 25, 3 + i);
                if (i < shapeHistory.Count)
                {
                    if (i == Console.WindowHeight - 7) Console.Write("...");
                    else
                    {
                        var text = $"{shapeHistory[shapeHistory.Count - 1 - i].Name}";
                        if (text.Length > 24) text = text[..24];
                        else for (int j = text.Length; j < 24; j++) text += " ";
                        Console.Write(text);
                    }
                }
                else
                {
                    Console.Write("                        ");
                }
            }
        }
        public static void DrawMsgbox(string messange, string title,bool error = true)
        {
            if (IsWindowTooSmall()) throw new Exceptions.WindowsTooSmallException();
            Console.CursorVisible = false;
            if(error) Console.ForegroundColor = ConsoleColor.Red;

            if (Console.WindowWidth - 27 < messange.Length + 4)
            {
                messange = messange[..(Console.WindowWidth - 31)];
            }
            if (Console.WindowWidth - 27 < title.Length + 4)
            {
                title = title[..(Console.WindowWidth - 31)];
            }

            var width = Math.Max(messange.Length, title.Length + 2) + 2;
            var height = 7;

            var x = (Console.WindowWidth - 25) / 2 - width / 2;
            var y = (Console.WindowHeight - 4) / 2 - height / 2;

            var sb = new StringBuilder();
            sb.Append('╭');
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append('─');
            }
            sb.Append('╮');
            Console.SetCursorPosition(x, y);
            Console.Write(sb.ToString());
            sb.Clear();

            sb.Append('│');
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append(' ');
            }
            sb.Append('│');
            var vertical = sb.ToString();
            sb.Clear();

            Console.SetCursorPosition(x, y + 1);
            Console.Write(vertical);

            sb.Append('├');
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append('─');
            }
            sb.Append('┤');
            Console.SetCursorPosition(x, y + 2);
            Console.Write(sb.ToString());
            sb.Clear();

            for (int i = 3; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(vertical);
            }

            sb.Append('╰');
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append('─');
            }
            sb.Append('╯');
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write(sb.ToString());

            int xoffset = (width - 2) / 2 - (title.Length) / 2;
            Console.SetCursorPosition(x + 1 + xoffset, y + 1);
            Console.Write(title);

            xoffset = ((width - 1) - messange.Length) / 2;
            Console.SetCursorPosition(x + 1 + xoffset, y + 4);
            Console.Write(messange);

            Console.ResetColor();
            Console.ReadKey();
        }
        public static void DrawTextbox(string[] messange, string title)
        {
            Console.CursorVisible = false;
            var textboxWidth = messange.Max(str => str.Length)+4;
            var textboxHeight = messange.Length + 4;
            
            if (textboxWidth > Console.WindowWidth - 27 || textboxHeight > Console.WindowHeight - 4)
            {
                Console.Clear();
                foreach (var line in messange)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
               

                var x = (Console.WindowWidth - 25 - textboxWidth) / 2;
                var y = (Console.WindowHeight - 2 - textboxHeight) / 2;

                var sb = new StringBuilder();
                sb.Append('╭');
                for (int i = 0; i < textboxWidth - 2; i++)
                {
                    sb.Append('─');
                }
                sb.Append('╮');
                Console.SetCursorPosition(x, y);
                Console.Write(sb.ToString());
                sb.Clear();

                sb.Append('│');
                for (int i = 0; i < textboxWidth - 2; i++)
                {
                    sb.Append(' ');
                }
                sb.Append('│');
                var vertical = sb.ToString();
                sb.Clear();

                Console.SetCursorPosition(x, y + 1);
                Console.Write(vertical);

                sb.Append('├');
                for (int i = 0; i < textboxWidth - 2; i++)
                {
                    sb.Append('─');
                }
                sb.Append('┤');
                Console.SetCursorPosition(x, y + 2);
                Console.Write(sb.ToString());
                sb.Clear();

                for (int i = 3; i < textboxHeight - 1; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    Console.Write(vertical);
                }

                sb.Append('╰');
                for (int i = 0; i < textboxWidth - 2; i++)
                {
                    sb.Append('─');
                }
                sb.Append('╯');
                Console.SetCursorPosition(x, y + textboxHeight - 1);
                Console.Write(sb.ToString());

                int xoffset = (textboxWidth - 2) / 2 - (title.Length) / 2;
                Console.SetCursorPosition(x + 1 + xoffset, y + 1);
                Console.Write(title);

                for (int i = 0; i < messange.Length; i++)
                {
                    Console.SetCursorPosition(x + 2, y + 3 + i);
                    Console.Write(messange[i]);
                }

                Console.ResetColor();
            }

            Console.ReadKey();
        }
        public static bool IsWindowTooSmall()
        {
            return (Console.WindowHeight < 11 || Console.WindowWidth < 50);
        }

        public static void ShowHelp()
        {
            var helpText = new string[]
            {
                "Új alakzat lézrehozása: (név elhagyható)",
                "    pont: dot {x} {y} {color} {name}",
                "    vonal: line {start_x} {start_y} {end_x} {end_y} {color} {name}",
                "    téglalap: rectangle {x} {y} {width} {height} {color} {name}",
                "    kör: circle {x} {y} {radius} {color} {name}",
                "    háromszög: triangle {x1} {y1} {x2} {y2} {x3} {y3} {color} {name}",
                "",
                "Támogatott színek:",
                "    Black DarkBlue DarkGreen DarkCyan DarkRed DarkMagenta DarkYellow",
                "    DarkGray Gray Blue Green Cyan Red Magenta Yellow White",
                "",
                "További parancsok:",
                "    help: segítség megjelenítése",
                "    clear: letörli a vászont",
                "    stat: statisztika megjelenítése",
                "    save {name}: alakzatok mentése {name} néven",
                "    load {name}: {name} néven mentett alakzatok betöltése",
                "    offset {x} {y}: megjelenítési eltolás megváltoztatása"
            };

            DrawTextbox(helpText, "Help");
        }
    }
}
