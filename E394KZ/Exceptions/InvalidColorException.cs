namespace E394KZ.Exceptions
{
    class InvalidColorException : ShapeException
    {
        public InvalidColorException(string colorString)
            : base($"Invalid ConsoleColor: {colorString}", "InvalidColorException"){}
    }
}
