using ConsoleDraw.Interfaces;

namespace ConsoleDraw
{
    public class DrawingCommands : IDrawingCommands
    {

        public int InternalCanvasHeight { get; set; } = 0;
        public int InternalCanvasWidth { get; set; } = 0; 
        public bool HasCanvas { get; set; } = false;
        public ICanvasMatrix CanvasMatrix { get; set; } = new CanvasMatrix(0,0);

        private IDrawingCommandHelper drawingCommandHelper;
        
        public DrawingCommands(IDrawingCommandHelper _drawingCommandHelper )
        {
            drawingCommandHelper = _drawingCommandHelper; 
        }
        public void ShowCommands()
        {
            // Display title as the C# console calculator app.
            Console.Clear();
            Console.WriteLine("Console Draw in C# - the commands are case sensitive characters, as shown below.\r");
            Console.WriteLine("-----------------------------------------------------------------------------------------------");
            Console.WriteLine("Command                Description");
            Console.WriteLine("C w h                  Create a new canvas of width w and height h.");
            Console.WriteLine("L col1 row1 col2 row2  Create line from point (col1,row1) to point (col2,row2), no diagonals.");
            Console.WriteLine("R col1 row1 col2 row2  Create rectangle, upper left corner is (col1,row1), lower right corner is (col2,row2).");
            Console.WriteLine("B col1 row1 color      Bucket paint area connected to point (col1,row1) with a color (eg. 'Red' or 'Green') .");
            Console.WriteLine("Q                      Quit the program.");
            Console.WriteLine("-----------------------------------------------------------------------------------------------"); 
        }

