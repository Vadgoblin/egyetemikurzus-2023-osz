namespace E394KZ.Program.Exceptions
{
    internal class NameAlreadyInUseException : Exception
    {

        public NameAlreadyInUseException(string name) : base($"\"{name}\" shape nichname is already in use.")
        {
        }
    }
}
