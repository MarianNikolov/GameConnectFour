using MastersThesisMarianNikolov0124IMD.Contracts;
using MastersThesisMarianNikolov0124IMD.GameObjects;
using MastersThesisMarianNikolov0124IMD.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.Factories
{
    public static class GameObjectFactory
    {
        public static Size GenerateSize(int width, int height)
        {
            return new Size(width, height); 
        }

        public static Position GeneratePosition(int left, int top)
        {
            return new Position(left, top);
        }

        public static IGameObject GenerateRedChecker(Position position, Size size)
        {
            return new CheckerRed(position, size); 
        }

        public static IGameObject GenerateBlueChecker(Position position, Size size)
        {
            return new CheckerBlue(position, size);
        }

        public static IGameObject GenerateBoard(Position position, Size size)
        {
            return new GameBoard(position, size);
        }

        public static ITextGameObject GenerateNextTurnMessage(Position position, Size size, string text)
        {
            return new NextTurnMessage(position, size, text);
        }

        public static ITextGameObject GenerateWrongKeyMessage(Position position, Size size, string text)
        {
            return new WrongKeyMessage(position, size, text);
        }

        public static ITextGameObject GenerateMessage(Position position, Size size, string text)
        {
            return new TextGameObject(position, size, text);
        }
        //public static Position GenerateNewPosition(Position giveSeed, IGameObject current)
        //{
        //    var left = current.Position.Left + giveSeed.Left;
        //    var top = current.Position.Top + giveSeed.Top;

        //    return new Position(left, top);
        //}

        //public static Position GenerateNewPosition(Position[] positions)
        //{
        //    return new Position(giveLeft, giveTop);
        //}

    }
}
