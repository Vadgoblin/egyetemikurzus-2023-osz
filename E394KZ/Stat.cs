using E394KZ.Shapes;

namespace E394KZ
{
    internal static class Stat
    {
        static readonly SemaphoreSlim semaphoreSlim = new(1, 1);
        private static Dictionary<BaseShape, uint>? size;
        public static string[] GetStat(ShapeHistory shapeHistory,uint canvasWidth, uint canvasHeight)
        {
            var top5LargesShape = GetTop5largesShape(shapeHistory, canvasWidth, canvasHeight);
            var top5colorCount = GetTop5MostCommonShapeColor(shapeHistory);
            var totalShapeCount = shapeHistory.Count;
            var totalAffectedPixelCount = GetAffectedPixelCount(shapeHistory, canvasWidth, canvasHeight);

            return new string[] {
                $"   Top 5 larges shape:        Top 5 shape color:      ",
                $"1. {top5LargesShape[0],-26} {top5colorCount[0]}",
                $"2. {top5LargesShape[1],-26} {top5colorCount[1]}",
                $"3. {top5LargesShape[2],-26} {top5colorCount[2]}",
                $"4. {top5LargesShape[3],-26} {top5colorCount[3]}",
                $"5. {top5LargesShape[4],-26} {top5colorCount[4]}",
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
        private static List<string> GetTop5largesShape(ShapeHistory shapeHistory, uint canvasWidth, uint canvasHeight)
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
            .Select(kvp => $"{LimitStringLength(kvp.Key.Name,12)} ({kvp.Value})")
            .ToList();

            while (stringList.Count < 5)
            {
                stringList.Add("");
            }

            return stringList;

        }
        private static List<string> GetTop5MostCommonShapeColor(ShapeHistory shapeHistory)
        {
            var fiveMostCommonShapeColorList = shapeHistory
            .GroupBy(shape => shape.Color)
            .ToDictionary(group => group.Key, group => group.Count())
            .OrderByDescending(kv => kv.Value)
            .Take(5)
            .Select(kvp => $"{kvp.Key} ({kvp.Value})")
            .ToList();

            while(fiveMostCommonShapeColorList.Count < 5)
            {
                fiveMostCommonShapeColorList.Add("");
            }

            return fiveMostCommonShapeColorList;
        }
        private static uint GetAffectedPixelCount(ShapeHistory shapeHistory, uint canvasWidth, uint canvasHeight)
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
                return input[..maxLength];
            }
            else
            {
                return input;
            }
        }
    }
}
