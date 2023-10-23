namespace E394KZ.Shapes
{
    internal abstract class BaseShape
    {
        public string name { get; init; }

        public uint x { get; private set; }
        public uint y { get; private set; }

        public ConsoleColor color { get; private set; }

        public BaseShape(string name, uint x, uint y, ConsoleColor color)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public abstract ConsoleColor?[,] Draw(ConsoleColor?[,] canvas);
    }
}
