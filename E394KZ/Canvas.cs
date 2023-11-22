using E394KZ.Shapes;

namespace E394KZ
{
    internal class Canvas
    {
        private ConsoleColor?[,] ColorArray { get; set; }

        public uint Width => (uint)ColorArray.GetLength(0);
        public uint Height => (uint)ColorArray.GetLength(1);

        public Canvas(uint width, uint height) 
        {
            ColorArray = new ConsoleColor?[width, height];
        }

        public ConsoleColor? this[uint x, uint y]
        {
            get
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    return ColorArray[x, y];
                }
                else
                {
                    throw new IndexOutOfRangeException("Index is out of range");
                }
            }
            set
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    ColorArray[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Index is out of range");
                }
            }
        }

        public void Fill(ConsoleColor? color)
        {
            for(uint i =  0; i < Width; i++)
            {
                for(uint j  = 0; j < Height; j++)
                {
                    this[i, j] = color;
                }
            }
        }

        public void Draw(BaseShape shape)
        { 
            shape.Draw(this);
        }
        public void Draw(ShapeHistory shapeList)
        {
            for(int i = 0 ; i < shapeList.Count; i++) Draw(shapeList[i]);
        }

        public void Clear()
        {
            for(int i =  0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    ColorArray[i,j]= null;
                }
            }
        }
    }
}
