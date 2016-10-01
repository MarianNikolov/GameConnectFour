using System;

namespace MastersThesisMarianNikolov0124IMD.Common
{
    public class KeyDownEventArgs : EventArgs
    {
        public GameCommandType Command { get; set; }

        public KeyDownEventArgs(GameCommandType enteredCommand)
        {
            this.Command = enteredCommand;
        }
    }
}
