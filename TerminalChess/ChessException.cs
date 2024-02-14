using System;

namespace TerminalChess
{
    [Serializable]
    public class ChessException : Exception
    {
        public enum CHESS_EXCEPTION_TYPE
        {
            INVALID_MOVE,
            END_IN_CHECK,
            CASTLE_THROUGH_CHECK,
            OPPONENTS_PIECE,
            NO_PIECE
        }

        private static readonly string[] messages = {
            "That is not a valid move!",
            "You cannot end your turn in check!",
            "You cannot castle through check!",
            "You cannot move your opponent's piece!",
            "The square you have selected does not contain a piece!"
        };

        public ChessException(CHESS_EXCEPTION_TYPE type) : base(messages[(int)type])
        {
        }

        public ChessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

