namespace ConsoleDraw.Interfaces
{
    public interface IDrawingCommands
    {
        int InternalCanvasHeight { get; set; } 
        int InternalCanvasWidth { get; set; }  
        bool HasCanvas { get; set; }
        ICanvasMatrix CanvasMatrix { get; set; }
        void ShowCommands();
        bool ExecuteCommand(string[] inputs, int cursorRowNr);  
    }
}