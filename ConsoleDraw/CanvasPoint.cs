using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw
{
   
    public class CanvasPoint
    {
        public bool IsPartOfLine { get; set; } = false; 
        public int AreaNumber { get; private set; } = 0; 
        public int Row { get; set; } = 0;
        public int Col { get; set; } = 0;
        public ConsoleColor? NeighborConsoleColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor? OwnConsoleColor { get; set; } = ConsoleColor.Black;
    
        // adjacent canvas point neighbors for bucket paint
        public List<CanvasPoint> CanvasPointNeighbors { get; set; } = new List<CanvasPoint>(); 

        public CanvasPoint(int _row, int _col)
        {
            Row = _row;
            Col = _col; 
        } 
    }
}
