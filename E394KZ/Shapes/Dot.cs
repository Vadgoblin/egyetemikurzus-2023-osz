using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E394KZ.Shapes
{
    internal class Dot : BaseShape
    {
        public Dot(string name, uint x, uint y, ConsoleColor color) : base(name, x, y,color)
        {
        }
        public override ConsoleColor?[,] Draw(ConsoleColor?[,] canvas)
        {
            canvas[x, y] = color;
            return canvas;
        }
    }
}
