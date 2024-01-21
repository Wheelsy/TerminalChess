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
             _/|                   |\_
            // -\                  /- \\
            || ._)                (_. ||
            //__\                  /__\\
            )___(  TERMINAL CHESS  )___(

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

        public enum MENU_TYPES {
            MAIN = 0,
            OPTIONS = 1,
            GAME_OVER = 2
        };

        /// <summary>
        /// Prints a message to the console
        /// </summary>
        /// <param name="txt"></param>
        public void Print(string txt)
        {
            Console.WriteLine(txt);
        }

        /// <summary>
        /// Takes and validates a menu input
        /// </summary>
        /// <param name="menuType"></param>
        public void GetMenuSelection(MENU_TYPES menuType)
        {
            string tmp = null;

            // While the users response is invalid repeat the prompt
            while (tmp == null)
            {
                // Get the input
                Console.Write(inputPrompt);
                tmp = Console.ReadLine();

                switch (menuType)
                {
                    case MENU_TYPES.MAIN:
                        // START A NEW GAME
                        if (tmp.Equals("0"))
                        {
                            Console.WriteLine("\nEnter player 1 name:");
                            string p1Name = Console.ReadLine();

                            Console.WriteLine("Enter player 2 name:");
                            string p2Name = Console.ReadLine();

                            Console.WriteLine();

                            Player p1 = new(p1Name);
                            Player p2 = new(p2Name);
                            p1.MyTurn = true;

                            GameEngine ge = new(p1, p2);
                            string view = ge.View();
                            Print(view);
                        }
                        // LOAD A GAME
                        else if (tmp.Equals("1"))
                        {
                        }
                        // EXIT THE APPLICATION
                        else if (tmp.Equals("2"))
                        {
                        }
                        // SHOW THE CREDITS
                        else if (tmp.Equals("3"))
                        {
                        }
                        break;
                    case MENU_TYPES.OPTIONS:
                        // SAVE THE CURRENT GAME
                        if (tmp.Equals("0"))
                        {
                        }
                        // EXIT THE CURRENT GAME
                        else if (tmp.Equals("1"))
                        {
                        }
                        break;
                    case MENU_TYPES.GAME_OVER:
                        // RETURN TO MAIN MENU
                        if (tmp.Equals("0"))
                        {
                        }
                        // EXIT THE APPLICATION
                        else if (tmp.Equals("1"))
                        {
                        }
                        break;
                    // Wrong input. Restart the loop
                    default:
                        Console.WriteLine("Invalid input! Try again:");
                        tmp = null;
                        break;
                }
            }
        }
    }
}
