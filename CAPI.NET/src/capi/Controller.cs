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
            _connections = new ConnectionCollection();
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
        public Connection Connect(string callingPartyNumber, string calledPartyNumber, int service,
            B1Protocol b1, B2Protocol b2, B3Protocol b3) {
            IAsyncResult asyncResult;
            try {
                asyncResult = BeginConnect(callingPartyNumber, calledPartyNumber, service, b1, b2, b3, null, null);
                if (((_timeout != -1) && !asyncResult.IsCompleted) && (!asyncResult.AsyncWaitHandle.WaitOne(_timeout, false) || !asyncResult.IsCompleted)) {
                    throw new TimeoutException("Listen");
                }
                return EndConnect(asyncResult);
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::Call, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }


        public IAsyncResult BeginConnect(string callingPartyNumber, string calledPartyNumber, int service,
            B1Protocol b1, B2Protocol b2, B3Protocol b3, AsyncCallback callback, object state) {
            try {
                ConnectRequest request = new ConnectRequest(_id);
                UInt16 CIPValue = 0;
                if (service != 0) {
                    do {
                        if ((service & 1) != 0) break;
                        service >>= 1;
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

        public IAsyncResult BeginListen(CIPMask CIPMask, AsyncCallback callback, object state) {
            try {
                ListenRequest request = new ListenRequest(_id);
                request.CIPMask = CIPMask;
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

        public void Listen(CIPMask CIPMask) {
            IAsyncResult asyncResult;
            try {
                asyncResult = BeginListen(CIPMask, null, null);
                if (((_timeout != -1) && !asyncResult.IsCompleted) && (!asyncResult.AsyncWaitHandle.WaitOne(_timeout, false) || !asyncResult.IsCompleted)) {
                    throw new TimeoutException("Listen");
                }
                EndListen(asyncResult);
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::Listen, Exception = {1}", ValidationHelper.HashString(this), e);
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


        private void AlertInternal(PLCIParameter plci) {
            AlertRequest alert = new AlertRequest();
            alert.Identifier = plci;
            _application.SendMessage(alert);
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
                _connections.InternalAdd(connection);
                result.InvokeCallback(connection);
                _application.OnPhysicalConnected(new ConnectionEventArgs(confirmation, connection));
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }

        internal void DisconnectIndication(DisconnectIndication indication) {
            Connection connection = _connections.GetConnectionByPLCI(indication.Identifier.PLCI);
            Debug.Assert(connection != null, "Connecion needs to exist at this point.");
            DisconnectResponse response = new DisconnectResponse(indication);
            _application.SendMessage(response);
            connection.Status = ConnectionStatus.Disconnected;
            _connections.InternalRemove(indication.Identifier.PLCI);
            connection.Dispose();
        }

        internal void ConnectIndication(ConnectIndication indication) {
            Connection connection = _connections.GetConnectionByPLCI(indication.Identifier.PLCI);
            if (connection == null) {
                connection = new Connection(_application,
                    this,
                    indication.Identifier.PLCI,
                    indication.CallingPartyNumber,
                    indication.CalledPartyNumber);
                _connections.InternalAdd(connection);
            }

            AlertRequest request = new AlertRequest();
            request.Identifier.Value = indication.Identifier.Value;
            _application.SendMessage(request);

            // Notify user application....
            connection.Status = ConnectionStatus.D_ConnectPending;
            // Notify user application....

            IncomingPhysicalConnectionEventArgs args = new IncomingPhysicalConnectionEventArgs(indication, connection);
            _application.OnIncomingPhysicalConnection(args);
            _application.SendMessage(args.Response);
            //_application.OnPhysicalConnected(new ConnectionEventArgs(indication, connection));
        }

    }
}

