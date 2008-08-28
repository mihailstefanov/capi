namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class ConnectionCollection : ReadOnlyBindingList<Connection> {
        private Dictionary<byte, Connection> _dictionary;
        internal ConnectionCollection() {
            IsReadOnly = true;
            _dictionary = new Dictionary<byte, Connection>();
        }

        public Connection GetConnectionByID(byte connectionID) {
            lock (_dictionary) {
                Connection connection;
                _dictionary.TryGetValue(connectionID, out connection);
                return connection;
            }
        }

        internal void InternalAdd(Connection connection) {
            lock (_dictionary) {
                _dictionary.Add(connection.ID, connection);
                ProtectedInsertItem(0, connection);
            }
        }

        internal void InternalRemove(byte connectionID) {
            lock (_dictionary) {
                Connection connection;
                if (_dictionary.TryGetValue(connectionID, out connection)) {
                    _dictionary.Remove(connectionID);
                    int index = base.IndexOf(connection);
                    ProtectedRemoveItem(index);
                }
            }
        }
    }
}
