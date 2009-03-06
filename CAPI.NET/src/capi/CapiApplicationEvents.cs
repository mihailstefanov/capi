using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mommosoft.Capi {

    partial class CapiApplication {

        #region IncomingPhysicalConnection

        public event EventHandler<IncomingPhysicalConnectionEventArgs> IncomingPhysicalConnection;

        internal void OnIncomingPhysicalConnection(IncomingPhysicalConnectionEventArgs e) {
            if (IncomingPhysicalConnection != null) {
                try {
                    IncomingPhysicalConnection(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnIncomingPhysicalConnection, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        #endregion IncomingPhysicalConnection

        #region ConnectionStatusChanged

        public event EventHandler<ConnectionEventArgs> ConnectionStatusChanged;

        internal void OnConnectionStatusChanged(ConnectionEventArgs e) {
            if (ConnectionStatusChanged != null) {
                try {
                    ConnectionStatusChanged(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnConnectionStatusChanged, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        #endregion ConnectionStatusChanged

        #region DTFMIndication

        public event EventHandler<DTFMEventArgs> DTFMIndication;

        internal void OnDTFMIndication(DTFMEventArgs e) {
            if (DTFMIndication != null) {
                try {
                    DTFMIndication(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnDTFMIndication, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        #endregion DTFMIndication
    }
}
