using MastersThesisMarianNikolov0124IMD.Contracts;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class TextGameObject : GameObject, IGameObject, ITextGameObject
    {
        private string text;

        public TextGameObject(Position givePosition, Size giveSize, string giveText) 
            : base(givePosition, giveSize)
        {
            this.Text = giveText;
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}
