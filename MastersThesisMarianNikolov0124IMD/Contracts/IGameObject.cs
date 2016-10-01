using MastersThesisMarianNikolov0124IMD.GameObjects;

namespace MastersThesisMarianNikolov0124IMD.Contracts
{
    public interface IGameObject
    {
        Position Position { get; set; }

        Size Bounds { get; set; }
    }
}
