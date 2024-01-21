using TerminalChess;

Utils utils = new Utils();

// Print terminal chess title
utils.Print(utils.welcome);

// Print main menu
utils.Print(utils.mainMenu);

// Take the menu response
utils.GetMenuSelection(Utils.MENU_TYPES.MAIN);