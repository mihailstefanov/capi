namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    public class ConnectionEventArgs : EventArgs {
        private readonly Connection _connection;

        public ConnectionEventArgs(Connection connection) {
            _connection = connection;
        }

        public Connection Connection {
            get { return _connection; }
        }
    }
}
