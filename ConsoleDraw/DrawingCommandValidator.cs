using ConsoleDraw.Interfaces;

namespace ConsoleDraw
{
    public class DrawingCommandValidator
    {
        protected static string[] commandLetters = new string[] { "C", "L", "R", "B", "Q" };
        private IDrawingCommandValidatorHelper drawingCommandValidatorHelper;
        private IDrawingCommands drawingCommands;
        public DrawingCommandValidator(IDrawingCommandValidatorHelper _drawingCommandValidatorHelper,
                                       IDrawingCommands _drawingCommands)
        {
            drawingCommandValidatorHelper = _drawingCommandValidatorHelper;
            drawingCommands = _drawingCommands;
        }
        public DrawingCommandValidatorResponse Validate(string input)
        {
            try
            {
                var inputs = input.Split(' ');

                if (!inputs.Any())
                {
                    return new DrawingCommandValidatorResponse
                        ("No Input", string.Empty, false);
                }
                  
                if (!(inputs.First() is String))
                {
                    return new DrawingCommandValidatorResponse
                        ("Bummer, first argument '{inputs.First()}' needs to be a string command.", string.Empty, false);
                }

                if (inputs.Any(input => string.IsNullOrEmpty(input)))
                {
                    return new DrawingCommandValidatorResponse
                        ("An input is null or empty", inputs.First(), false);
                }

                DrawingCommandValidatorResponse validationState = CheckCommand(inputs, input);


                if (!validationState.IsValid)
                {
                    return validationState;
                }

                var validateArgumentSize = CheckArgumentSize(inputs);

                if (validateArgumentSize.IsValid)
                {
                    return new DrawingCommandValidatorResponse(input, inputs.First(), true);
                }
                else
                {
                    return validateArgumentSize;
                }
            }
            catch (Exception ex)
            {
                Console.SetCursorPosition(Program.origCol, Program.origRow + 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(Program.origCol, Program.origRow + 1);
                var message = $"Validation exception: {ex.Message}";
                Console.Write(message);
                return new DrawingCommandValidatorResponse(ex.Message, input, false);
            } 
        }

        private DrawingCommandValidatorResponse CheckCommand(string[] inputs, string orginalInput)
        {     
            var commandLetter = inputs.First();
            if (inputs.First() != "C" && !drawingCommands.HasCanvas)
                return new DrawingCommandValidatorResponse
                  ($"Bummer, canvas must be ready before other commands.", commandLetter, false);

            switch (commandLetter)
            {
                case "C":
                    return CheckIndividualCommand(inputs, "C", 2, orginalInput); 
                case "L":
                    return CheckIndividualCommand(inputs, "L", 4, orginalInput); 
                case "R":
                    return CheckIndividualCommand(inputs, "R", 4, orginalInput); 
                case "B":
                    return CheckBucketCommand(inputs, "B", 3, 2, orginalInput); 
                case "Q":
                    return CheckIndividualCommand(inputs, "Q", 0, orginalInput);  
                default:
                    var stringOfCommandLetters = string.Join("','", commandLetters);
                    return new DrawingCommandValidatorResponse
                        ($"Bummer, '{inputs.First()}' is not a command, must be one of '{stringOfCommandLetters}'.", string.Empty, false);
            }
        } 
         
        private DrawingCommandValidatorResponse CheckIndividualCommand(string[] inputs, string commandLetter, int numberOfArguments, string orginalInput)
        {
            RemoveExtraInputArgument(inputs, numberOfArguments);

            if (inputs.Length != numberOfArguments + 1)
                return new DrawingCommandValidatorResponse($"Bummer, command '{commandLetter}' must have {numberOfArguments} numeric arguments", commandLetter, false);
            var arguments = inputs.ToList();
            arguments.RemoveAt(0);
            arguments.ForEach(arg => arg.Trim()); 
            var argumentsAreNumeric = arguments.All(i => i.ToCharArray().All(g => Char.IsDigit(g)));
            if (!argumentsAreNumeric)
                return new DrawingCommandValidatorResponse($"Bummer, arguments are not numeric", commandLetter, false);
            return new DrawingCommandValidatorResponse(orginalInput, commandLetter, true);
        }
     
        private DrawingCommandValidatorResponse CheckBucketCommand(string[] inputs, string commandLetter, int numberOfArguments, int numericArgNr, string orginalInput)
        {
            RemoveExtraInputArgument(inputs, numberOfArguments); 

            if (inputs.Length != numberOfArguments + 1)
                return new DrawingCommandValidatorResponse($"Bummer, command '{commandLetter}' must have {numberOfArguments}  arguments", commandLetter, false);
            var arguments = inputs.ToList(); 
             
            arguments.RemoveAt(3);
            arguments.RemoveAt(0);
            arguments.ForEach(arg => arg.Trim());
            var argumentsAreNumeric = arguments.All(i => i.ToCharArray().All(g => Char.IsDigit(g)));
            if (!argumentsAreNumeric)
                return new DrawingCommandValidatorResponse($"Bummer, arguments are not numeric", commandLetter, false); 
         
            return new DrawingCommandValidatorResponse(orginalInput, commandLetter, true);
        }

        private void RemoveExtraInputArgument(string[] inputs, int numberOfArguments)
        {
            if (inputs.Length > numberOfArguments + 1)
            {
                for (int i = inputs.Length - 1; i >= numberOfArguments + 1; i--)
                {   
                    inputs = inputs.Where((val, idx) => idx != i).ToArray();
                }
            } 
        }

        private DrawingCommandValidatorResponse CheckArgumentSize(string[] inputs)
        {
            switch (inputs.First())
            {
                case "C":
                    return CheckCanvasSize(inputs, inputs.First());
                case "L":
                    return CheckLine(inputs, inputs.First()); 
                case "R":
                     return CheckSquare(inputs, inputs.First());
                case "B":
                    return CheckBucketPaint(inputs, inputs.First()); 
                case "Q":
                   
                default:
                    return new DrawingCommandValidatorResponse(string.Empty, inputs.First(), true);
            }
        }

        private DrawingCommandValidatorResponse CheckBucketPaint(string[] inputs, string commandLetter)
        {
            var col1 = Convert.ToInt32(inputs.ToList()[1]);
            var row1 = Convert.ToInt32(inputs.ToList()[2]);
            var fillerString = inputs.ToList()[3]; 

            if (col1 < 1 || col1 > drawingCommands.InternalCanvasWidth ||
             row1 < 1 || row1 > drawingCommands.InternalCanvasHeight)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, chosen point to bucket paint is not in canvas.", commandLetter, false);
            }

            var colorString = inputs[3]; 
            if (string.IsNullOrEmpty(colorString))
            {
                return new DrawingCommandValidatorResponse($"Bummer, color/char is null or empty", commandLetter, false);
            } 
           
            ConsoleColor validConsoleColor; 
            if (!(Enum.TryParse<ConsoleColor>(colorString, out validConsoleColor)))
            {
                return new DrawingCommandValidatorResponse($"Bummer, only valid ConsoleColor supported, eg 'Green', 'Red', 'Yellow', 'Blue', 'White'... ", commandLetter, false);
            }

            var isPartOfLine = drawingCommands.CanvasMatrix.CanvasPointMatrix[row1 - 1][col1 - 1].IsPartOfLine;
            if (isPartOfLine)
                return new DrawingCommandValidatorResponse($"Bummer, Bucket Paint on existing line is not possible.", commandLetter, false);

            return new DrawingCommandValidatorResponse(string.Empty, commandLetter, true); 
        }
         
