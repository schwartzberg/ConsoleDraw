using NUnit.Framework;
using FluentAssertions;
using ConsoleDraw;
using NSubstitute;
using System.Linq;
using ConsoleDraw.Interfaces;

namespace ConsoleDrawTest
{
    [TestFixture]
    public class DrawingCommandTest
    {
     
        IDrawingCommands? drawingCommands;
        const int cursorRowNr = 9;

        [SetUp]
        public void Setup()
        { 
            drawingCommands = new DrawingCommands(new ConsoleDrawTest.DrawingCommandHelper()); 
        }

        // I would need to implement a console manager for the Console.Write and Console.WriteLine(s) that are using
        // in the DrawingCommand Class.

        //[Test]
        //public void CanvasCommandOK()
        //{
        //    string command = "C 50 15";
        //    var inputs = command.Split(" ");
        //    var isOK = drawingCommands?.ExecuteCommand(inputs, cursorRowNr);
        //    isOK?.Should().BeTrue();
             
        //    Assert.Pass();
        //} 
        
    }
}