using E394KZ;
using E394KZ.Exceptions;
using E394KZ.Shapes;
using System.Text;

static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var canvas = new Canvas(1920, 1080);
        var shapeHistory = new ShapeHistory();

        while (true)
        {
            Console.Title = $"Offset: {GUI.Xoffset}x{GUI.Yoffset}, Canvas size: {canvas.Width}x{canvas.Height}";
            try
            {
                GUI.RedrawScreen(canvas, shapeHistory);
                var input = Console.ReadLine() ?? "";

                if (input != "") UserInputProcessor.Process(input, canvas, shapeHistory);
            }
            catch (WindowsTooSmallException)
            {
                Console.Clear();
                Console.WriteLine("Window is too small!");
                Console.WriteLine("It must be at least 11 character high and 50 character wide");
                while (GUI.IsWindowTooSmall()) Thread.Sleep(50);
            }
            catch (ShapeException ex)
            {
                GUI.DrawMsgbox(ex.Message, "InvalidArgumentumCountException");
            }
            catch (CoordinateOutOfCanvas)
            {
                GUI.DrawMsgbox("Shape's all point must be within the canvas.", "CoordinateOutOfCanvas");
            }
            catch (LoadException ex)
            {
                GUI.DrawMsgbox(ex.Message, "LoadException");
            }
            catch (InvalidCharacterInNameException ex)
            {
                GUI.DrawMsgbox(ex.Message, "InvalidCharacterInNameException");
            }
            catch (NameAlreadyInUseException ex)
            {
                GUI.DrawMsgbox(ex.Message, "NameAlreadyInUseException");
            }
            catch (Exception)
            {
                GUI.DrawMsgbox("Unknown error", "");
                return;
            }
        }
    }

}