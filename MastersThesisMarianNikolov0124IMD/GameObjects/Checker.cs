using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class Checker : GameObject, IGameObject
    {
        public Checker(Position givePosition, Size giveSize) 
            : base(givePosition, giveSize)
        {
        }
    }
}
