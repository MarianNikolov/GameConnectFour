using MastersThesisMarianNikolov0124IMD.GameObjects;

namespace MastersThesisMarianNikolov0124IMD.Contracts
{
    public interface IMovable
    {
        Position Speed { get; set; }

        void Move(int width, int height);
    }
}
