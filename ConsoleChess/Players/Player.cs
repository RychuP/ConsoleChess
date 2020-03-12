namespace ConsoleChess.Players
{
    using System;
    using System.Collections.Generic;

    using Common;
    using Contracts;
    using Figures.Contracts;

    public class Player : IPlayer
    {
        private readonly ICollection<IFigure> figures;

        public Player(string name, ChessColor color)
        {
            // TODO: validate name length
            Name = name;
            figures = new List<IFigure>();
            Trophies = new List<IFigure>();
            Color = color;
        }

        public string Name { get; private set; }

        public ChessColor Color { get; private set; }

        public IList<IFigure> Trophies { get; private set; }

        public void AddTrophy(IFigure figure)
        {
            Trophies.Add(figure);
        }

        public void ResetTrophies()
        {
            Trophies.Clear();
        }

        public void AddFigure(IFigure figure)
        {
            ObjectValidator.CheckIfObjectIsNull(figure, GlobalErrorMessages.NullFigureErrorMessage);

            // TODO: check figure and player color
            CheckIfFigureExists(figure);
            figures.Add(figure);
        }

        public void RemoveFigure(IFigure figure)
        {
            ObjectValidator.CheckIfObjectIsNull(figure, GlobalErrorMessages.NullFigureErrorMessage);

            // TODO: check figure and player color
            CheckIfFigureDoesNotExist(figure);
            figures.Remove(figure);
        }

        private void CheckIfFigureExists(IFigure figure)
        {
            if (figures.Contains(figure))
            {
                throw new InvalidOperationException("This player already owns this figure!");
            }
        }

        private void CheckIfFigureDoesNotExist(IFigure figure)
        {
            if (!figures.Contains(figure))
            {
                throw new InvalidOperationException("This player does not own this figure!");
            }
        }
    }
}
