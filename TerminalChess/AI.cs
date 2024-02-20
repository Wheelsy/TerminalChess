using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class AI
    {
        private Dictionary<string, float> rewards;

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

        public string GenerateBestMove(GameEngine ge)
        {
            return "";
        }
    }
}
