using E394KZ.Shapes;
using System.Text;

namespace E394KZ
{
    internal class GUI
    {
        public static uint HorizontalOffset { get; private set; } = 0 ;
        public static uint VerticalOffset { get; private set; } = 0 ;
        public static ConsoleColor BackgroundColor { get; private set; } = ConsoleColor.Black;
        public static void RedrawScreen(Canvas canvas, List<BaseShape> shapeHistory)
        {
            Console.Clear();
            DrawFrame();
            DrawLastShapes(shapeHistory);
            DrawCanvas(canvas);
            DrawPrompt();
        }
        public static void DrawFrame()
        {
            if (IsWindowTooSmall()) throw new WindowsTooSmallException();
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
        public static void DrawPrompt()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, Console.WindowHeight - 2);

            var sb = new StringBuilder();
            for (uint i = 0; i < Console.WindowWidth - 3; i++) sb.Append(" ");

            Console.Write(">" + sb.ToString());

            Console.SetCursorPosition(2, Console.WindowHeight - 2);
            Console.CursorVisible = true;
        }
        public static void DrawCanvas(Canvas canvas)
        {
            Console.CursorVisible = false;
            int line = 1;
            Console.SetCursorPosition(1, line++);

            for (uint hindex = HorizontalOffset; hindex < HorizontalOffset + (Console.WindowHeight - 4) * 2 && hindex < canvas.Height; hindex += 2)
            {
                for (uint windex = VerticalOffset; windex < VerticalOffset + Console.WindowWidth - 27 && windex < canvas.Height; windex++)
                {
                    var upper = canvas[windex, hindex] ?? BackgroundColor;
                    var lower = canvas[windex, hindex + 1] ?? BackgroundColor;

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
        public static void DrawLastShapes(List<BaseShape> shapeHistory)
        {
            for (int i = 0; i < Console.WindowHeight - 6 && i < shapeHistory.Count; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 25, 3 + i);
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
        public static void ClearLastShapes()
        {
            for (int i = 0; i < Console.WindowHeight - 6; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 25, 3 + i);
                var text = "";
                for (int j = text.Length; j < 24; j++) text += " ";
                Console.Write(text);
            }
        }
        public static void DrawErrorbox(string msg, string title)
        {
            if (IsWindowTooSmall()) throw new WindowsTooSmallException();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;

            if (Console.WindowWidth - 27 < msg.Length + 4)
            {
                msg = msg.Substring(0, Console.WindowWidth - 31);
            }
            if (Console.WindowWidth - 27 < title.Length + 4)
            {
                title = title.Substring(0, Console.WindowWidth - 31);
            }

            var width = Math.Max(msg.Length, title.Length + 2) + 2;
            var height = 7;

            var x = (Console.WindowWidth - 25) / 2 - width / 2;
            var y = (Console.WindowHeight - 4) / 2 - height / 2;

            var sb = new StringBuilder();
            sb.Append("╭");
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append("─");
            }
            sb.Append('╮');
            Console.SetCursorPosition(x, y);
            Console.Write(sb.ToString());
            sb.Clear();

            sb.Append("│");
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append(" ");
            }
            sb.Append('│');
            var vertical = sb.ToString();
            sb.Clear();

            Console.SetCursorPosition(x, y + 1);
            Console.Write(vertical);

            sb.Append("├");
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append("─");
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

            sb.Append("╰");
            for (int i = 0; i < width - 2; i++)
            {
                sb.Append("─");
            }
            sb.Append('╯');
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write(sb.ToString());

            int xoffset = (width - 2) / 2 - (title.Length) / 2;
            Console.SetCursorPosition(x + 1 + xoffset, y + 1);
            Console.Write(title);

            if (msg.Length > width - 2)
            {

            }
            else
            {
                xoffset = ((width - 1) - msg.Length) / 2;
                Console.SetCursorPosition(x + 1 + xoffset, y + 4);
                Console.Write(msg);
            }

            Console.ResetColor();
        }
        public static bool IsWindowTooSmall()
        {
            return (Console.WindowHeight < 11 || Console.WindowWidth < 50);
        }
    }
}