        private DrawingCommandValidatorResponse CheckSquare(string[] inputs, string commandLetter)
        {
            var col1 = Convert.ToInt32(inputs.ToList()[1]);
            var row1 = Convert.ToInt32(inputs.ToList()[2]);
            var col2 = Convert.ToInt32(inputs.ToList()[3]);
            var row2 = Convert.ToInt32(inputs.ToList()[4]);

            // A square or rectangle is made up of 4 lines
            (int, int, int, int) topLine = (col1, row1, col2, row1);
            (int, int, int, int) leftLine = (col1, row1, col1, row2);
            (int, int, int, int) rightLine = (col2, row1, col2, row2);
            (int, int, int, int) bottomLine = (col1, row2, col2, row2);

            (int, int, int, int)[] squareLines = new[] { topLine , leftLine, rightLine, bottomLine};
           
            if (col1 == col2 && row1 == row2)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, its point and not a square, and not supported.", commandLetter, false);
            }

            foreach (var sqline in squareLines)
            {
                var lineIsInCanvasValidation = CheckIfLineIsInCanvas(sqline.Item1,
                                                                     sqline.Item2,
                                                                     sqline.Item3,
                                                                     sqline.Item4, 
                                                                     commandLetter,
                                                                     "square");

                if (!lineIsInCanvasValidation.IsValid)
                {
                    return lineIsInCanvasValidation;
                }
            }
            
