using E394KZ.Exceptions;
using System.Text.RegularExpressions;

namespace E394KZ.Shapes
{
    static internal class ShapeParser
    {
        public static bool IsStringStartingWidthShape(string text)
        {
            var prefixArray = new string[] { "dot ", "line ", "circle ", "rectangle ", "triangle " };
            foreach (var prefix in prefixArray)
            {
                if (text.StartsWith(prefix)) return true;
            }
            return false;
        }
        private static bool ContainsInvalidCharacter(string text)
        {
            string validCharsPattern = @"^[a-zA-Z0-9_.-]+$";
            return !Regex.IsMatch(text, validCharsPattern);
        }
        static public BaseShape Parse(string text, ShapeHistory shapeHistory, uint canvasWidth, uint canvasHeight)
        {
            static string NameChecker(string name, ShapeHistory shapeHistory)
            {
                if (ContainsInvalidCharacter(name)) throw new InvalidCharacterInNameException("shape nickname");
                if (IsStringStartingWidthShape(name.ToLower()))
                {
                    var tmp = name.Replace("dot", "").Replace("line", "").Replace("circle", "").Replace("rectangle", "").Replace("triangle", "");
                    if (tmp.Length > 0 && char.IsDigit(tmp[0])) throw new InvalidNameException(name);
                }
                else
                {
                    if (shapeHistory.Any(shape => shape.Name == name)) throw new NameAlreadyInUseException(name);
                }
                return name;
            }

            static string GetAutoName(ShapeHistory shapeHistory, string shapename)
            {
                int countOfThatTypeOfShape = shapeHistory.Count(shape => shape.GetShapeName() == shapename);
                var name = new String(
                    new[] { char.ToUpper(shapename.First()) }
                    .Concat(shapename.Skip(1))
                    .ToArray()
                );
                return $"{name}{countOfThatTypeOfShape + 1}";
            }
            static uint StrToUint(string str, uint upperBound)
            {
                var isSuccesful = UInt32.TryParse(str, out var result);
                if (!isSuccesful || result >= upperBound) throw new CoordinateOutOfCanvas();
                return result;
            }

            var textSplit = text.Split(' ');

            uint x;
            uint y;
            ConsoleColor color;
            string name;

            switch (textSplit[0].ToLower())
            {
                case "dot":
                    if (textSplit.Length != 5 && textSplit.Length != 4) throw new InvalidArgumentumCountException("dot", textSplit.Length);
                    x = StrToUint(textSplit[1], canvasWidth);
                    y = StrToUint(textSplit[2], canvasWidth);
                    color = ConsoleColorParser(textSplit[3]);
                    name = (textSplit.Length == 5) ? NameChecker(textSplit[4], shapeHistory) : GetAutoName(shapeHistory, "dot");
                    return new Dot(name, x, y, color);

                case "line":
                    if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("line", textSplit.Length);
                    x = StrToUint(textSplit[1], canvasWidth);
                    y = StrToUint(textSplit[2], canvasHeight);
                    var x2 = StrToUint(textSplit[3], canvasWidth);
                    var y2 = StrToUint(textSplit[4], canvasHeight);
                    color = ConsoleColorParser(textSplit[5]);
                    name = (textSplit.Length == 7) ? NameChecker(textSplit[6], shapeHistory) : GetAutoName(shapeHistory, "line");
                    return new Line(name, x, y, x2, y2, color);

                case "rectangle":
                    if (textSplit.Length != 7 && textSplit.Length != 6) throw new InvalidArgumentumCountException("rectangle", textSplit.Length);
                    x = StrToUint(textSplit[1], canvasWidth);
                    y = StrToUint(textSplit[2], canvasHeight);
                    var width = StrToUint(textSplit[3], canvasWidth - x);
                    var height = StrToUint(textSplit[4], canvasHeight - y);
                    color = ConsoleColorParser(textSplit[5]);
                    name = (textSplit.Length == 7) ? NameChecker(textSplit[6], shapeHistory) : GetAutoName(shapeHistory, "rectangle");
                    return new Rectangle(name, x, y, width, height, color);

                case "circle":
                    if (textSplit.Length != 6 && textSplit.Length != 5) throw new InvalidArgumentumCountException("circle", textSplit.Length);
                    x = StrToUint(textSplit[1], canvasWidth);
                    y = StrToUint(textSplit[2], canvasHeight);
                    var r = Convert.ToUInt32(textSplit[3]);
                    color = ConsoleColorParser(textSplit[4]);
                    name = (textSplit.Length == 6) ? NameChecker(textSplit[5], shapeHistory) : GetAutoName(shapeHistory, "circle");
                    return new Circle(name, x, y, color, r);

                case "triangle":
                    if (textSplit.Length != 9 && textSplit.Length != 8) throw new InvalidArgumentumCountException("triangle", textSplit.Length);
                    x = StrToUint(textSplit[1], canvasWidth);
                    y = StrToUint(textSplit[2], canvasHeight);
                    var v2x = StrToUint(textSplit[3], canvasWidth);
                    var v2y = StrToUint(textSplit[4], canvasHeight);
                    var v3x = StrToUint(textSplit[5], canvasWidth);
                    var v3y = StrToUint(textSplit[6], canvasHeight);
                    color = ConsoleColorParser(textSplit[7]);
                    name = (textSplit.Length == 9) ? NameChecker(textSplit[8], shapeHistory) : GetAutoName(shapeHistory, "triangle");
                    return new Triangle(name, x, y, v2x, v2y, v3x, v3y, color);
                default:
                    throw new Exception("this should never be called");
            }
        }
        private static ConsoleColor ConsoleColorParser(string colorString)
        {
            if (Enum.TryParse<ConsoleColor>(colorString, true, out ConsoleColor color))
            {
                return color;
            }
            else
            {
                throw new InvalidColorException(colorString);
            }
        }
    }
}
