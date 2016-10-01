using MastersThesisMarianNikolov0124IMD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class CheckerRed : Checker, IGameObject, IMovable
    {
        public CheckerRed(Position givePosition, Size giveSize) 
            : base(givePosition, giveSize)
        {
        }
    }
}
