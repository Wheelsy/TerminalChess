using TerminalChess;

Utils utils = new Utils();

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
    p1.MyTurn = true;

    GameEngine ge = new(p1, p2);
    string view = ge.View();
    utils.Print(view);
}