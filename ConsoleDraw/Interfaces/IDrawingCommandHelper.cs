namespace ConsoleDraw.Interfaces
{
    public interface IDrawingCommandHelper
    {
        int GetConsoleWidth();
        int GetConsoleHeight();
        int GetProgramOriginalColumn();
        int GetProgramOriginalRow();
        void SetCursorPosition();

    } 
}
