using System;
using ConsoleDraw;
using ConsoleDraw.Interfaces;

namespace ConsoleDrawTest
{
    public class DrawingCommandHelper: IDrawingCommandHelper
    {
        public int GetConsoleWidth()
        {
            return 120;
        }
        public int GetConsoleHeight()
        {
            return 30;
        }

        public int GetProgramOriginalColumn()
        {
           return 0;
        }

        public int GetProgramOriginalRow()
        {
            return 9;
        }

        public void SetCursorPosition()
        {
             // empty is ok (for now):
        }
    } 
}
