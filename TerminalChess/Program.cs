using TerminalChess;

Utils utils = new();
GameEngine ge;

// Print terminal chess title
utils.Print(utils.welcome);

string menuSelection = "";

while (menuSelection != "1")
{
    // Print main menu
    utils.Print(utils.mainMenu);

    // Take the menu response
    menuSelection = utils.GetMenuSelection(Utils.MENU_TYPES.MAIN);

    // Start a new game
    if (menuSelection == "0")
    {
        utils.Print("\nEnter player 1 name:");
        string p1Name = Console.ReadLine();

        utils.Print("Enter player 2 name:");
        string p2Name = Console.ReadLine();

        utils.Print("");

        Player p1 = new(p1Name);
        Player p2 = new(p2Name);
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
    else if(menuSelection == "1")
    {
        utils.Print(utils.goodbye);
    }
    // Credits
    else if(menuSelection == "2")
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