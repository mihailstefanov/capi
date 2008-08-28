namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;

    partial class CapiApplication {
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

        public event EventHandler<ConnectionEventArgs> PhysicalConnected;

        internal void OnPhysicalConnected(ConnectionEventArgs e) {
            if (PhysicalConnected != null) {
                try {
                    PhysicalConnected(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnPhysicalConnected, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        public event EventHandler<ConnectionEventArgs> PhysicalConnectionActive;

        internal void OnPhysicalConnectionActive(ConnectionEventArgs e) {
            if (PhysicalConnectionActive != null) {
                try {
                    PhysicalConnectionActive(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnPhysicalConnectionActive, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        public event EventHandler<IncomingLogicalConnectionEventArgs> IncomingLogicalConnection;

        internal void OnIncomingLogicalConnection(IncomingLogicalConnectionEventArgs e) {
            if (IncomingLogicalConnection != null) {
                try {
                    IncomingLogicalConnection(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnIncomingLogicalConnection, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        public event EventHandler<ConnectionEventArgs> LogicalConnectionActive;

        internal void OnLogicalConnectionActive(ConnectionEventArgs e) {
            if (LogicalConnectionActive != null) {
                try {
                    LogicalConnectionActive(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnLogicalConnectionActive, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

        public event EventHandler<ConnectionEventArgs> LogicalConnectionDisconnected;

        internal void OnLogicalConnectionDisconnected(ConnectionEventArgs e) {
            if (LogicalConnectionDisconnected != null) {
                try {
                    LogicalConnectionDisconnected(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnLogicalConnectionDisconnected, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }
        public event EventHandler<ConnectionEventArgs> PhysicalConnectionDisconnected;

        internal void OnPhysicalConnectionDisconnected(ConnectionEventArgs e) {
            if (PhysicalConnectionDisconnected != null) {
                try {
                    PhysicalConnectionDisconnected(this, e);
                } catch (Exception ex) {
                    Trace.TraceError("CapiApplication#{0}::OnPhysicalConnectionDisconnected, Exception = {1}", ValidationHelper.HashString(this), ex);
                }
            }
        }

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
    }
}
