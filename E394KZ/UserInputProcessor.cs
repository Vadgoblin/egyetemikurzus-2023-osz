using E394KZ.Exceptions;
using E394KZ.Shapes;

namespace E394KZ
{
    static internal class UserInputProcessor
    {
        public static void Process(string input, Canvas canvas, ShapeHistory shapeHistory)
        {
            if (ShapeParser.IsStringStartingWidthShape(input.ToLower())) ParseShape(input, shapeHistory, canvas);

            else if (input == "undo") Undo(shapeHistory, canvas);
            else if (input == "help") Help();
            else if (input == "clear") Clear(shapeHistory, canvas);
            else if (input == "stat") Stat(shapeHistory, canvas);

            else if (input.StartsWith("save")) Save(input, shapeHistory);
            else if (input.StartsWith("load")) Load(input, shapeHistory, canvas);
            else if (input.StartsWith("offset")) Offset(input, canvas);

            else if (input == "q" || input == "quit" || input == "exit") Environment.Exit(0);

            else GUI.DrawMsgbox($"Unknown command: \"{input}\"", "Input error");
        }

        private static void ParseShape(string input, ShapeHistory shapeHistory, Canvas canvas)
        {
            var shape = ShapeParser.Parse(input, shapeHistory, canvas.Width, canvas.Height);
            canvas.Draw(shape);
            shapeHistory.Add(shape);
        }
        private static void Undo(ShapeHistory shapeHistory, Canvas canvas)
        {
            if (shapeHistory.Count > 0)
            {
                shapeHistory.RemoveLast();
                canvas.Clear();
                canvas.Draw(shapeHistory);
            }
            else
            {
                GUI.DrawMsgbox("There is nothing to undo.", "Error!");
            }
        }
        private static void Help()
        {
            GUI.ShowHelp();
        }
        private static void Clear(ShapeHistory shapeHistory, Canvas canvas)
        {
            canvas.Clear();
            shapeHistory.Clear();
        }
        private static void Stat(ShapeHistory shapeHistory, Canvas canvas)
        {
            GUI.DrawTextbox(E394KZ.Stat.GetStat(shapeHistory, canvas.Width, canvas.Height), "Stat");
        }
        private static void Save(string input, ShapeHistory shapeHistory)
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
        private static void Load(string input,ShapeHistory shapeHistory , Canvas canvas)
        {
            var inputSplit = input.Split(' ');
            if (inputSplit.Length != 2) throw new InvalidArgumentumCountException("load", inputSplit.Length);

            shapeHistory.Load(inputSplit[1]);
            canvas.Clear();
            canvas.Draw(shapeHistory);
        }
        private static void Offset(string input, Canvas canvas)
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
    }
}
