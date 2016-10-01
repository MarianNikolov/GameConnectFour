using MastersThesisMarianNikolov0124IMD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class CheckerBlue : Checker, IGameObject, IMovable
    {
        public CheckerBlue(Position givePosition, Size giveSize) 
            : base(givePosition, giveSize)
        {
        }
    }
}
