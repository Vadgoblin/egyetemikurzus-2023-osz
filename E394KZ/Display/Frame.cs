namespace E394KZ.Display
{
    internal class Frame
    {
        private readonly CharacterUnit[,] frame;

        public int Width => frame.GetLength(0);
        public int Height => frame.GetLength(1);

        public Frame(uint width, uint height)
        {
            frame = new CharacterUnit[width, height];
        }

        public Frame() : this((uint)Console.WindowWidth, (uint)Console.WindowHeight) { }

        public CharacterUnit this[int x, int y]
        {
            get => frame[x, y];
            set => frame[x, y] = value;
        } 

        public void SetCharUnit(int x, int y, char c, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if(x < 0 || y < 0 || x > Width || y > Height) throw new ArgumentOutOfRangeException();
            frame[x,y].letter = c;
            frame[x,y].backgroundColor = backgroundColor;
            frame[x,y].foregroundColor = foregroundColor;
        }
    }
}
