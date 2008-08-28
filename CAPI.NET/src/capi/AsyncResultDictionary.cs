namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class AsyncResultDictionary : Dictionary<short, MessageAsyncResult> {
        private object _syncObject = new object();

        public void AddSafe(short key, MessageAsyncResult result) {
            lock (_syncObject) {
                Add(key, result);
            }
        }

        public void RemoveSafe(short key) {
            lock (_syncObject) {
                Remove(key);
            }
        }

        public MessageAsyncResult GetSafe(short key) {
            lock (_syncObject) {
                MessageAsyncResult r;
                TryGetValue(key, out r);
                return r;
            }
        }
    }
}
