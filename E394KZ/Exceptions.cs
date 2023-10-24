namespace E394KZ.Shapes
{
    abstract class ShapeException : Exception
    {
        public string ExceptionType { get; init; }
        public ShapeException(string messange, string exceptiontype) : base(messange)
        {
            ExceptionType = exceptiontype;
        }
    }
    class InvalidColorException : ShapeException
    {
        public InvalidColorException(string colorString)
            : base($"Invalid ConsoleColor: {colorString}", "InvalidColorException")
        {
        }
    }
    class InvalidArgumentumCountException : ShapeException
    {
        public InvalidArgumentumCountException(string shapetype,int argcount)
            : base($"Invalid argumentum count({argcount}) for {shapetype}", "InvalidArgumentumCountException") { }
    }
    class InvalidNameException : ShapeException
    {
        public InvalidNameException(string name)
        : base($"\"{name}\" is invalid name for a shape", "InvalidNameException") { }
    }

    class WindowsTooSmallException : Exception
    {
        public WindowsTooSmallException():base(){}
    }
}
