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

        public CharacterUnit this[uint x, uint y]
        {
            get => frame[x, y];
            set => frame[x, y] = value;
        } 
    }
}