        public bool ExecuteCommand(string[] inputs, int cursorRowNr)
        {
            try
            {
                switch (inputs.First())
                {
                    case "C":
                        InternalCanvasWidth = Convert.ToInt32(inputs.ToList()[1]);
                        InternalCanvasHeight = Convert.ToInt32(inputs.ToList()[2]);
                        DrawCanvas(InternalCanvasWidth, InternalCanvasHeight, cursorRowNr);
                        break;
                    case "L":
                        DrawLine(Convert.ToInt32(inputs.ToList()[1]),
                                 Convert.ToInt32(inputs.ToList()[2]),
                                 Convert.ToInt32(inputs.ToList()[3]),
                                 Convert.ToInt32(inputs.ToList()[4]),
                                 cursorRowNr);
                        break;
                    case "R":
                        DrawSquare(Convert.ToInt32(inputs.ToList()[1]),
                                   Convert.ToInt32(inputs.ToList()[2]),
                                   Convert.ToInt32(inputs.ToList()[3]),
                                   Convert.ToInt32(inputs.ToList()[4]),
                                   cursorRowNr);
                        break;
                    case "B":
                        BucketPaint(Convert.ToInt32(inputs.ToList()[1]),
                                   Convert.ToInt32(inputs.ToList()[2]),
                                   inputs.ToList()[3],
                                   cursorRowNr);
                        break;
                    case "Q":
                    // handled in program main method
                    default:
                        Console.SetCursorPosition(Program.origCol, Program.origRow + 1);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(Program.origCol, Program.origRow + 1);
                        Console.Write($"Wrong command letter, obviously the validation failed to reach this point.");
                        return false; 
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.SetCursorPosition(Program.origCol, Program.origRow + 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(Program.origCol, Program.origRow + 1);
                Console.Write($"Command Error: {ex.Message}"); 
            }

            return false;
        }

        private void BucketPaint(int col1, int row1, string consoleColorString, int cursorRowNr)
        {
            try
            {
                var consoleColor = Enum.Parse<ConsoleColor>(consoleColorString);

                Console.SetCursorPosition(col1, cursorRowNr + 4);
                bool allCPMsFound = false;
                int currentRow = row1 - 1;
                int currentColumn = col1 - 1;

                while (!allCPMsFound)
                {
                    CanvasMatrix.CanvasPointMatrix[currentRow][currentColumn].NeighborConsoleColor = consoleColor;
                    CanvasMatrix.CanvasPointMatrix[currentRow][currentColumn].OwnConsoleColor = consoleColor; 
                    CanvasMatrix.SetNeighborColorChar(currentRow, currentColumn, consoleColor);
                   
                    // items in each row
                    for (int j = currentColumn + 1; j < CanvasMatrix.CanvasPointMatrix[currentRow].Length; j++)
                    {
                        if (j == CanvasMatrix.CanvasPointMatrix[currentRow].Length ||
                            CanvasMatrix.CanvasPointMatrix[currentRow][j].IsPartOfLine)
                        {
                            break;
                        }


                        if (CanvasMatrix.CanvasPointMatrix[currentRow][j].NeighborConsoleColor == consoleColor)
                        {
                            CanvasMatrix.CanvasPointMatrix[currentRow][j].OwnConsoleColor = consoleColor;
                            CanvasMatrix.SetNeighborColorChar(currentRow, j, consoleColor);
                        }
                    } 
                     
                    currentRow = currentColumn = -1;
                    for (int i = 0; i < CanvasMatrix.CanvasPointMatrix.Length; i++)
                    {
                        for (int j = 0; j < CanvasMatrix.CanvasPointMatrix[i].Length; j++)
                        {
                            if (CanvasMatrix.CanvasPointMatrix[i][j].NeighborConsoleColor == consoleColor && 
                                CanvasMatrix.CanvasPointMatrix[i][j].OwnConsoleColor != consoleColor &&
                                !CanvasMatrix.CanvasPointMatrix[i][j].IsPartOfLine)
                            {
                                currentRow = i;
                                currentColumn = j;
                                break;
                            } 
                        }

                        if (currentRow > -1)
                        {
                            break;
                        }
                    }
                     
                    if (currentRow == -1)
                    {
                        allCPMsFound = true;
                    } 
                } 

                for (int i = 0; i < CanvasMatrix.CanvasPointMatrix.Length; i++)
                {
                    for (int j = 0; j < CanvasMatrix.CanvasPointMatrix[i].Length; j++)
                    {
                        if (CanvasMatrix.CanvasPointMatrix[i][j].OwnConsoleColor == consoleColor)
                        {   
                            Console.SetCursorPosition(j, i); 
                            WriteAt(" ", j + 1, cursorRowNr + 4 + i, consoleColor); 
                        }
                    }
                } 
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DrawSquare(int col1, int row1, int col2, int row2, int cursorRowNr)
        {
            try
            {
                // A square or rectangle is made up of 4 lines
                (int, int, int, int) topLine = (col1, row1, col2, row1);
                (int, int, int, int) leftLine = (col1, row1, col1, row2);
                (int, int, int, int) rightLine = (col2, row1, col2, row2);
                (int, int, int, int) bottomLine = (col1, row2, col2, row2);

                (int, int, int, int)[] squareLines = new[] { topLine, leftLine, rightLine, bottomLine };


                foreach (var sqline in squareLines)
                {
                   DrawLine(sqline.Item1,
                            sqline.Item2,
                            sqline.Item3,
                            sqline.Item4,
                            cursorRowNr); 
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DrawLine(int col1, int row1, int col2, int row2, int cursorRowNr)
        {
            try
            { 
                if (row1 == row2)
                {
                    // Same row 
                    int colStart, colEnd;

                    colStart = col1 < col2 ? col1 : col2;
                    colEnd = col1 > col2 ? col1 : col2; 
                     
                     
                    var cpRowList = CanvasMatrix.CanvasPointMatrix[row1 - 1];
                    var cpRowListInColRange = cpRowList.Where(cp =>  cp.Col >= colStart - 1 && cp.Col < colEnd);   
                    
                    foreach (var cp in cpRowListInColRange)
                    {  
                        cp.IsPartOfLine = true;
                        cp.NeighborConsoleColor = null;
                        cp.OwnConsoleColor = null;
                    } 

                    for (int i = 0; i < (colEnd - colStart) + 1; i++)
                    {
                        WriteAt("X", colStart + i, cursorRowNr + 3 + row1);
                    }
                }
                else
                { 
                    //Same column

                    int rowStart, rowEnd;

                    rowStart = row1 < row2 ? row1 : row2;
                    rowEnd = row1 > row2 ? row1 : row2;

                    for (int i = rowStart - 1; i < rowEnd; i++)
                    { 
                        var cpRowList = CanvasMatrix.CanvasPointMatrix[i];

                        foreach (var cp in cpRowList)
                        {
                            if (cp.Col == col1 - 1)
                            {  
                               cp.IsPartOfLine = true;
                               cp.NeighborConsoleColor = null;
                               cp.OwnConsoleColor = null;
                            }
                        }
                    }
                      
                    for (int i = rowStart; i < rowEnd + 1; i++)
                    {
                        WriteAt("X", col1,  cursorRowNr + 3 + i);
                    } 
                } 
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DrawCanvas(int internalWidth, int internalHeight, int cursorRowNr)
        {
            try
            { 
                SetCanvasPointMatrix(internalHeight, internalWidth);


                // we first clear the area where the canvas goes
                drawingCommandHelper.SetCursorPosition();

                for (int i = 0; i < drawingCommandHelper.GetConsoleHeight() - (drawingCommandHelper.GetProgramOriginalRow() + 1); i++)
                {
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }

                // Draw the left and right side of rectangle, from top to bottom.
                for (int i = 0; i < internalHeight + 2; i++)
                {
                    if (i == 0)
                    {
                        WriteAt("-", 0, i + cursorRowNr + 3);
                        WriteAt("-", internalWidth + 1, i + cursorRowNr + 3);
                    }
                    else if (i == internalHeight + 1)
                    {
                        WriteAt("-", 0, i + cursorRowNr + 3);
                        WriteAt("-", internalWidth + 1, i + cursorRowNr + 3);
                    }
                    else
                    {
                        WriteAt("|", 0, i + cursorRowNr + 3);
                        WriteAt("|", internalWidth + 1, i + cursorRowNr + 3);
                    }
                }
                
                // Draw top and bottom
                string stringOfDashes = new string('-', internalWidth);
                WriteAt(stringOfDashes, 1, cursorRowNr + 3);
                WriteAt(stringOfDashes, 1, cursorRowNr + 4 + internalHeight);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        private void SetCanvasPointMatrix(int internalHeight, int internalWidth)
        {
            CanvasMatrix = new CanvasMatrix(internalHeight, internalWidth);
            CanvasMatrix.SetNeighborsToEachCanvasPoint(internalHeight, internalWidth); 
            HasCanvas = true;
        }

        private void WriteAt(string s, int colPosition, int rowPosition, ConsoleColor consoleColor = ConsoleColor.Black )
        {
            try
            {
                Console.SetCursorPosition(colPosition, rowPosition);
                Console.BackgroundColor = consoleColor;
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        } 
    } 
}
