using ConsoleDraw.Interfaces;

namespace ConsoleDraw
{ 
    public class DrawingCommandHelper: IDrawingCommandHelper
    {
        public int GetConsoleWidth()
        {
            return Console.WindowWidth;
        }
        public int GetConsoleHeight()
        {
            return Console.WindowHeight;
        }

        public int GetProgramOriginalColumn()
        {
            return Program.origCol;
        }
        public int GetProgramOriginalRow()
        {
            return Program.origRow;
        }
          
        public void SetCursorPosition()
        {
            Console.SetCursorPosition(Program.origCol,
                                           Program.origRow + 3);
        }

    } 
}
