using System;
using ConsoleDraw;
using ConsoleDraw.Interfaces;

namespace ConsoleDrawTest
{
    public class DrawingCommandValidatorHelper: IDrawingCommandValidatorHelper
    {
        public int GetConsoleWidth()
        {
            return 120;
        }
        public int GetConsoleHeight()
        {
            return 30;
        }
    } 
}
