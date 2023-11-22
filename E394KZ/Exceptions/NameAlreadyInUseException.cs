namespace E394KZ.Exceptions
{
    internal class NameAlreadyInUseException : Exception
    {

        public NameAlreadyInUseException(string name) : base($"\"{name}\" shape nichname is already in use.")
        {
        }
    }
}
