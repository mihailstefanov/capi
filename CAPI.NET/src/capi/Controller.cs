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
        private int _id;
        private CapiApplication _application;
        private ProtocolCollection _protocols;
        private ConnectionCollection _connections;
        private bool _alert;
        private ControllerStatus _status;

        internal Controller(CapiApplication application, int id) {
            _connections = new ConnectionCollection();
            _application = application;
            _id = id;
            _status = ControllerStatus.Idle;
        }

        public int ID {
            get { return _id; }
        }

        public CapiApplication Application {
            get { return _application; }
        }

        public bool Alert {
            get { return _alert; }
            set { _alert = value; }
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

        public Connection GetConnectionByID(byte id) {
            return Connections.GetConnectionByID(id);
        }

        public IAsyncResult BeginCall(string callingPartyNumber, string calledPartyNumber, AsyncCallback callback, object state) {
            try {
                ConnectRequest request = new ConnectRequest(_id);
                request.CIPValue = 16;
                request.BPtotocol.B1 = B1Protocol.HDLC64BFN;
                request.BPtotocol.B2 = B2Protocol.Transparent;
                request.CalledPartyNumber = calledPartyNumber;
                request.CallingPartyNumber = callingPartyNumber;

                MessageAsyncResult result = new MessageAsyncResult(this, request, callback, state);
                _application.SendRequestMessage(result);
                _status = ControllerStatus.Calling;
                return result;
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::BeginListen, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public Connection EndCall(IAsyncResult asyncResult) {
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

        /// <summary>
        /// Call calledPartyNumber.
        /// </summary>
        /// <param name="callingPartyNumber">our number</param>
        /// <param name="calledPartyNumber">called party number</param>
        /// <returns></returns>
        public Connection Call(string callingPartyNumber, string calledPartyNumber) {
            IAsyncResult asyncResult;
            try {
                asyncResult = BeginCall(callingPartyNumber, calledPartyNumber, null, null);
                if (((_timeout != -1) && !asyncResult.IsCompleted) && (!asyncResult.AsyncWaitHandle.WaitOne(_timeout, false) || !asyncResult.IsCompleted)) {
                    throw new TimeoutException("Listen");
                }
                return EndCall(asyncResult);
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::Call, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public IAsyncResult BeginListen(string callingPartyNumber, string callingSubaddress, AsyncCallback callback, object state) {
            try {
                ListenRequest request = new ListenRequest(_id);
                //request.CallingPartyNumber = callingPartyNumber;
                //request.CallingSubaddress = callingSubaddress;
                //request.CIPMask = CIPMask.Speech | CIPMask.Telephony | CIPMask.Telephony7KHZ | CIPMask.Audio7KHZ | CIPMask.Audio31KHZ;
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

        public void Listen() {
            Listen(string.Empty, string.Empty);
        }

        public void Listen(string callingPartyNumber, string callingSubaddress) {
            IAsyncResult asyncResult;
            try {
                asyncResult = BeginListen(callingPartyNumber, callingSubaddress, null, null);
                if (((_timeout != -1) && !asyncResult.IsCompleted) && (!asyncResult.AsyncWaitHandle.WaitOne(_timeout, false) || !asyncResult.IsCompleted)) {
                    throw new TimeoutException("Listen");
                }
                EndListen(asyncResult);
            } catch (Exception e) {
                Trace.TraceError("Controller#{0}::Listen, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;

            }
        }

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
            Connection connection = _connections.GetConnectionByID(indication.Identifier.PLCI);
            Debug.Assert(connection != null, "Connecion needs to be already initialized by connect confirmation.");
            connection.DisconnectIndication(indication);
            _connections.InternalRemove(indication.Identifier.PLCI);
            DisconnectResponse response = new DisconnectResponse(indication);
            _application.SendMessage(response);
            _application.OnPhysicalConnectionDisconnected(new ConnectionEventArgs(indication, connection));
        }

        internal void ConnectIndication(ConnectIndication indication) {
            Connection connection = new Connection(_application, this, indication.Identifier.PLCI, indication.CallingPartyNumber, indication.CalledPartyNumber);
            _connections.InternalAdd(connection);

            ConnectResponse response = new ConnectResponse(indication);

            IncomingPhysicalConnectionEventArgs args = new IncomingPhysicalConnectionEventArgs(indication, connection);
            _application.OnIncomingPhysicalConnection(args);

            if (args.Reject == Reject.Accept) {
                if (_alert) {
                    AlertInternal(indication.Identifier);
                }
                response.ConnectedNumber = indication.CalledPartyNumber;
            }

            response.Reject = args.Reject;
            response.BPtotocol.B1 = B1Protocol.HDLC64BFN;
            response.BPtotocol.B2 = B2Protocol.Transparent;

            _application.SendMessage(response);
            _application.OnPhysicalConnected(new ConnectionEventArgs(indication, connection));
        }

    }
}

