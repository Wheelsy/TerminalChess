using TerminalChess;

Utils utils = new();
GameEngine ge;

// Print terminal chess title
utils.Print(utils.welcome);

// Print main menu
utils.Print(utils.mainMenu);

// Take the menu response
string menuSelection = utils.GetMenuSelection(Utils.MENU_TYPES.MAIN);

// Start a new game
if (menuSelection == "0")
{
    Console.WriteLine("\nEnter player 1 name:");
    string p1Name = Console.ReadLine();

    Console.WriteLine("Enter player 2 name:");
    string p2Name = Console.ReadLine();

    Console.WriteLine();

    Player p1 = new(p1Name);
    Player p2 = new(p2Name);
    bool gameOver = false;
    ge = new(p1, p2);

    while (!gameOver)
    {
        gameOver = Play(ge);
    }

    utils.Print(utils.gameOverMenu);
    var selection = utils.GetMenuSelection(Utils.MENU_TYPES.GAME_OVER);
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
    
    // Call the game engine to execute the turn
    ge.Turn();

    // Check if the game is over
    if (!ge.p1.Winner && !ge.p2.Winner)
    {
        return false;
    }

    view = ge.View();
    utils.Print(view);

    string winner = (ge.p1.Winner) ? ge.p1.username : ge.p2.username;
    string Loser = (ge.p1.Winner) ? ge.p2.username : ge.p1.username;

    utils.Print($"\n{Loser} has been checkmated. {winner} wins!\n");

    return true;
}