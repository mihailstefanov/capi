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
        private const uint INVAL_NCCI = 0;
        private CapiApplication _application;
        private Controller _controller;
        private uint _plci;
        private uint _ncci;
        private ConnectionStatus _status;
        private bool _dtfmListen;
        private short _dtfmDuration; //ms, defaule 40ms.
        private short _dtfmPause; // ms, default 40ms.
        private bool _listen;
        private string _callingPartyNumber;
        private string _calledPartyNumber;
        private object _tag;
        private bool _inititator;
        private ConnectIndication _connectIndication;

        internal Connection(CapiApplication application, Controller controller,
            uint plci, string calledPartyNumber, string callingPartyNumber) {

            _dtfmDuration = 40;
            _dtfmPause = 40;
            _application = application;
            _controller = controller;
            _plci = plci;
            _calledPartyNumber = calledPartyNumber;
            _callingPartyNumber = callingPartyNumber;
            _status = ConnectionStatus.Disconnected;
        }

        public uint PLCI {
            get { return _plci; }
        }

        public CapiApplication Application {
            get { return _application; }
        }
        public Controller Controller {
            get { return _controller; }
        }

        public ConnectionStatus Status {
            get { return _status; }
            internal set {
                _status = value; 
                _application.OnConnectionStatusChanged(new ConnectionEventArgs(this));
            }
        }

        public bool Inititator {
            get { return _inititator; }
            internal set { _inititator = value; }
        }

        public string CalledPartyNumber {
            get { return _calledPartyNumber; }
        }

        public string CallingPartyNumber {
            get { return _callingPartyNumber; }
        }

        public uint NCCI {
            get { return _ncci; }
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

        
        public void SendDTFMTone(string digits) {
            if (_status != ConnectionStatus.Connected)
                throw Error.NotSupported();

            DTMFFacilityRequestParameter dtfmParam = new DTMFFacilityRequestParameter();
            FacilityRequest request = new FacilityRequest(dtfmParam);

            request.FacilitySelector = FacilitySelector.DTMF;
            dtfmParam.FacilityFunction = FacilityFunction.Send;
            dtfmParam.ToneDuration = _dtfmDuration;
            dtfmParam.GapDuration = _dtfmPause;
            dtfmParam.Digits = digits;
            request.Identifier.Value = _plci;
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

        private void UpdateDTFMState() {

            if (_dtfmListen && !_listen)
                ThreadPool.QueueUserWorkItem(StartListenDTFM);
            else if (!_dtfmListen && _listen)
                ThreadPool.QueueUserWorkItem(StopListenDTFM);
        }

        private void StartListenDTFM(object o) {
            Debug.Assert(!_listen);
            DTMFFacilityRequestParameter dtfmParam = new DTMFFacilityRequestParameter();
            FacilityRequest request = new FacilityRequest(dtfmParam);

            request.FacilitySelector = FacilitySelector.DTMF;
            dtfmParam.FacilityFunction = FacilityFunction.StartListen;
            dtfmParam.ToneDuration = _dtfmPause;
            dtfmParam.GapDuration = _dtfmPause;
            request.Identifier.Value = _plci;
            RequestDTFM(request);
            _listen = true;
        }

        private void StopListenDTFM(object o) {
            Debug.Assert(_listen);
            DTMFFacilityRequestParameter dtfmParam = new DTMFFacilityRequestParameter();
            FacilityRequest request = new FacilityRequest(dtfmParam);

            request.FacilitySelector = FacilitySelector.DTMF;
            dtfmParam.FacilityFunction = FacilityFunction.StopListen;
            request.Identifier.Value = _plci;
            RequestDTFM(request);
            _listen = false;
        }

        #region Hangup

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
                Message request = null;
                switch (_status) {
                    case ConnectionStatus.Connected:
                    case ConnectionStatus.B_ConnectPending:
                        _inititator = true;
                        request = new DisconnectB3Request(_ncci);
                        break;
                    case ConnectionStatus.D_Connected:
                    case ConnectionStatus.D_ConnectPending:
                    case ConnectionStatus.B_DisconnectPending:
                        _inititator = true;
                        request = new DisconnectRequest(_plci);
                        break;
                    default:
                        throw Error.NotSupported();
                }
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

        #endregion HangUp

        public void Answer(Reject reject, B1Protocol b1, B2Protocol b2, B3Protocol b3) {
            Debug.Assert(_status == ConnectionStatus.D_ConnectPending);
            if (_connectIndication == null || _status != ConnectionStatus.D_ConnectPending) {
                throw new NotSupportedException("Connection is not in the right state.");
            }
            ConnectResponse response = new ConnectResponse(_connectIndication);
            _connectIndication = null;
            response.Reject = reject;
            response.BPtotocol.B1 = b1;
            response.BPtotocol.B2 = b2;
            response.BPtotocol.B3 = b3;
            _application.SendMessage(response);
        }

        public override string ToString() {
            return string.Format("Connection {0} on Controller {1}", _plci, _controller.ID);
        }

        internal void ConnectIndication(ConnectIndication indication) {
            _connectIndication = indication;
        }

        internal void ConnectActiveIndication(ConnectActiveIndication indication) { 
            try {
                ConnectActiveResponse response = new ConnectActiveResponse(indication);
                _application.SendMessage(response);
                Status = ConnectionStatus.D_Connected;
                if (_inititator) {
                    ConnectB3Request request = new ConnectB3Request();
                    request.Identifier.Value = response.Identifier.Value;
                    _application.SendMessage(request);
                }
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::ConnectActiveIndication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void ConnectB3Indication(ConnectB3Indication indication) {
            try {
                _ncci = indication.Identifier.NCCI;
                ConnectB3Response response = new ConnectB3Response(indication);
                _application.SendMessage(response);
                Status = ConnectionStatus.B_ConnectPending;
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::ConnectB3Indication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void ConnectB3ActiveIndication(ConnectB3ActiveIndication indication) {
            try {
                _inititator = false;
                if (_ncci == INVAL_NCCI) {
                    _ncci = indication.Identifier.NCCI;
                }
                ConnectB3ActiveResponse response = new ConnectB3ActiveResponse(indication);
                _application.SendMessage(response);
                Status = ConnectionStatus.Connected;
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::ConnectionB3ActiveIndication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void DisconnectB3Indication(DisconnectB3Indication indication) {
            try {
                _ncci = INVAL_NCCI;
                DisconnectB3Response response = new DisconnectB3Response(indication);
                _application.SendMessage(response);
                Status = ConnectionStatus.D_Connected;
                if (_inititator) {
                    DisconnectRequest request = new DisconnectRequest(_plci);
                    _application.SendMessage(request);
                } 
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::DisconnectB3Indication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void FacilityIndication(FacilityIndication indication) {
            Trace.TraceInformation("Connection#{0}::FacilityIndication, Digits = {0}", indication.Digits);

            try {
                _application.OnDTFMIndication(new DTFMEventArgs(this, indication.Digits));
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::FacilityIndication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void DataB3Indication(DataB3Indication indication) {
            try {
                DataB3Response response = new DataB3Response(indication);
                _application.SendMessage(response);
            } catch (Exception e) {
                Trace.TraceError("Connection#{0}::DataB3Indication, Exception = {1}", ValidationHelper.HashString(this), e);
                throw;
            }
        }

        internal void ConnectB3Confirmation(ConnectB3Confirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Connection#{0}::ConnectB3Confirmation, Info = {0}", confirmation.Info);
            if (confirmation.Succeeded) {
                _ncci = confirmation.Identifier.NCCI;
                Status = ConnectionStatus.B_ConnectPending;
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

        internal void DisconnectConfirmation(DisconnectConfirmation confirmation, MessageAsyncResult result) {
            Trace.TraceInformation("Connection#{0}::DisconnectConfirmation, Info = {0}", confirmation.Info);
            if (confirmation.Succeeded) {
                this.Status = ConnectionStatus.D_DisconnectPending;
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


        private void RequestWaitCallback(object state) {
            _application.SendRequestMessage((MessageAsyncResult)state);
        }
    }
}
