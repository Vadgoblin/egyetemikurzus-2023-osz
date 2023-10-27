using E394KZ.Shapes;

namespace E394KZ
{
    internal static class Stat
    {
        static readonly SemaphoreSlim semaphoreSlim = new(1, 1);
        private static Dictionary<BaseShape, uint>? size;
        public static string[] GetStat(List<BaseShape> shapeHistory,uint canvasWidth, uint canvasHeight)
        {
            var top5LargesShape = GetTop5largesShape(shapeHistory, canvasWidth, canvasHeight);
            var top5colorCount = GetTop5MostCommonShapeColor(shapeHistory);
            var totalShapeCount = shapeHistory.Count;
            var totalAffectedPixelCount = GetAffectedPixelCount(shapeHistory, canvasWidth, canvasHeight);

            //foreach (var a in top5LargesShape) { Console.WriteLine($"{a.Key} {a.Value}"); }
            //foreach (var a in top5colorCount) { Console.WriteLine($"{a.Key} {a.Value}"); }
            //Console.WriteLine(totalShapeCount);
            //Console.WriteLine(totalAffectedPixelCount);
            //Console.ReadLine();

            return new string[] {
                $"   Top 5 larges shape:        Top 5 shape color:      ",
                $"1. {top5LargesShape[0].PadRight(26)} {top5colorCount[0]}",
                $"2. {top5LargesShape[1].PadRight(26)} {top5colorCount[1]}",
                $"3. {top5LargesShape[2].PadRight(26)} {top5colorCount[2]}",
                $"4. {top5LargesShape[3].PadRight(26)} {top5colorCount[3]}",
                $"5. {top5LargesShape[4].PadRight(26)} {top5colorCount[4]}",
                $"",

                $"Shape count: {totalShapeCount}",
                $"Affected pixels on the canvas: {totalAffectedPixelCount}",
            };
        }


        private static async Task GetShapeArea(BaseShape shape,uint canvasWidth,uint canvasHeight)
        {
            var tmpCanvas = new Canvas(canvasWidth, canvasHeight);

            uint tmpSize = 0;
            tmpCanvas.Draw(shape);
            for (uint x = 0; x < canvasWidth; x++)
            {
                for (uint y = 0; y < canvasHeight; y++)
                {
                    if (tmpCanvas[x, y] != null) tmpSize++;
                }
            }

            await semaphoreSlim.WaitAsync();
            try
            {
                size!.Add(shape, tmpSize);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        private static List<string> GetTop5largesShape(List<BaseShape> shapeHistory, uint canvasWidth, uint canvasHeight)
        {
            size = new Dictionary<BaseShape, uint>();
            var taskList = new List<Task>();
            foreach (var shape in shapeHistory)
            {
                var newTask = new Task(async () =>
                {
                    await GetShapeArea(shape, canvasWidth, canvasHeight);
                });
                newTask.Start();
                taskList.Add(newTask);
            }
            Task.WaitAll(taskList.ToArray());

            List<string> stringList = size
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            .Select(kvp => $"{LimitStringLength(kvp.Key.Name,12)} ({kvp.Value})")
            .ToList();

            while (stringList.Count < 5)
            {
                stringList.Add("");
            }

            return stringList;

        }
        private static List<string> GetTop5MostCommonShapeColor(List<BaseShape> shapeHistory)
        {
            var fiveMostCommonShapeColorList = shapeHistory
            .GroupBy(shape => shape.Color)
            .ToDictionary(group => group.Key, group => group.Count())
            .OrderByDescending(kv => kv.Value)
            .Take(5)
            .ToDictionary(kv => kv.Key, kv => (uint)kv.Value)
            .Select(kvp => $"{kvp.Key} ({kvp.Value})")
            .ToList();

            while(fiveMostCommonShapeColorList.Count < 5)
            {
                fiveMostCommonShapeColorList.Add("");
            }

            return fiveMostCommonShapeColorList;
        }
        private static uint GetAffectedPixelCount(List<BaseShape> shapeHistory, uint canvasWidth, uint canvasHeight)
        {
            var tmpCanvas = new Canvas(canvasWidth, canvasHeight);
            tmpCanvas.Draw(shapeHistory);

            uint affectedPixels = 0;
            for(uint x = 0; x < canvasWidth; x++)
            {
                for(uint y = 0; y < canvasHeight; y++)
                {
                    if (tmpCanvas[x, y] != null) affectedPixels++;
                }
            }

            return affectedPixels;
        }

        private static string LimitStringLength(string input, int maxLength)
        {
            if (input.Length > maxLength)
            {
                return input.Substring(0, maxLength);
            }
            else
            {
                return input;
            }
        }
    }
}