            return new DrawingCommandValidatorResponse(string.Empty, commandLetter, true);
        }


        private DrawingCommandValidatorResponse CheckLine(string[] inputs, string commandLetter)
        {
            var col1 = Convert.ToInt32(inputs.ToList()[1]);
            var row1 = Convert.ToInt32(inputs.ToList()[2]);
            var col2 = Convert.ToInt32(inputs.ToList()[3]);
            var row2 = Convert.ToInt32(inputs.ToList()[4]);

            if (col1 == col2 && row1 == row2)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, its point and not a line, and not supported.", commandLetter, false);
            }

            if (col1 != col2 && row1 != row2)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, diagnoal lines not (yet) supported.", commandLetter, false);
            }

            var lineIsInCanvasValidation = CheckIfLineIsInCanvas(col1, row1, col2, row2, commandLetter);
           
            if (lineIsInCanvasValidation.IsValid)
            {
                return new DrawingCommandValidatorResponse(string.Empty, commandLetter, true);
            }

            return lineIsInCanvasValidation;
        }

        private DrawingCommandValidatorResponse CheckIfLineIsInCanvas(int col1, int row1, 
                                                                int col2, int row2, 
                                                                string commandLetter,
                                                                string formName = "line")
        {
            if (col1 < 1 || col2 > drawingCommands.InternalCanvasWidth ||
                    row1 < 1 || row2 > drawingCommands.InternalCanvasHeight)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, entire {formName} is not in canvas.", commandLetter, false);
            }

            return new DrawingCommandValidatorResponse(string.Empty, commandLetter, true);
        }

        private DrawingCommandValidatorResponse CheckCanvasSize(string[] inputs, string commandLetter)
        {
            var windowWidth = drawingCommandValidatorHelper.GetConsoleWidth();
            var windowHeight = drawingCommandValidatorHelper.GetConsoleHeight();

            var canvasInternalWidth = Convert.ToInt32(inputs.ToList()[1]);
            var canvasInternalHeight = Convert.ToInt32(inputs.ToList()[2]);

            if (canvasInternalWidth > windowWidth - 2)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, canvas width seems to big '{inputs[1]}', max width is {windowWidth - 2}.", "C", false);
            }

            if (canvasInternalHeight > windowHeight - 12)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, canvas height seems to big '{inputs[2]}', max height is {windowHeight - 12}.", "C", false);
            }

            if (canvasInternalWidth < 4)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, canvas width too small, size 4 x 4 is minimum.", "C", false);
            }

            if (canvasInternalHeight < 4)
            {
                return new DrawingCommandValidatorResponse
                    ($"Bummer, canvas height too small, size 4 x 4 is minimum.", "C", false);
            }

            return new DrawingCommandValidatorResponse(string.Empty, commandLetter, true);
        }
    } 
}
