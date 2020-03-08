namespace ConsoleChess
{
    // .NET
    using System;

    // Libraries
    using SadConsole;
    using Game = SadConsole.Game;

    // Chess
    using Engine;
    using Engine.Initializations;
    using Renderers;

    public static class Program
    {
        static void Main()
        {

            // Setup the engine and create the main window (to do: calculate these values)
            Game.Create(120, 38);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            var container = new ContainerConsole();
            Global.CurrentScreen = container;

            var renderer = new ConsoleRenderer(container);
            // renderer.RenderMainMenu();
            // var inputProvider = new ConsoleInputProvider();
            // var chessEngine = new StandardTwoPlayerEngine(renderer, inputProvider);

        }
    }
}