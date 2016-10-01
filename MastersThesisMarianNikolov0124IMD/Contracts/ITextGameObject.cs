using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.Contracts
{
    public interface ITextGameObject : IGameObject
    {
        string Text { get; set; }
    }
}
