namespace E394KZ.Shapes
{
    class InvalidConsoleColorException : Exception
    {
        public InvalidConsoleColorException(string colorString)
            : base($"Invalid ConsoleColor: {colorString}")
        {
        }
    }
    class InvalidArgumentumCountException : Exception
    {
        public InvalidArgumentumCountException(string shapetype,int argcount)
            : base($"Invalid argumentum count({argcount}) for {shapetype}") { }
    }
    class InvalidNameException : Exception
    {
        public InvalidNameException(string name)
        : base($"\"{name}\" is invalid name for a shape") { }
    }
}
