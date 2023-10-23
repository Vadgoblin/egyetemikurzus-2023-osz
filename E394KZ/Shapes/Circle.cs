namespace E394KZ.Shapes
{
    internal record Circle : BaseShape
    {
        public uint Radius { get; init; }
        public Circle(string name, uint x, uint y, ConsoleColor color, uint radius) : base(name, x, y, color)
        {
            Radius = radius;
        }
        public override void Draw(Canvas canvas)
        {
            for (uint y = 0; y < canvas.Height; y++)
            {
                for (uint x = 0; x < canvas.Width; x++)
                {
                    double distance = Math.Sqrt(Math.Pow(X - (int)x, 2) + Math.Pow(Y - (int)y, 2));
                    if (distance < Radius)
                    {
                        //Console.Write("X");
                        canvas[x, y] = Color;
                    }
                    //else Console.Write(" ");
                }
                //Console.WriteLine();
            }
        }
    }
}
