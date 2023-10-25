using System.Text.Json.Serialization;

namespace E394KZ.Shapes
{
    [JsonDerivedType(typeof(Dot), typeDiscriminator: "dot")]
    [JsonDerivedType(typeof(Line), typeDiscriminator: "line")]
    [JsonDerivedType(typeof(Circle), typeDiscriminator: "circle")]
    [JsonDerivedType(typeof(Rectangle), typeDiscriminator: "rectangle")]
    [JsonDerivedType(typeof(Triangle), typeDiscriminator: "triangle")]
    internal abstract record BaseShape
    {
        public string Name { get; init; }

        public uint X { get; init; }
        public uint Y { get; init; }

        public ConsoleColor Color { get; init; }

        public BaseShape(string name, uint x, uint y, ConsoleColor color)
        {
            Name = name;
            X = x;
            Y = y;
            Color = color;
        }

        public abstract void Draw(Canvas canvas);
        public abstract string GetShapeName();
    }
}
