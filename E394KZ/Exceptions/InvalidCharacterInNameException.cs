namespace E394KZ.Exceptions
{
    class InvalidCharacterInNameException : Exception
    {
        public InvalidCharacterInNameException(string inWhatName) : base($"Invalid caharcter in {inWhatName}.") { }
    }
}
