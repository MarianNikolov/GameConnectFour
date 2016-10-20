using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class NextTurnMessage : TextGameObject, ITextGameObject
    {
        public NextTurnMessage(Position givePosition, Size giveSize, string giveText) 
            : base(givePosition, giveSize, giveText)
        {
        }
    }
}
