using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static TerminalChess.Piece;

namespace TerminalChess
{
    internal class AI
    {
        private Dictionary<string, float> rewards;
        private int[] centerBoardBoundary = { 1, 6 };

        public AI()
        {
            // Initialize rewards
            rewards = new Dictionary<string, float>
            {
                { "Piece Capture", 0f }, // Replace 0 with captured piece point value
                { "Check", 2.5f },
                { "Checkmate", 100f },
                { "Escape capture", 1.5f },
                { "No capture center of board", 0.5f },
                { "No capture edge of board", -0.5f }
            };
        }

        /// <summary>
        /// Search through all possible moves to find and execute the best
        /// </summary>
        /// <param name="ge"></param>
        /// <param name="board"></param>
        /// <param name="colour"></param>
        /// <param name="opponent"></param>
        /// <exception cref="Exception"></exception>
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
                    Console.WriteLine(e.Message);
                    // Move is invalid, continue to the next move
                    continue;
                }
            }

            // If no valid move was found, handle the situation here (e.g., throw an exception or return a default move)
            if (!foundValidMove)
            {
                ge.CheckStalemate();
                throw new Exception("No valid move found.");
            }
        }

        /// <summary>
        /// Calculate the rewardd value for a move
        /// </summary>
        /// <param name="board"></param>
        /// <param name="ge"></param>
        /// <param name="curSquare"></param>
        /// <param name="toCol"></param>
        /// <param name="toRow"></param>
        /// <param name="colour"></param>
        /// <param name="opponent"></param>
        /// <returns></returns>
        public float CalculateReward(Board board, GameEngine ge, Square curSquare, int toCol, int toRow, Colour colour, Player opponent)
        {
            float reward = 0;
            int myValue = 0;

            if (curSquare.piece != null)
            {
                myValue = curSquare.piece.Value;
            }
            
            // Check if this piece is currently threatened
            if(CheckForReturnCaptue(curSquare, board, opponent.Colour))
            {
                reward += rewards["Escape capture"];
            }

            // Get the destination square
            Square newSquare = board.GetSquareAtPos(toRow, toCol);

            // If opponent is checkmated immediately return the move
            if (ge.IsOpponentCheckmated())
            {
                return rewards["Checkmate"];
            }

            // Check
            if (ge.IsPlayerInCheck(opponent))
            {

                reward += rewards["Check"];

            }

            // Check for captured piece
            if (newSquare.piece != null)
            {
                // Null the destination piece so a simulation can be run to determine if opponent 
                // pieces can move to the square
                Piece p = newSquare.piece;
                newSquare.piece = curSquare.piece;
                bool canBeReturnCaptured = CheckForReturnCaptue(newSquare, board, opponent.Colour);
                // Place the piece back
                newSquare.piece = p;

                // AI will capture a piece but also be return captured
                if (canBeReturnCaptured)
                {
                    reward += (newSquare.piece.Value - myValue);
                }
                // If there is a free capture return the reward without checking for the center of the board
                else if (!canBeReturnCaptured)
                {
                    reward += newSquare.piece.Value;
                    return reward;
                }
            }
            // Check for AI not capturing a piece but be return captured 
            else
            {
                Piece p = newSquare.piece;
                newSquare.piece = curSquare.piece;
                if (CheckForReturnCaptue(newSquare, board, opponent.Colour))
                {
                    reward -= myValue;
                }
                newSquare.piece = p;
            }

            // Check center and edge of board moves with no capture
            if (toRow >= centerBoardBoundary[0] && toRow <= centerBoardBoundary[1] && toCol >= centerBoardBoundary[0] && toCol <= centerBoardBoundary[1])
            {
                reward += rewards["No capture center of board"];

            }
            else
            {
                reward += rewards["No capture edge of board"];

            }

            return reward;
        }

        /// <summary>
        /// Check if the move will result in a return capture
        /// </summary>
        /// <param name="square"></param>
        /// <param name="board"></param>
        /// <param name="colour"></param>
        /// <returns>True if there is a return capture. Flase if not</returns>
        private bool CheckForReturnCaptue(Square square, Board board, Colour colour)
        {
            // Get the opponent's pieces
            var opponentSquares = board.GetAllSquaresWithPiecesOfOneColour(colour);

            // Iterate over opponent's pieces to check for potential captures
            foreach (var opponentSquare in opponentSquares)
            {
                // Check if the opponent's piece can capture the square
                if (opponentSquare.piece != null && opponentSquare.piece.GetPossibleMoves(opponentSquare.row, opponentSquare.col, board).Contains((square.row, square.col)))
                {
                    // Ignore pawn forward movement as this is not a capture
                    if(opponentSquare.piece is Pawn && opponentSquare.col == square.col)
                    {
                        continue;
                    }

                    // The square can be captured by the opponent
                    return true;
                }
            }

            // No immediate capture by the opponent
            return false;
        }


        /// <summary>
        /// Create the turn string to send to the game engine
        /// </summary>
        /// <param name="curRow"></param>
        /// <param name="curCol"></param>
        /// <param name="toRow"></param>
        /// <param name="toCol"></param>
        /// <returns>Turn string</returns>
        private string ParseCoordinateToTurnString(int curRow, int curCol, int toRow, int toCol)
        {
            char curColChar = IntColToCharCol(curCol);
            char toColChar = IntColToCharCol(toCol);

            return $"{curColChar}{curRow + 1}TO{toColChar}{toRow + 1}";
        }

        /// <summary>
        /// Convert collum characters into int coordinateds
        /// </summary>
        /// <param name="col"></param>
        /// <returns>Character representing a collum</returns>
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
