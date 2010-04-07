namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IncomingPhysicalConnectionEventArgs : ConnectionEventArgs {

        internal IncomingPhysicalConnectionEventArgs(Connection connection)
            : base(connection) {
        }
    }
}
