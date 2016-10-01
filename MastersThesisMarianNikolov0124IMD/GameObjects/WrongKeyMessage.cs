using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class WrongKeyMessage : TextGameObject, ITextGameObject
    {
        public WrongKeyMessage(Position givePosition, Size giveSize, string giveText) 
            : base(givePosition, giveSize, giveText)
        {
        }
    }
}
