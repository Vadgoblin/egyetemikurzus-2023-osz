﻿namespace E394KZ.Program.Exceptions
{
    abstract class ShapeException : Exception
    {
        public string ExceptionType { get; init; }
        public ShapeException(string messange, string exceptiontype) : base(messange)
        {
            ExceptionType = exceptiontype;
        }
    }
}
