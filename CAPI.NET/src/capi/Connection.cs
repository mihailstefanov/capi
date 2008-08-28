namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.IO;
    using System.Runtime.InteropServices;

    public partial class Connection : Component {
        private CapiApplication _application;
        private Controller _controller;
        private byte _id;
        private short _ncci;
        private ConnectionStatus _status;
        private bool _dtfmListen;
        private short _dtfmDuration; //ms, defaule 40ms.
        private short _dtfmPause; // ms, default 40ms.
        private bool _listen;
        private string _callingPartyNumber;
        private string _calledPartyNumber;
        private object _tag;

        internal Connection(CapiApplication application, Controller controller, byte ID, string calledPartyNumber, string callingPartyNumber) {
            _dtfmDuration = 40;
            _dtfmPause = 40;
            _application = application;
            _controller = controller;
            _id = ID;
            _calledPartyNumber = calledPartyNumber;
            _callingPartyNumber = callingPartyNumber;
            _status = ConnectionStatus.WaitActivation;
        }

        public byte ID {
            get { return _id; }
        }

        public CapiApplication Application {
            get { return _application; }
        }
        public Controller Controller {
            get { return _controller; }
        }

        private ConnectionStatus Status {
            get { return _status; }
        }

        public string CalledPartyNumber {
            get { return _calledPartyNumber; }
        }

        public string CallingPartyNumber {
            get { return _callingPartyNumber; }
        }

        public object Tag {
            get { return _tag; }
            set { _tag = value; }
        }

        public bool DTFMListen {
            get {
                return _dtfmListen;
            }
            set {
                if (_dtfmListen == value) return;

                _dtfmListen = value;
                if (_status == ConnectionStatus.Connected) {
                    UpdateDTFMState();
                }
            }
        }

        public short DTFMPause {
            get { return _dtfmPause; }
            set { _dtfmPause = value; }
        }

        public short DTFMDuration {
            get { return _dtfmDuration; }
            set { _dtfmDuration = value; }
        }


        private void UpdateDTFMState() {

            if (_dtfmListen && !_listen)
                ThreadPool.QueueUserWorkItem(StartListenDTFM);
            else if (!_dtfmListen && _listen)
                ThreadPool.QueueUserWorkItem(StopListenDTFM);
        }

        private void StartListenDTFM(object o) {
            Debug.Assert(!_listen);
            FacilityRequest request = new FacilityRequest();

            request.FacilitySelector = FacilitySelector.DTMF;
            request.DTMFFacilityRequestParameter.FacilityFunction = FacilityFunction.StartListen;
            request.DTMFFacilityRequestParameter.ToneDuration = _dtfmPause;
            request.DTMFFacilityRequestParameter.GapDuration = _dtfmPause;
            PLCIParameter p = new PLCIParameter();
            p.ControllerID = _controller.ID;
            p.PLCI = _id;
            request.Identifier.Value = p.Value;
            RequestDTFM(request);
            _listen = true;
        }

        private void StopListenDTFM(object o) {
            Debug.Assert(_listen);
            FacilityRequest request = new FacilityRequest();

            request.FacilitySelector = FacilitySelector.DTMF;
            request.DTMFFacilityRequestParameter.FacilityFunction = FacilityFunction.StopListen;
            PLCIParameter p = new PLCIParameter();
            p.ControllerID = _controller.ID;
            p.PLCI = _id;
            request.Identifier.Value = p.Value;
            RequestDTFM(request);
            _listen = false;
        }

        public void SendDTFMTone(string digits) {
            if (_status != ConnectionStatus.Connected)
                throw new NotSupportedException();

            FacilityRequest request = new FacilityRequest();

            request.FacilitySelector = FacilitySelector.DTMF;
            request.DTMFFacilityRequestParameter.FacilityFunction = FacilityFunction.Send;
            request.DTMFFacilityRequestParameter.ToneDuration = _dtfmDuration;
            request.DTMFFacilityRequestParameter.GapDuration = _dtfmPause;
            request.DTMFFacilityRequestParameter.Digits = digits;
            PLCIParameter p = new PLCIParameter();
            p.ControllerID = _controller.ID;
            p.PLCI = _id;
            request.Identifier.Value = p.Value;
            RequestDTFM(request);
        }

        private void RequestDTFM(FacilityRequest request) {
            MessageAsyncResult result = new MessageAsyncResult(this, request, null, null);
            ThreadPool.QueueUserWorkItem(RequestWaitCallback, result);
            object o = result.InternalWaitForCompletion();
            if (o is Exception) {
                throw ((Exception)o);
            }
        }

        private void RequestWaitCallback(object state) {
            _application.SendRequestMessage((MessageAsyncResult)state);
        }

        public void HangUp() {
            try {
                IAsyncResult result = BeginHangUp(null, null);
                EndHangUp(result);
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::HangUp, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public IAsyncResult BeginHangUp(AsyncCallback callback, object state) {
            try {
                if (_status != ConnectionStatus.Connected)
                    throw new NotSupportedException();
                DisconnectB3Request request = new DisconnectB3Request(_ncci);
                MessageAsyncResult result = new MessageAsyncResult(this, request, callback, state);
                _application.SendRequestMessage(result);
                return result;
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::BeginHangUp, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public void EndHangUp(IAsyncResult asyncResult) {
            try {
                MessageAsyncResult result = asyncResult as MessageAsyncResult;
                if (asyncResult == null || result == null) {
                    throw (asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException();
                }
                object o = result.InternalWaitForCompletion();
                if (o is Exception) {
                    throw ((Exception)o);
                }
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::EndHangUp, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        short i = 0;
        public IAsyncResult BeginWriteData(byte[] data, int startIndex, AsyncCallback callback, object state) {
            IntPtr ptr = IntPtr.Zero;
            try {
                if (_status != ConnectionStatus.Connected)
                    throw new NotSupportedException();
                int length = data.Length - startIndex;
                Debug.Assert(length <= short.MaxValue);
                if (length >= short.MaxValue) throw new NotSupportedException();
                ptr = Marshal.AllocHGlobal(length);
                Marshal.Copy(data, startIndex, ptr, length);

                DataB3Request request = new DataB3Request();
                request.Identifier.ControllerID = _controller.ID;
                request.Identifier.PLCI = _id;
                request.Identifier.NCCI = _ncci;
                request.Data = ptr;
                request.DataLength = (short)length;
                i++;
                request.DataHandle = i;
                MessageAsyncResult result = new MessageAsyncResult(this, request, callback, state);
                _application.SendRequestMessage(result);
                return result;
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::BeginWriteData, Exception = {1}", ValidationHelper.HashString(this), e);
                // clean memory
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
                throw;
            }
        }

        public void EndWriteData(IAsyncResult asyncResult) {
            try {
                MessageAsyncResult result = asyncResult as MessageAsyncResult;
                if (asyncResult == null || result == null) {
                    throw (asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException();
                }
                object o = result.InternalWaitForCompletion();
                DataB3Request request = (DataB3Request)result.Request;
                IntPtr ptr = request.Data;
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
                if (o is Exception) {
                    throw ((Exception)o);
                }
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::EndWriteData, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        public override string ToString() {
            return string.Format("Connection {0} on Controller {1}", _id, _controller.ID);
        }


        internal void ConnectActiveIndication(ConnectActiveIndication indication) {
            try {
                ConnectActiveResponse response = new ConnectActiveResponse(indication);
                _application.SendMessage(response);
                _status = ConnectionStatus.WaitB3ProtocolActivation;
                ConnectB3Request request = new ConnectB3Request();
                request.Identifier.Value = response.Identifier.Value;
                _application.SendMessage(request);
                _application.OnPhysicalConnectionActive(new ConnectionEventArgs(indication, this));
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::ConnectActiveIndication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void ConnectB3Indication(ConnectB3Indication indication) {
            try {
                ConnectB3Response response = new ConnectB3Response(indication);
                IncomingLogicalConnectionEventArgs args = new IncomingLogicalConnectionEventArgs(indication, this, response);
                _application.OnIncomingLogicalConnection(args);
                _status = ConnectionStatus.ActivationB3Indication;
                _application.SendMessage(response);

            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::ConnectB3Indication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void ConnectB3ActiveIndication(ConnectB3ActiveIndication indication) {
            try {
                _ncci = indication.Identifier.NCCI;
                ConnectB3ActiveResponse response = new ConnectB3ActiveResponse(indication);
                _application.SendMessage(response);
                _status = ConnectionStatus.Connected;
                _application.OnLogicalConnectionActive(new ConnectionEventArgs(indication, this));
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::ConnectionB3ActiveIndication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void DisconnectB3Indication(DisconnectB3Indication indication) {
            try {
                DisconnectB3Response response = new DisconnectB3Response(indication);
                _application.SendMessage(response);
                _status = ConnectionStatus.Disconnecting;
                _ncci = 0;
                _application.OnLogicalConnectionDisconnected(new ConnectionEventArgs(indication, this));
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::DisconnectB3Indication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void DisconnectIndication(DisconnectIndication indication) {
            _status = ConnectionStatus.Disconnected;
        }

        internal void FacilityIndication(FacilityIndication indication) {
            Trace.TraceInformation("Connection#{0}::FacilityIndication, Digits = {0}", indication.Digits);

            try {
                _application.OnDTFMIndication(new DTFMEventArgs(indication, this, indication.Digits));
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::FacilityIndication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void ConnectB3Confirmation(ConnectB3Confirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Connection#{0}::ConnectB3Confirmation, Info = {0}", confirmation.Info);

            if (confirmation.Succeeded) {
                result.InvokeCallback();
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }

        internal void FacilityConfirmation(FacilityConfirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Connection#{0}::FacilityConfirmation, Info = {0}", confirmation.Info);

            if (confirmation.Succeeded) {
                result.InvokeCallback();
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }

        internal void DisconnectB3Confirmation(DisconnectB3Confirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Connection#{0}::DisconnectB3Confirmation, Info = {0}", confirmation.Info);

            if (confirmation.Succeeded) {
                result.InvokeCallback();
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }

        internal void DataB3Confirmation(DataB3Confirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Connection#{0}::DataB3Confirmation, Info = {0}", confirmation.Info);

            if (confirmation.Succeeded) {
                result.InvokeCallback();
            } else {
                result.InvokeCallback(new CapiException(confirmation.Info));
            }
        }
    }
}
