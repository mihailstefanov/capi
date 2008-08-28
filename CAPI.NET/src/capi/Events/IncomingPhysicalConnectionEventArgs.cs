namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IncomingPhysicalConnectionEventArgs : ConnectionEventArgs {
        private Reject _reject = Reject.NotAvailable;

        public IncomingPhysicalConnectionEventArgs(Message message, Connection connection)
            : base(message, connection) { }

        public Reject Reject {
            get { return _reject; }
            set { _reject = value; }
        }
    }
}
