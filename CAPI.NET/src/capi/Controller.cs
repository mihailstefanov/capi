namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Resources;
    using Mommosoft.Capi.Properties;
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using System.Threading;
    using System.ComponentModel;

    public partial class Controller : Component {
        private int _timeout = 1000 * 60; // 1 min.
        private uint _id;
        private CapiApplication _application;
        private ProtocolCollection _protocols;
        private ConnectionCollection _connections;
        private ControllerStatus _status;

        internal Controller(CapiApplication application, uint id) {
            _application = application;
            _id = id;
            _status = ControllerStatus.Idle;
        }

        public uint ID {
            get { return _id; }
        }

        public CapiApplication Application {
            get { return _application; }
        }

        public ControllerStatus Status {
            get { return _status; }
        }

        public ProtocolCollection Protocols {
            get {
                if (_protocols == null) {
                    CreateProtocolCollection();
                }
                return _protocols;
            }
        }

        public ConnectionCollection Connections {
            get {
                if (_connections == null) {
                    _connections = new ConnectionCollection();
                }
                return _connections;
            }
        }

        public Connection GetConnectionByPLCI(uint id) {
            return Connections.GetConnectionByPLCI(id);
        }


        #region Connect

        /// <summary>
        /// Call calledPartyNumber.
        /// </summary>
        /// <param name="callingPartyNumber">our number</param>
        /// <param name="calledPartyNumber">called party number</param>
        /// <returns></returns>
        public Connection Connect(string callingPartyNumber, string calledPartyNumber, CIPServices service,
            B1Protocol b1, B2Protocol b2, B3Protocol b3) {
            IAsyncResult asyncResult;
            try {
                asyncResult = BeginConnect(callingPartyNumber, calledPartyNumber, service, b1, b2, b3, null, null);
                if (((_timeout != -1) && !asyncResult.IsCompleted) && (!asyncResult.AsyncWaitHandle.WaitOne(_timeout, false) || !asyncResult.IsCompleted)) {
                    throw new TimeoutException("Connect");
                }
                return EndConnect(asyncResult);
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::Call, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }


        public IAsyncResult BeginConnect(string callingPartyNumber, string calledPartyNumber, CIPServices service,
            B1Protocol b1, B2Protocol b2, B3Protocol b3, AsyncCallback callback, object state) {
            try {
                ConnectRequest request = new ConnectRequest(_id);
                UInt16 CIPValue = 0;
                int s = (int)service;
                if (s != 0) {
                    do {
                        if ((s & 1) != 0) break;
                        s >>= 1;
                        CIPValue++;
                    } while (CIPValue < 31);
                }

                request.CIPValue = CIPValue;

                request.CalledPartyNumber = calledPartyNumber;
                request.CallingPartyNumber = callingPartyNumber;

                request.BPtotocol.B1 = b1;
                request.BPtotocol.B2 = b2;
                request.BPtotocol.B3 = b3;

                MessageAsyncResult result = new MessageAsyncResult(this, request, callback, state);
                _application.SendRequestMessage(result);
                _status = ControllerStatus.Connecting;
                return result;
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::BeginListen, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public Connection EndConnect(IAsyncResult asyncResult) {
            try {
                MessageAsyncResult result = asyncResult as MessageAsyncResult;
                if (asyncResult == null || result == null) {
                    throw (asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException();
                }
                object o = result.InternalWaitForCompletion();
                if (o is Exception) {
                    throw ((Exception)o);
                }
               
                return (Connection)result.Result;
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::EndCall, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        #endregion Connect

        #region Listen

        public void Listen(CIPMask service) {
            IAsyncResult asyncResult;
            try {
                asyncResult = BeginListen(service, null, null);
                if (((_timeout != -1) && !asyncResult.IsCompleted) && (!asyncResult.AsyncWaitHandle.WaitOne(_timeout, false) || !asyncResult.IsCompleted)) {
                    throw new TimeoutException("Listen");
                }
                EndListen(asyncResult);
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::Listen, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;

            }
        }

        public IAsyncResult BeginListen(CIPMask service, AsyncCallback callback, object state) {
            try {
                ListenRequest request = new ListenRequest(_id);
                request.CIPMask = service;
                MessageAsyncResult result = new MessageAsyncResult(this, request, callback, state);
                _application.SendRequestMessage(result);
                return result;
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::BeginListen, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public void EndListen(IAsyncResult asyncResult) {
            try {
                MessageAsyncResult result = asyncResult as MessageAsyncResult;
                if (asyncResult == null || result == null) {
                    throw (asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException();
                }
                object o = result.InternalWaitForCompletion();
                if (o is Exception) {
                    throw ((Exception)o);
                }
                _status = ControllerStatus.Listen;
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::EndListen, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        #endregion Listen

        public override string ToString() {
            return string.Format("Controller {0}", _id);
        }

        private void CreateProtocolCollection() {
            NativeMethods.Profile profile = CapiPInvoke.GetProfile(_id);
            // B1 protocols;
            List<Protocol> protocolList = GetProtocols(profile.B1_Protocols, typeof(NativeMethods.B1Protocol), ProtocolStrings.ResourceManager, "B1_");
            _protocols = new ProtocolCollection(protocolList);
        }

        private List<Protocol> GetProtocols(int protocols, Type t, ResourceManager rm, string prefix) {
            List<Protocol> protocolList = new List<Protocol>();
            Array valueList = Enum.GetValues(t);

            foreach (int value in valueList) {
                if ((protocols & value) != 0) {
                    string key = string.Format("{0}{1:X4}", prefix, value);
                    protocolList.Add(new Protocol(value, rm.GetString(key)));
                }
            }
            return protocolList;
        }

        internal void ListenConfirmation(ListenConfirmation confirmation, MessageAsyncResult result) {
            if (confirmation.Succeeded) {
                result.InvokeCallback();
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }

        internal void ConnectConfirmation(ConnectConfirmation confirmation, MessageAsyncResult result) {
            if (confirmation.Succeeded) {

                ConnectRequest request = (ConnectRequest)result.Request;
                Connection connection = new Connection(_application, this, confirmation.Identifier.PLCI, request.CalledPartyNumber, request.CallingPartyNumber);
                Connections.InternalAdd(connection);
                connection.Inititator = true;
                connection.Status = ConnectionStatus.D_ConnectPending;
                result.InvokeCallback(connection);
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }

        internal void AlertConfirmation(AlertConfirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Controller#{0}::AlertConfirmation, Info = {0}", confirmation.Info);

            if (result != null) {
                if (confirmation.Succeeded) {
                    result.InvokeCallback();
                } else {
                    result.InvokeCallback(new CapiException(confirmation.Info));
                }
            }
        }

        internal void DisconnectIndication(DisconnectIndication indication) {
            Connection connection = Connections.GetConnectionByPLCI(indication.Identifier.PLCI);
            Debug.Assert(connection != null, "Connecion needs to exist at this point.");
            DisconnectResponse response = new DisconnectResponse(indication);
            _application.SendMessage(response);
            connection.Status = ConnectionStatus.Disconnected;
            Connections.InternalRemove(indication.Identifier.PLCI);
            connection.Dispose();
        }

        internal void ConnectIndication(ConnectIndication indication) {
            Connection connection = Connections.GetConnectionByPLCI(indication.Identifier.PLCI);
            if (connection == null) {
                connection = new Connection(_application,
                    this,
                    indication.Identifier.PLCI,
                    indication.CalledPartyNumber,
                    indication.CallingPartyNumber);
                Connections.InternalAdd(connection);
            }

            AlertRequest request = new AlertRequest();
            request.Identifier.Value = indication.Identifier.Value;
            _application.SendMessage(request);

            // Notify user application....
            connection.Status = ConnectionStatus.D_ConnectPending;
            connection.ConnectIndication(indication);
            IncomingPhysicalConnectionEventArgs args = new IncomingPhysicalConnectionEventArgs(connection);
            _application.OnIncomingPhysicalConnection(args);
        }
    }
}

