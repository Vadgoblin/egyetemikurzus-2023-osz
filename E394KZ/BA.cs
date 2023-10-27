using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E394KZ
{
    partial class BA
    {
        static public void Start()
        {
            ScreenSizeCheck();
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            var magicConstant = File.ReadAllBytes("bincodedmagicconstant.bin");
            /*
             * Imagine if it were a hardcoded byte array.
             * The reason why is's in a file is because its a tiny little bit big and poor VS would have a hard time handling it.
             * In other words, it would slow down VS and make it eat all of your ram .
             */
            var w = new Stopwatch();
            double asd = 0;
            w.Start();
            var s = new StringBuilder();
            int q = 0;
            for (int i = 0; i < 6569; i++)
            {
                for(int l = 0; l < 18; l++)
                {
                    for(int c = 0; c < 12; c++)
                    {
                        s.Append(Decode(magicConstant[q++]));
                    }
                    s.Append('\n');
                }
                asd += (1000 / (double)30);
                while (w.ElapsedMilliseconds < asd) ;
                Console.SetCursorPosition(0, 0);
                Console.Write(s.ToString().Substring(0, s.ToString().Length-1));
                s.Clear();
            }
            Console.ResetColor();
        }

        static private void ScreenSizeCheck()
        {
            while (Console.WindowHeight < 18 || Console.WindowWidth < 49) Thread.Sleep(50);
        }

        private static readonly char[] validChars = { '▀', '▄', '█', ' ' };
        private static string Decode(byte input)
        {
            if (validChars.Length != 4)
            {
                throw new ArgumentException("Valid character array must contain exactly 4 characters.");
            }

            StringBuilder result = new(4);
            for (int i = 0; i < 4; i++)
            {
                int index = (input >> (i * 2)) & 0b11;
                result.Append(validChars[index]);
            }

            return result.ToString();
        }
    }
}