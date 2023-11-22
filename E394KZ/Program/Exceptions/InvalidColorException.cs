namespace E394KZ.Program.Exceptions
{
    class InvalidColorException : ShapeException
    {
        public InvalidColorException(string colorString)
            : base($"Invalid ConsoleColor: {colorString}", "InvalidColorException") { }
    }
}
