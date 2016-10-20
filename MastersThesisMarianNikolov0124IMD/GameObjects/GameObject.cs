using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class GameObject : IGameObject
    {
        public GameObject()
        {
        }

        public GameObject(Position givePosition)
        {
            this.Position = givePosition;
        }

        public GameObject(Position givePosition, Size giveSize) : this (givePosition)
        {
            this.Bounds = giveSize;
        }

        public Size Bounds { get; set; }

        public Position Position { get; set; }
    }
}
