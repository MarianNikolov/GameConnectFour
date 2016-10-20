using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class CheckerBlue : Checker, IGameObject
    {
        public CheckerBlue(Position givePosition, Size giveSize) 
            : base(givePosition, giveSize)
        {
        }
    }
}
