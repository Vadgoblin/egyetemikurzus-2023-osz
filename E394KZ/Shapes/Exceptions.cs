using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
