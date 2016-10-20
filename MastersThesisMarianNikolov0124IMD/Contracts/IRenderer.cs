using MastersThesisMarianNikolov0124IMD.GameObjects;
using MastersThesisMarianNikolov0124IMD.Renderers;

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

        void StopEventHandler();

        void ShowStartGameScreen();

        void ShowEndGameScreen(ITextGameObject winnerText);
    }
}
