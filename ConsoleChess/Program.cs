namespace ConsoleChess
{
    // .NET
    using System;

    // Libraries
    using SadConsole;

    // Chess
    using Engine;
    using Engine.Initializations;
    using ConsoleChess.Common;

    public static class Program
    {
        static void Main()
        {
            // setup the engine and create the main window
            Game.Create(GlobalConstants.BoardWidth + GlobalConstants.UserInterfaceWidth,
                GlobalConstants.WindowHeight);

            // hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            // wrapper
            var container = new ContainerConsole();
            Global.CurrentScreen = container;

            // main class
            var chessEngine = new StandardTwoPlayerEngine(container);

            // initialize players, pieces and the board
            var gameInitializationStrategy = new StandardStartGameInitializationStrategy();
            chessEngine.Initialize(gameInitializationStrategy);
        }
    }
}