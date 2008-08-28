namespace Mommosoft.Capi
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ControllerEventArgs : MessageEventArgs
    {
        private readonly Connection _connection;

        public ControllerEventArgs(Message message, Connection connection)
            : base(message)
        {
            _connection = connection;
        }

        public Connection Connection
        {
            get { return _connection; }
        }
    }
}
