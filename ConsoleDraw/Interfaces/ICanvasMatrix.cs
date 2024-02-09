namespace ConsoleDraw.Interfaces
{
    public interface ICanvasMatrix
    {
        CanvasPoint[][] CanvasPointMatrix { get; set; }
        void SetNeighborColorChar(int row, int col, ConsoleColor consoleColor);
        void SetNeighborsToEachCanvasPoint(int internalHeight, int internalWidth);
    }
}