xx

For the definition of what this code is supposed to do, pls see:
RequirementDefinitionForApplication.txt  

Concerning the solution:
It is done in c#.

It more or less solves the requirement. 

I used console color (instead of a character) for bucket paint.
I hope it is OK.  I first did it with a character, but it was not clear what was going on, when there were many lines and rectangles.
At work or for production I would always ask (verify with) the customer first.

Unit Testing:
There are DrawingCommands.cs for the 4 commands, and the DrawingCommandValidator.cs
I unit tested DrawingCommandValidator.cs  Í would likewise do so for the DrawingCommand class, i used though the Console a lot in the DrawingCommand class,
and would have to refactor it a lot with IConsoleManager or something like that that I would then inject in the 
DrawingCommand constructor. But since I did similiar for the DrawingCommandValidator, I hope the
coverage (at least for this exercise) would suffice with mostly testing the Validator.  
If it was production code I would of course unit test completely these two classes.

There are special cases:

- Resizing the screen?  The commands might work a bit.  It is buggy and not supported.  
- The application probably works best on Windows.
- Bucket Paint works on all areas that are not completely closed.  

    For command    B 1 1 Green    - it will not paint area 2, color Green
    ----------------------
    |             xxxxx  |
    |xxxxxx       x 2 x  |
    |     x       xxxxx  |
    |     x              |
    ----------------------
     
    For command    B 1 1 Green    - it will paint area 2, color Green (because it is not completely closed, a diagonal leads to the area)
    ----------------------
    |             xxxxx  |
    |xxxxxx       x 2 x  |
    |     x        xxxx  |
    |     x              |
    ----------------------

    This is because the Line command cannot be used for creating diagonals, I thought then they cannot likewise
    create an area closure.
     
- The Bucket Paint actually uses console color 
- The command line is a bit shaky (you can use up and down arrow key to get last commands (it just worked out of the box))
  but you sometimes have to use the right / left arrow keys to place the cursor on the character after the command and press enter.
- The Canvas commands works best for a Canvas of max height 18 and max Width 118
- There are different validations in the program.  Bucket Paint on an existing line, for example, will return error.
  And there are other validations.  You will see them when running the program.

  To run the program:

  You can run the program from Visual Studio or via the command line from the location you unzipped the solution, and at 
  and type (something like this):
   
  C:\<location of solution>\ConsoleDraw\ConsoleDraw\bin\Release\net6.0>ConsoleDraw.exe

  Valdation:

  I do a lot of validation, and return validation messages just beneath the "Enter command here:" prompt.
  The validation is useful for useability and finding a solution.

  Generally:  

  There are probably many ways to do a console paint app this is just one version - mostly for recruiters and demo purposes.


 


   
