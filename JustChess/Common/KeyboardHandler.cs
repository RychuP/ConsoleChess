namespace JustChess.Common
{
    // Libraries
    using SadConsole.Components;
    using SadConsole.Input;

    // Chess
    using Engine.Contracts;

    class KeyboardHandler : KeyboardConsoleComponent
    {
        IChessEngine engine;

        public KeyboardHandler(IChessEngine engine)
        {
            this.engine = engine;
        }

        public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                engine.ToggleMenu();
            }

            handled = true;
        }
    }
}