using ConsoleDraw.Interfaces;

namespace ConsoleDraw
{

    public class CanvasMatrix : ICanvasMatrix
    {
        public CanvasPoint[][] CanvasPointMatrix { get; set; } = new CanvasPoint[1][];
        public CanvasMatrix(int internalHeight, int internalWidth)
        {
            CanvasPointMatrix = Enumerable.Range(0, internalHeight).Select((row) => Enumerable.Range(0, internalWidth).Select((col) => new CanvasPoint(row, col)).ToArray()).ToArray(); 
        }

        public void SetNeighborColorChar(int row, int col, ConsoleColor consoleColor)
        {
            CanvasPointMatrix[row][col].CanvasPointNeighbors.ForEach(nCP => {
                if (!nCP.IsPartOfLine)
                {
                    if (nCP.Col != -1 && nCP.Row != -1 && !nCP.IsPartOfLine)
                    {
                        nCP.NeighborConsoleColor = consoleColor;
                    } 
                }
            }); 
        }

        public void SetNeighborsToEachCanvasPoint(int internalHeight, int internalWidth)
        {    
            // for each row
            for (int i = 0; i < CanvasPointMatrix.Length; i++)
            {
                // items in each row
                for (int j = 0; j < CanvasPointMatrix[i].Length; j++)
                {
                    var eastCpm = j + 1 < internalWidth ? CanvasPointMatrix[i][j + 1] : new CanvasPoint(-1, -1);
                    var southCpm = i + 1 < internalHeight ? CanvasPointMatrix[i + 1][j] : new CanvasPoint(-1, -1);
                    var westCpm = j - 1 > -1 ? CanvasPointMatrix[i][j - 1] : new CanvasPoint(-1, -1);
                    var northCpm = i - 1 > -1 ? CanvasPointMatrix[i - 1][j] : new CanvasPoint(-1, -1); 
                    var northWestCpm = j - 1 > -1 && i - 1 > -1 ? CanvasPointMatrix[i - 1][j - 1] : new CanvasPoint(-1, -1);
                    var northEastCpm = j + 1 < internalWidth && i - 1 > -1 ? CanvasPointMatrix[i - 1][j + 1] : new CanvasPoint(-1, -1); 
                    var southWestCpm = i + 1 < internalHeight && j - 1 > -1 ? CanvasPointMatrix[i + 1][j - 1] : new CanvasPoint(-1, -1); 
                    var southEastCpm = i + 1 < internalHeight && j + 1 < internalWidth ? CanvasPointMatrix[i + 1][j + 1] : new CanvasPoint(-1, -1);

                    var refCpm = CanvasPointMatrix[i][j]; 
                     
                    refCpm.CanvasPointNeighbors.Add(eastCpm);
                    refCpm.CanvasPointNeighbors.Add(southCpm);
                    refCpm.CanvasPointNeighbors.Add(westCpm);
                    refCpm.CanvasPointNeighbors.Add(northCpm);
                    refCpm.CanvasPointNeighbors.Add(northWestCpm);
                    refCpm.CanvasPointNeighbors.Add(northEastCpm);
                    refCpm.CanvasPointNeighbors.Add(southWestCpm);
                    refCpm.CanvasPointNeighbors.Add(southEastCpm); 
                }
            } 
        } 
    }
}
