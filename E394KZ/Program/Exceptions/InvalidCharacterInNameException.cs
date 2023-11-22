namespace E394KZ.Program.Exceptions
{
    class InvalidCharacterInNameException : Exception
    {
        public InvalidCharacterInNameException(string inWhatName) : base($"Invalid caharcter in {inWhatName}.") { }
    }
}
