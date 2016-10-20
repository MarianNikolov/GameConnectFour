using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class CheckerRed : Checker, IGameObject
    {
        public CheckerRed(Position givePosition, Size giveSize) 
            : base(givePosition, giveSize)
        {
        }
    }
}
