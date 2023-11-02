namespace E394KZ.Exceptions
{
    class InvalidArgumentumCountException : ShapeException
    {
        public InvalidArgumentumCountException(string command, int argcount)
            : base($"Invalid argumentum count ({argcount}) for {command}", "InvalidArgumentumCountException") { }
    }
}
