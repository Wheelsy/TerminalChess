using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Utils
    {
        public string welcome =
            """
             _/|                    |\_
            // -\                   /- \\
            || ._)                 (_. ||
            //__\                   /__\\
            )___(  TERMINAL CHESS   )___(

            """;

        public string mainMenu =
            """
            ----MAIN MENU----
            [0] New game
            [1] Load game
            [2] Exit application
            [3] Credits

            """;

        public string optionsMenu =
            """
            ----OPTIONS----
            [0] Save game
            [1] Exit game

            """;

        public string gameOverMenu =
            """
            ----GAME OVER----
            [0] Menu
            [1] Exit application

            """;

        public string inputPrompt = ">";

        public string goodbye = "Goodbye =]";
    }
}
