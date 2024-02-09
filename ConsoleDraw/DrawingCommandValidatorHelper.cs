using ConsoleDraw.Interfaces;

namespace ConsoleDraw
{ 
    public class DrawingCommandValidatorHelper: IDrawingCommandValidatorHelper
    {
        public int GetConsoleWidth()
        {
            return Console.WindowWidth;
        }
        public int GetConsoleHeight()
        {
            return Console.WindowHeight;
        }
    } 
}
