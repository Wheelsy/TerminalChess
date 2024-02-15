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
            [1] Exit application
            [2] Credits

            """;

        public string credits = """

            Created by Ely Hawkins
            Email: elyh117@gmail.com
            
            """;

        public string optionsMenu =
            """
            ----OPTIONS----
            [0] Draw
            [1] Concede
            [2] Back
            [3] Exit game

            """;

        public string promotion =
            """
             N   B   R   Q
            [0] [1] [2] [3]
            """;

        public string inputPrompt = ">";

        public string goodbye = "\nGoodbye =]";

        public enum MENU_TYPES {
            MAIN = 0,
            OPTIONS = 1,
            PROMOTION = 2
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
        public string GetMenuSelection(MENU_TYPES menuType)
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
                        if(tmp.Equals("0") || tmp.Equals("1") || tmp.Equals("2"))
                        {
                            return tmp;
                        }
                        break;
                    case MENU_TYPES.OPTIONS:
                        // Return the options option selected
                        if (tmp.Equals("0") || tmp.Equals("1") || tmp.Equals("2") || tmp.Equals("3"))
                        {
                            return tmp;
                        }
                        break;
                    case MENU_TYPES.PROMOTION:
                        // Return the promotion option selected
                        if (tmp.Equals("0") || tmp.Equals("1") || tmp.Equals("2") || tmp.Equals("3"))
                        {
                            return tmp;
                        }
                        break;
                    // Wrong input. Restart the loop
                    default:
                        tmp = null;
                        break;
                }
                Print("Invalid input! Try again:");
            }
            return null;
        }

        public string GetInput()
        {
            Console.Write(inputPrompt);
            string tmp = Console.ReadLine();
            return tmp;
        }
    }
}
