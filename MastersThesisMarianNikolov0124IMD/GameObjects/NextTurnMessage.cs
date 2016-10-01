using MastersThesisMarianNikolov0124IMD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
