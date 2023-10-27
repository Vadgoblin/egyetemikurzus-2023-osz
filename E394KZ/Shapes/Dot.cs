namespace E394KZ.Shapes
{
    internal record Dot : BaseShape
    {
        public Dot(string name, uint x, uint y, ConsoleColor color) : base(name, x, y,color){}
        public override void Draw(Canvas canvas)
        {
            canvas[X, Y] = Color;
        }
        public override string GetShapeName()
        {
            return "dot";
        }
    }
}
