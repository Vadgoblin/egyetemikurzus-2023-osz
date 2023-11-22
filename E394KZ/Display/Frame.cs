namespace E394KZ.Display
{
    internal class Frame
    {
        private readonly CharacterUnit[,] frame;

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
