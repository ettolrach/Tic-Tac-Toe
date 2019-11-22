﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tic_Tac_Toe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare initial variables.

            // Declare the symbols used to represent players 1 and 2.
            const string player1Symbol = "X";
            const string player2Symbol = "O";
            // Make the width and height variables, and the variables to check for a win.
            int width, height, winNumber, winner = 0;
            // Declare a player variable which keeps track of whose turn it is.


            Console.WriteLine("\n\n=TIC-TAC-TOE=");

            // Get the width and height of the board.
            while (true)
            {
                try
                {
                    Console.WriteLine("Please enter the width of the board: ");
                    width = int.Parse(Console.ReadLine());
                    Console.WriteLine("Please enter the height of the board: ");
                    height = int.Parse(Console.ReadLine());
                    Console.WriteLine("Please enter the symbols in a row to win: ");
                    winNumber = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("That is not a number. Please enter a natural number.");
                }
            }

            List<string> board = MakeBoard(width, height);
            // Work out what the possible maximum moves are (which is the area).
            int maxMoves = width * height;


            // Continuously loop until there cannot possibly be any moves left.
            for (int currentMoveNumber = 0; currentMoveNumber < maxMoves / 2; currentMoveNumber++)
            {
                Console.WriteLine(DrawBoard(board, width, height));
                MakeMove(board, width, height, 1, player1Symbol);
                if (CheckForWin(board, width, height, winNumber) == true)
                {
                    winner = 1;
                    break;
                }

                Console.WriteLine(DrawBoard(board, width, height));
                MakeMove(board, width, height, 2, player2Symbol);
                if (CheckForWin(board, width, height, winNumber) == true)
                {
                    winner = 2;
                    break;
                }
            }

            // If the board's area is odd, then player 1 will go last, and maxMoves / 2 will have remainder 1. That's why the above for loop won't cover for the last move. Also, the and checks if someone hasn't won already.
            if (maxMoves % 2 == 1 && winner == 0)
            {
                Console.WriteLine(DrawBoard(board, width, height));
                MakeMove(board, width, height, 1, player1Symbol);
                if (CheckForWin(board, width, height, winNumber) == true)
                {
                    winner = 1;
                }
            }

            switch (winner)
            {
                case 1:
                    Console.WriteLine(DrawBoard(board, width, height));
                    Console.WriteLine("Player 1 wins!");
                    break;
                case 2:
                    Console.WriteLine(DrawBoard(board, width, height));
                    Console.WriteLine("Player 2 wins!");
                    break;
                default:
                    Console.WriteLine(DrawBoard(board, width, height));
                    Console.WriteLine("It's a draw.");
                    break;
            }
        }

        static int CheckForValidMove(System.ConsoleKeyInfo input, int maxNum)
        {
            int toReturn;
            while (true)
            {
                // Check if the key pressed is a digit.
                if (char.IsDigit(input.KeyChar))
                {
                    // Try and convert it to an int.
                    toReturn = int.Parse(input.KeyChar.ToString());
                    // If it isn't between the right numbers, then continue onwards and ask for a new one.
                    if (toReturn >= 0 && toReturn <= maxNum - 1)
                    {
                        break;
                    }
                    Console.WriteLine($"That is not a valid move. Please enter a number between 0 and {maxNum - 1}: ");
                    input = Console.ReadKey();
                }
                else
                {
                    // It isn't, so it definitely isn't a right move.
                    Console.WriteLine($"That is not a valid move. Please enter a number between 0 and {maxNum - 1}: ");
                    input = Console.ReadKey();
                }
            }

            return toReturn;
        }

        static bool CheckForWin(List<string> boardToCheck, int width, int height, int winNumber)
        {
            for (int currentSpaceNumber = 0; currentSpaceNumber < boardToCheck.Count; currentSpaceNumber++)
            {
                string currentState = boardToCheck[currentSpaceNumber];

                // If the state is a space, no-one could have won.
                if (currentState == " ")
                {
                    continue;
                }

                // Get the (x,y) coordinate of the current space that is being checked.
                int x = currentSpaceNumber % width,
                    y = currentSpaceNumber / width;

                // Check left-right.
                // All of these methods take the same sort of structure.
                // Check whether a win through horizontal in a row is even possible.
                if ((x + winNumber) <= width)
                {
                    // Check the next spaces up to the winNumber. If it's any longer, then there's no point in continuing to check. Getting 4-in-a-row if you only need 3 doesn't get you extra points.
                    for (int i = 0; i < winNumber; i++)
                    {
                        // If the space that's to the left is the same as the current one, break the loop. If it is, then continue to the next one.
                        if (boardToCheck[x + i] != currentState)
                        {
                            break;
                        }
                        // Once you've reached winNumber, then return true, as you've won.
                        if (i == winNumber - 1)
                        {
                            return true;
                        }
                    }
                }

                // Check up-down. Same structure as above.
                if ((y + winNumber) <= height)
                {
                    for (int i = 1; i < winNumber; i++)
                    {
                        // If the space that's to the left is the same as the current one, break the loop. If it is, then continue to the next one.
                        if (boardToCheck[x + width * (y + i)] != currentState)
                        {
                            break;
                        }
                        // Once you've reached winNumber, then return true, as you've won.
                        if (i == winNumber - 1)
                        {
                            return true;
                        }
                    }
                }

                // Check diagonal up-right.
                // Firstly, check if the selected space can even have winNumber in a row. For example, in Tic-Tac-Toe, only (0,2) can have a diagonal up-right win.
                if ((x + winNumber) <= width && (y + 1 - winNumber) >= 0)
                {
                    // Repeat for the number of necessary spaces you need to have in a row to win (winNumber). Doing more would be useless.
                    for (int i = 1; i < winNumber; i++)
                    {
                        // Check if up and to the right is the same as the current one. If it isn't, then stop checking (as you cannot win).
                        if (boardToCheck[(x + i) + width * (y - i)] != currentState)
                        {
                            break;
                        }
                        // If you've checked them all, return true.
                        if (i == winNumber - 1)
                        {
                            return true;
                        }
                    }
                }

                // Check diagonal down-right. Same algorithm as above, just adjusted to go in another direction.
                if ((x + winNumber) <= width && (y + winNumber) <= height)
                {
                    for (int i = 0; i < winNumber; i++)
                    {
                        if (boardToCheck[(x + i) + width * (y + i)] != currentState)
                        {
                            break;
                        }
                        if (i == winNumber - 1)
                        {
                            return true;
                        }
                    }
                }
            }

            // If nothing's been returned yet, then it must be false.
            return false;
        }

        static string DrawBoard(List<string> boardToDraw, int width, int height)
        {
            StringBuilder toReturn = new StringBuilder("\n+");
            // Add the top border. For Tic-Tac-Toe, this will be "+---+---+---+" (the initial + was added on the line above).
            for (int x = 0; x < width; x++)
            {
                toReturn.Append("---+");
            }
            toReturn.Append("\n|");

            for (int y = 0; y < height; y++)
            {
                // This will add "|   |   |   |" etc. for how long the width says to do so (again, the initial | was added above).
                for (int x = 0; x < width; x++)
                {
                    toReturn.Append("   |");
                }
                toReturn.Append("\n|");

                // Now write in whatever the spaces are, e.g. "| X |   | O |".
                for (int x = 0; x < width; x++)
                {
                    // This write the actual state. x+width*y will return whatever is at the current x,y position.
                    toReturn.Append($" {boardToDraw[(x + width * y)]} |");
                }
                toReturn.Append("\n|");

                // Finally, add "|   |   |   |" again.
                for (int x = 0; x < width; x++)
                {
                    toReturn.Append("   |");
                }
                toReturn.Append("\n+");

                // Add the top border for the next row.
                for (int x = 0; x < width; x++)
                {
                    toReturn.Append("---+");
                }

                // If it's not the last loop, then append a | for the next row.
                if (y < height - 1)
                {
                    toReturn.Append("\n|");
                }
                // Otherwise, append a new line control character.
                else
                {
                    toReturn.Append("\n");
                }
            }

            return toReturn.ToString();
        }

        static List<string> MakeBoard(int width, int height)
        {
            List<string> createdBoard = new List<string>();

            // Fill the board with empty spaces.
            for (int i = 0; i < width * height; i++)
            {
                createdBoard.Add(" ");
            }

            return createdBoard;
        }

        static List<string> MakeMove(List<string> board, int width, int height, int currentPlayer, string moveSymbol)
        {
            List<string> newBoard = board;
            // Take the player's move coordinates.
            int moveX, moveY;
            // Save user input.
            System.ConsoleKeyInfo rawInput;

            Console.WriteLine($"\nPlease enter an x-coordinate, player {currentPlayer}: ");
            rawInput = Console.ReadKey();
            moveX = CheckForValidMove(rawInput, width);

            Console.WriteLine($"\nPlease enter a y-coordinate, player {currentPlayer}: ");
            rawInput = Console.ReadKey();
            moveY = CheckForValidMove(rawInput, height);

            int spaceToPlace = moveX + width * moveY;

            newBoard[spaceToPlace] = moveSymbol;

            return newBoard;
        }

    }
}
