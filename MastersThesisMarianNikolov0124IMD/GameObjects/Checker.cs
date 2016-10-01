using MastersThesisMarianNikolov0124IMD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class Checker : GameObject, IGameObject, IMovable
    {
        public Position Speed { get; set; }

        public Checker(Position givePosition, Size giveSize) 
            : base(givePosition, giveSize)
        {
        }

        public void Move(int width, int height)
        {
            this.Position = new Position(width, height);
        }
    }
}
