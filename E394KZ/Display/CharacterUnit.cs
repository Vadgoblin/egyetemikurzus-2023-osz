namespace E394KZ.Display
{
    internal struct CharacterUnit
    {
        public char letter;
        public ConsoleColor backgroundColor;
        public ConsoleColor foregroundColor;

        public CharacterUnit()
        {
            backgroundColor = Console.BackgroundColor;
            foregroundColor = Console.ForegroundColor;
        }
    }
}
