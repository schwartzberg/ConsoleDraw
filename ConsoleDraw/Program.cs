// See https://aka.ms/new-console-template for more information

using System;

namespace ConsoleDraw
{
    class Program
    {
        public static int origRow;
        public static int origCol;

        static void Main(string[] args)
        {
            try
            {
                origRow = Console.CursorTop;
                origCol = Console.CursorLeft;

                bool endApp = false;
                var drawingCommands = new DrawingCommands(new DrawingCommandHelper());
                var drawingCommandValidator = new DrawingCommandValidator(new DrawingCommandValidatorHelper(), drawingCommands);

                drawingCommands.ShowCommands();

                origRow = Console.CursorTop;
                origCol = Console.CursorLeft;

                while (!endApp)
                {
                    string input = "";
                    Console.SetCursorPosition(origCol, origRow);
                    Console.Write("Enter your command here: ");
                    input = Console.ReadLine() ?? "Unknown ínput";
                    var inputs = input.Split(' ');
                    inputs.ToList().ForEach(x => x.Trim());
                    var validationResult = drawingCommandValidator.Validate(input);

                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(origCol, origRow + 1);

                    if (validationResult.IsValid)
                    {
                        Console.Write($"You entered: {validationResult.ResponseMessage}");
                    }
                    else
                    {
                        Console.Write($"{validationResult.ResponseMessage}");
                    }

                    if (input == "Q")
                    {
                        endApp = true;
                    }
                    else
                    {

                        if (validationResult.IsValid)
                        { 
                            // Execute Command 
                            var isCommandOK = drawingCommands.ExecuteCommand(inputs, origRow);
                            Console.BackgroundColor = ConsoleColor.Black;

                            if (isCommandOK)
                            {
                                Console.SetCursorPosition(origCol, origRow);
                                Console.Write(new string(' ', Console.WindowWidth));
                            }

                            // in case user between commands changes size of console window
                            if (OperatingSystem.IsWindows())
                            {
                                Console.SetWindowSize(120, 30);
                            }
                            else
                            {
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(origCol, origRow + 1);
                                Console.Write("This application works best on windows at the moment.");
                            }

                            Console.SetCursorPosition(0, 0);
                        }
                    }
                }

                Console.WriteLine(String.Empty);
                System.Environment.Exit(1);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, the program is closing.");
                Console.WriteLine(ex);
            } 
        } 
    }
}

