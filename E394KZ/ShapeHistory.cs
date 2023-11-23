using E394KZ.Exceptions;
using E394KZ.Shapes;
using System.Collections;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace E394KZ
{
    internal class ShapeHistory : IEnumerable<BaseShape>
    {
        private List<BaseShape> shapeHistory = new();

        public BaseShape this[int index]
        {
            get
            {
                if (index < 0 || index >= shapeHistory.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return shapeHistory[index];
            }
        }

        public void Add(BaseShape shape)
        {
            shapeHistory.Add(shape);
        }

        public void Clear()
        {
            shapeHistory.Clear();
        }

        public void RemoveLast()
        {
            shapeHistory.RemoveAt(shapeHistory.Count - 1);
        }

        public int Count => shapeHistory.Count;

        public void Save(string saveName)
        {
            if (ContainsInvalidCharacter(saveName)) throw new InvalidCharacterInNameException("save name");
            try
            {
                if (!Directory.Exists("saves")) Directory.CreateDirectory("saves");

                var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(Path.Combine("saves",$"{saveName}.json"), JsonSerializer.Serialize(shapeHistory, jsonOptions));

                GUI.DrawMsgbox("Save succesfull.", "Save", false);
            }
            catch(IOException)
            {
                GUI.DrawMsgbox("Save failed.", "Save error");
            }
        }

        public void Load(string saveName)
        {
            if (ContainsInvalidCharacter(saveName)) throw new InvalidCharacterInNameException("load name");
            if (!File.Exists(Path.Combine("saves", $"{saveName}.json"))) throw new LoadException($"There is no save named \"{saveName}\".");
            else
            {
                try
                {
                    var jsonTExt = File.ReadAllText(Path.Combine("saves", $"{saveName}.json"));
                    shapeHistory = JsonSerializer.Deserialize<List<BaseShape>>(jsonTExt) ?? throw new Exception();
                }
                catch (IOException)
                {
                    GUI.DrawMsgbox("Load failed.", "IOException");
                }
                
            }
        }

        private static bool ContainsInvalidCharacter(string text)
        {
            string validCharsPattern = @"^[a-zA-Z0-9_.-]+$";
            return !Regex.IsMatch(text, validCharsPattern);
        }

        public IEnumerator<BaseShape> GetEnumerator()
        {
            return new ShapeHistoryEnumerator(shapeHistory);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private class ShapeHistoryEnumerator : IEnumerator<BaseShape>
        {
            private readonly List<BaseShape> shapeList;
            private int currentIndex = -1;

            public ShapeHistoryEnumerator(List<BaseShape> list)
            {
                shapeList = list;
            }

            public BaseShape Current
            {
                get
                {
                    if (currentIndex >= 0 && currentIndex < shapeList.Count)
                    {
                        return shapeList[currentIndex];
                    }
                    throw new InvalidOperationException();
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < shapeList.Count;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose()
            {
                // Dispose resources if needed
            }
        }
    }
}
