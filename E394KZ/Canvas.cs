namespace E394KZ
{
    internal class Canvas
    {
        private ConsoleColor?[,] ColorArray { get; set; }

        public int Width => ColorArray.GetLength(1);
        public int Height => ColorArray.GetLength(0);

        public Canvas(uint width, uint height) 
        {
            ColorArray = new ConsoleColor?[width, height];
        }

        public ConsoleColor? this[int row, int column]
        {
            get
            {
                if (row >= 0 && row < Height && column >= 0 && column < Width)
                {
                    return ColorArray[row, column];
                }
                else
                {
                    throw new IndexOutOfRangeException("Index is out of range");
                }
            }
            set
            {
                if (row >= 0 && row < Height && column >= 0 && column < Width)
                {
                    ColorArray[row, column] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Index is out of range");
                }
            }
        }
    }
}
