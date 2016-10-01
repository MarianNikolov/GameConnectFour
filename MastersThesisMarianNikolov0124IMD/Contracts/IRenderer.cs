using MastersThesisMarianNikolov0124IMD.GameObjects;
using MastersThesisMarianNikolov0124IMD.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.Contracts
{
    public interface IRenderer
    {
        int ScreenWidth { get; }

        int ScreenHeigth { get; }

        void Clear();

        void RefreshGame();

        void Draw(params IGameObject[] drawObjects);

        void DrawPleyField(IGameObject[,] drawObjects);

        void DrawWinLine(Position startPosition, Position endPosition);

        event KeyDownEventHandler presingKey;

        void ShowStartGameScreen();

        void ShowEndGameScreen();
    }
}
