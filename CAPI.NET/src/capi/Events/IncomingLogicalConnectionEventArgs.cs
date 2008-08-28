using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    public class IncomingLogicalConnectionEventArgs : ConnectionEventArgs {
        private ConnectB3Response _response;

        public IncomingLogicalConnectionEventArgs(ConnectB3Indication indication, Connection connection,
            ConnectB3Response response)
            : base(indication, connection) {
            _response = response;
        }
        public ConnectB3Response Response {
            get { return _response; }
        }
    }
}
