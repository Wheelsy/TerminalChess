using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static TerminalChess.Piece;

namespace TerminalChess
{
    internal class AI
    {
        private Dictionary<string, float> rewards;
        private int[] centerBoardBoundary = { 2, 5 };

        public AI()
        {
            // Initialize rewards
            rewards = new Dictionary<string, float>
            {
                { "Piece Capture", 0f }, // Replace 0 with captured piece point value
                { "Check", 2f },
                { "Checkmate", 100f },
                { "No capture center of board", 0.5f },
                { "No capture edge of board", 0f }
            };
        }
        public void GenerateBestMove(GameEngine ge, Board board, Colour colour, Player opponent)
        {
            Utils utils = new();

            var mySquares = board.GetAllSquaresWithPiecesOfOneColour(colour);

            // Generate a list of all possible moves
            List<(Square, (int, int))> allMoves = new();

            foreach (Square square in mySquares)
            {
                if (square.piece != null)
                {
                    List<(int, int)> possibleMoves = square.piece.GetPossibleMoves(square.row, square.col, board);
                    foreach (var move in possibleMoves)
                    {
                        allMoves.Add((square, move));
                    }
                }
            }

            bool foundValidMove = false; // Flag to indicate if a valid move has been found

            // Create a list to store moves along with their rewards
            List<(string, float)> movesWithRewards = new List<(string, float)>();

            // Calculate rewards for all moves and store them in the list
            foreach (var (curSquare, move) in allMoves)
            {
                int toRow = move.Item1;
                int toCol = move.Item2;

                // Create a turn string for the move
                string turn = ParseCoordinateToTurnString(curSquare.row, curSquare.col, toRow, toCol);

                // Calculate the reward for the move
                float reward = CalculateReward(board, ge, curSquare, toCol, toRow, colour, opponent);

                // Add the move and its reward to the list
                movesWithRewards.Add((turn, reward));
            }

            // Sort the list based on reward values in descending order
            movesWithRewards.Sort((x, y) => y.Item2.CompareTo(x.Item2));

            // Iterate through the sorted list and attempt to validate each move
            foreach (var (turn, reward) in movesWithRewards)
            {
                try
                {
                    // Check if the user is intending to castle this turn
                    ge.CheckCastling(turn);

                    // Validate the move
                    ge.ValidateTurn(turn);

                    foundValidMove = true;
                    utils.Print(turn);
                    // Exit the loop since a valid move is found
                    break;
                }
                catch (ChessException e)
                {
                    // Move is invalid, continue to the next move
                    continue;
                }
            }

            // If no valid move was found, handle the situation here (e.g., throw an exception or return a default move)
            if (!foundValidMove)
            {
                throw new Exception("No valid move found.");
            }
        }

        public float CalculateReward(Board board, GameEngine ge, Square curSquare, int toCol, int toRow, Colour colour, Player opponent)
        {
            bool capture = false;
            float reward = 0;

            // Simulate the move
            Square backupCurSquare = new Square(curSquare.row, curSquare.col, curSquare.piece);
            Square newSquare = board.GetSquareAtPos(toRow, toCol);
            Square backupNewSquare = new Square(toRow, toCol, newSquare?.piece);

            // Update the board state
            newSquare.piece = curSquare.piece;
            curSquare.piece = null;

            // If opponent is checkmated immediately return the move
            if (ge.IsOpponentCheckmated())
            {
                return rewards["Checkmate"];
            }

            // Check
            if (ge.IsPlayerInCheck(opponent))
            {
                if (rewards["Check"] > reward)
                {
                    reward = rewards["Check"];
                }
            }

            // Check for captured piece
            if (backupNewSquare.piece != null)
            {
                capture = true;
                if (backupNewSquare.piece.Value >= reward)
                {
                    reward = backupNewSquare.piece.Value;
                }
            }

            // Check center and edge of board moves with no capture
            if (toRow >= centerBoardBoundary[0] && toRow <= centerBoardBoundary[1] && toCol >= centerBoardBoundary[0] && toCol <= centerBoardBoundary[1])
            {
                if (rewards["No capture center of board"] > reward)
                {
                    reward = rewards["No capture center of board"];
                }
            }
            else
            {
                if (rewards["No capture edge of board"] > reward)
                {
                    reward = rewards["No capture edge of board"];
                }
            }

            // Undo the simulated move
            ge.UndoMove(newSquare, curSquare, backupNewSquare, backupCurSquare, capture);

            return reward;
        }

        private string ParseCoordinateToTurnString(int curRow, int curCol, int toRow, int toCol)
        {
            char curColChar = IntColToCharCol(curCol);
            char toColChar = IntColToCharCol(toCol);

            return $"{curColChar}{curRow+1}TO{toColChar}{toRow+1}";
        }

        private char IntColToCharCol(int col)
        {
            char colChar = ' ';

            switch (col)
            {
                case 0:
                    colChar = 'A';
                    break;
                case 1:
                    colChar = 'B';
                    break;
                case 2:
                    colChar = 'C';
                    break;
                case 3:
                    colChar = 'D';
                    break;
                case 4:
                    colChar = 'E';
                    break;
                case 5:
                    colChar = 'F';
                    break;
                case 6:
                    colChar = 'G';
                    break;
                case 7:
                    colChar = 'H';
                    break;
            }
            return colChar;
        }
    }
}
