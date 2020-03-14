﻿namespace JustChess.Engine.Contracts
{
    using System.Collections.Generic;

    using Players.Contracts;

    public interface IChessEngine
    {
        IEnumerable<IPlayer> Players { get; }

        void Initialize(IGameInitializationStrategy gameInitializationStrategy);

        void WinningConditions();

        void ToggleMenu();
    }
}
