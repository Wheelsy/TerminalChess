using System.Xml.Linq;
using TerminalChess;

Utils utils = new();
GameEngine ge;

// Print terminal chess title
utils.Print(utils.welcome);

string mainMenuSelection = "";

while (mainMenuSelection != "1")
{
    // Print main menu
    utils.Print(utils.mainMenu);

    // Take the main menu response
    mainMenuSelection = utils.GetMenuSelection(Utils.MENU_TYPES.MAIN);

    // Start a new game
    if (mainMenuSelection == "0")
    {
        string newGameMenuSelection = "";

        // Print new game menu
        utils.Print(utils.newGameMenu);

        // Take the new game menu response
        newGameMenuSelection = utils.GetMenuSelection(Utils.MENU_TYPES.NEW_GAME);

        Player p1 = null;
        Player p2 = null;

        // 2P game
        if (newGameMenuSelection == "0")
        {
            utils.Print("\nEnter player 1 name:");
            string p1Name = Console.ReadLine();

            utils.Print("Enter player 2 name:");
            string p2Name = Console.ReadLine();

            utils.Print("");

            p1 = new(p1Name, false, Piece.Colour.White);
            p2 = new(p2Name, false, Piece.Colour.Black);
        }
        // Vs AI game
        else if (newGameMenuSelection == "1")
        {
            utils.Print("Would you like to play as black or white? (b/w)");
            string bw = Console.ReadLine();

            // Validate black or white selection
            while (!bw.ToUpper().Equals("B") && !bw.ToUpper().Equals("W"))
            {
                utils.Print("Invalid response try again:");
                bw = Console.ReadLine();
            }

            utils.Print("\nEnter your name:");
            string p1Name = Console.ReadLine();

            if (bw.ToUpper().Equals("W"))
            {
                p1 = new(p1Name, false, Piece.Colour.White);
                p2 = new("AI", true, Piece.Colour.Black);
            }
            else
            {
                p1 = new("AI", true, Piece.Colour.White);
                p2 = new(p1Name, false, Piece.Colour.Black);
            }

        }

        // Start the game
        bool gameOver = false;
        ge = new(p1, p2);

        while (!gameOver)
        {
            try
            {
                gameOver = Play(ge);
            }
            catch (ChessException e)
            {
                utils.Print(e.Message);
                continue;
            }
        }
    }
    // Exit
    else if (mainMenuSelection == "1")
    {
        utils.Print(utils.goodbye);
    }
    // Credits
    else if (mainMenuSelection == "2")
    {
        utils.Print(utils.credits);
    }
}

/// <summary>
/// Game loop
/// </summary>
/// <param name="ge"></param>
bool Play(GameEngine ge)
{
    // Print the board
    string view = ge.View();
    utils.Print(view);
    utils.Print($"{ge.CurrentPlayer.username} your turn:");

    if (ge.IsPlayerInCheck(ge.CurrentPlayer))
    {
        utils.Print("(You are in check)");
    }

    // Call the game engine to execute the turn
    ge.Turn();

    // Check if the game is over or continuing
    if (!ge.p1.Winner && !ge.p2.Winner)
    {
        return false;
    }
    else if (ge.p1.Winner && ge.p2.Winner)
    {
        view = ge.View();
        utils.Print(view);
        utils.Print("Stalemate!\n");
        return true;
    }
    else
    {
        view = ge.View();
        utils.Print(view);
        string winner = (ge.p1.Winner) ? ge.p1.username : ge.p2.username;
        string Loser = (ge.p1.Winner) ? ge.p2.username : ge.p1.username;
        utils.Print($"\n{Loser} has been defeated. {winner} wins!\n");
    }

    return true;
}