namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IncomingPhysicalConnectionEventArgs : ConnectionEventArgs {
        private ConnectResponse _response;

        internal IncomingPhysicalConnectionEventArgs(ConnectIndication indication, Connection connection)
            : base(indication, connection) {
            _response = new ConnectResponse(indication);
            _response.ConnectedNumber = connection.CalledPartyNumber;
        }

        public string ConnectedNumber {
            get { return _response.ConnectedNumber; }
            set { _response.ConnectedNumber = value; }
        }

        public Reject Reject {
            get { return _response.Reject; }
            set { _response.Reject = value; }
        }

        public B1Protocol B1Protocol {
            get { return _response.BPtotocol.B1; }
            set { _response.BPtotocol.B1 = value; }
        }

        public B2Protocol B2Protocol {
            get { return _response.BPtotocol.B2; }
            set { _response.BPtotocol.B2 = value; }
        }

        public B3Protocol B3Protocol {
            get { return _response.BPtotocol.B3; }
            set { _response.BPtotocol.B3 = value; }
        }

        internal ConnectResponse Response {
            get { return _response; }
        }
    }
}
