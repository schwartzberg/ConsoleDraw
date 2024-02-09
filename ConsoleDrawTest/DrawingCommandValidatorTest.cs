using NUnit.Framework;
using FluentAssertions;
using ConsoleDraw;
using NSubstitute;
using System.Linq;
using ConsoleDraw.Interfaces;

namespace ConsoleDrawTest
{
    [TestFixture]
    public class DrawingCommandValidatorTest
    {
        DrawingCommandValidator? drawingCommandValidator;
        IDrawingCommands? drawingCommands;
        [SetUp]
        public void Setup()
        {
            var helper = new ConsoleDrawTest.DrawingCommandValidatorHelper();
            drawingCommands = Substitute.For<IDrawingCommands>();
            drawingCommandValidator = new DrawingCommandValidator(helper, drawingCommands);
        }

        [Test]
        public void CanvasCommandOK()
        {
            string command = "C 50 15";
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeTrue();
            validationResponse?.ResponseMessage.Should().Be(command);
            Assert.Pass();
        }

        [Test]
        public void CanvasCommand_Wrong_Letter_NotOK()
        {
            string command = "c 50 15";
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("Bummer, canvas must be ready before other commands."); 
        }

        [Test]
        public void CanvasCommand_Argument_Not_Numeric_NotOK()
        {
            string command = "C aa 15";
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("Bummer, arguments are not numeric");
        }

        [Test]
        public void CanvasCommand_Argument_Lacking_NotOK()
        {
            string command = "C 15 ";
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("An input is null or empty");
        }

        [Test]
        public void CanvasCommand_Argument_To_Big_NotOK()
        {
            string command = "C 120 15";
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("Bummer, canvas width seems to big '120', max width is 118.");
        }

        [Test]
        public void CanvasCommand_2nd_Argument_To_Big_NotOK()
        {
            string command = "C 118 20";
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("Bummer, canvas height seems to big '20', max height is 18.");
        }

        [Test]
        public void LineCommand_OK()
        {
            string command = "L 2 12 24 12";
            drawingCommands?.HasCanvas.Returns(true);
            drawingCommands?.InternalCanvasHeight.Returns(12);
            drawingCommands?.InternalCanvasWidth.Returns(24);
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeTrue();
            validationResponse?.ResponseMessage.Should().Be(command);
        }

        [TestCase("L 2 2 25 2")]
        [TestCase("L 2 13 24 13")]
        public void LineCommand_LineNotInCanvas_NotOK(string _command)
        {
            string command = _command;
            drawingCommands?.HasCanvas.Returns(true);
            drawingCommands?.InternalCanvasHeight.Returns(12);
            drawingCommands?.InternalCanvasWidth.Returns(24);
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("Bummer, entire line is not in canvas.");
        }

        [TestCase("L 2 2 24 3")]
        [TestCase("L 2 11 24 12")]
        public void LineCommand_LineDiagonal_NotOK(string _command)
        {
            string command = _command;
            drawingCommands?.HasCanvas.Returns(true);
            drawingCommands?.InternalCanvasHeight.Returns(12);
            drawingCommands?.InternalCanvasWidth.Returns(24);
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeFalse();
            validationResponse?.ResponseMessage.Should().Be("Bummer, diagnoal lines not (yet) supported.");
        }

        [Test]
        public void RectangleCommand_OK()
        {
            string command = "R 3 3 20 10";
            drawingCommands?.HasCanvas.Returns(true);
            drawingCommands?.InternalCanvasHeight.Returns(12);
            drawingCommands?.InternalCanvasWidth.Returns(30);
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeTrue();
            validationResponse?.ResponseMessage.Should().Be(command);
        }

        [Test]
        public void BucketPaintCommand_OK()
        {
            string command = "B 3 3 Green";
            drawingCommands?.HasCanvas.Returns(true);
            const int rows = 12;
            const int columns = 30;
            drawingCommands?.InternalCanvasHeight.Returns(rows);
            drawingCommands?.InternalCanvasWidth.Returns(columns);
            var canvasPointMatrix = Enumerable.Range(0, rows).Select((row) => Enumerable.Range(0, columns).Select((col) => new CanvasPoint(row, col)).ToArray()).ToArray();

            ICanvasMatrix canvasMatrix = Substitute.For<ICanvasMatrix>();
            canvasMatrix.CanvasPointMatrix.Returns(canvasPointMatrix);
            drawingCommands?.CanvasMatrix.Returns(canvasMatrix); 
            
            var validationResponse = drawingCommandValidator?.Validate(command);
            validationResponse?.IsValid.Should().BeTrue();
            validationResponse?.ResponseMessage.Should().Be(command);
        }
    }
}