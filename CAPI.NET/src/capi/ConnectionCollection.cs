namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class ConnectionCollection : ReadOnlyBindingList<Connection> {
        private Dictionary<uint, Connection> _dictionary;
        internal ConnectionCollection() {
            IsReadOnly = true;
            _dictionary = new Dictionary<uint, Connection>();
        }

        public Connection GetConnectionByPLCI(uint plci) {
            lock (_dictionary) {
                Connection connection = null;
                _dictionary.TryGetValue(plci, out connection);
                return connection;
            }
        }

        internal void InternalAdd(Connection connection) {
            lock (_dictionary) {
                _dictionary.Add(connection.PLCI, connection);
                ProtectedInsertItem(0, connection);
            }
        }

        internal void InternalRemove(uint connectionID) {
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
