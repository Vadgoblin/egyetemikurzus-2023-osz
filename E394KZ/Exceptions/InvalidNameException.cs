namespace E394KZ.Exceptions
{
    class InvalidNameException : ShapeException
    {
        public InvalidNameException(string name)
        : base($"\"{name}\" is invalid name for a shape", "InvalidNameException") { }
    }
}
