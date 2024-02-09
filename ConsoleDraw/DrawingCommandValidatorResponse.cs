namespace ConsoleDraw
{
    public class DrawingCommandValidatorResponse
    {
        public string ResponseMessage { get; set; } = string.Empty;
        public string CommandLetter { get; set; } = string.Empty;
        public bool IsValid { get; set; }

        public DrawingCommandValidatorResponse(string responseMessage,
                                               string commandLetter,
                                               bool isValid)
        {
            ResponseMessage = responseMessage;
            CommandLetter = commandLetter;
            IsValid = isValid;
        }
    } 
}
