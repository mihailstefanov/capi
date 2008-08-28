namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MessageEventArgs : EventArgs {
        private readonly Message _message;

        public MessageEventArgs(Message message) {
            _message = message;
        }

        public Message Message {
            get { return _message; }
        }
    }
}
