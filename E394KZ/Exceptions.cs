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
        public InvalidArgumentumCountException(string command,int argcount)
            : base($"Invalid argumentum count ({argcount}) for {command}", "InvalidArgumentumCountException") { }
    }
    class InvalidNameException : ShapeException
    {
        public InvalidNameException(string name)
        : base($"\"{name}\" is invalid name for a shape", "InvalidNameException") { }
    }

    class CoordinateOutOfCanvas : Exception
    {
        public CoordinateOutOfCanvas(){}
    }

    class WindowsTooSmallException : Exception
    {
        public WindowsTooSmallException():base(){}
    }
    class LoadException : Exception
    {
        public LoadException(string msg) : base(msg) { }
    }
    class InvalidCharacterInNameException : Exception
    {
        public InvalidCharacterInNameException(string inWhatName) : base($"Invalid caharcter in {inWhatName}.") { }
    }
}
