namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;
    using System.Threading;
    using System.IO;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public partial class CapiApplication : Component {
        private static short s_messageNumber = 0;
        private static object s_messageNumberLockObject = new object();
        private static readonly TimeSpan MessageQueueTimeout = new TimeSpan(0, 0, 0, 10);
        private Thread _messageQueueThread;
        /// <summary>
        /// The maximum number of logical connections this application can maintain concurrently.
        /// </summary>
        private const int MaxLogicalConnection = 10;

        /// <summary>
        /// The maximum number of received data
        /// blocks that can be reported to the application simultaneously for each logical connection.
        /// </summary>
        private const int MaxBDataBlocks = 7;

        /// <summary>
        /// The maximum size of the application data block to be transmitted and received.
        /// </summary>
        private const int MaxBDataLenght = 2048;

        private const int BlockLenght = 1024;

        private const int AppIDPlaceHolder = 0;

        private int _appID;
        private readonly int _BDataLenght;
        private readonly int _BDataBlocks;
        private ControllerCollection _controllers;
        private CapiSerializer _serializer;

        private AsyncResultDictionary _asyncDictionary = new AsyncResultDictionary();

        public CapiApplication()
            : this(GetBufferLenght(MaxLogicalConnection), MaxLogicalConnection, MaxBDataBlocks, MaxBDataLenght) {

        }

        public CapiApplication(int messageBufferLenght, int maxLogicalConnections, int maxBDataBlocks, int maxBDataLen) {
            _BDataBlocks = maxBDataBlocks;
            _BDataLenght = maxBDataLen;

            _appID = CapiPInvoke.Register(messageBufferLenght, maxLogicalConnections, maxBDataBlocks, maxBDataLen);
            _serializer = new CapiSerializer(this);
            _messageQueueThread = new Thread(WaitForConfirmation);
            _messageQueueThread.IsBackground = true;
            _messageQueueThread.Name = string.Format("CAPI Application: {0} message queue", _appID);
            _messageQueueThread.Start();

        }

        protected override void Dispose(bool disposing) {
            if (_appID != AppIDPlaceHolder) {
                _run = false;
                SendFakeListenRequest();
                _messageQueueThread.Join(MessageQueueTimeout);
                CapiPInvoke.Release(_appID);
                _appID = AppIDPlaceHolder;
            }
            base.Dispose(disposing);
        }

        private void SendFakeListenRequest() {
            ListenRequest request = new ListenRequest(0);
            request.CIPMask = CIPMask.None;
            SendMessage(request);
        }

        public int AppID {
            get { return _appID; }
        }

        public int BDataLenght {
            get { return _BDataLenght; }
        }

        public int BDataBlocks {
            get { return _BDataBlocks; }
        }

        public ControllerCollection Controllers {
            get {
                if (_controllers == null) {
                    _controllers = CreateControllerCollection();
                }
                return _controllers;
            }
        }

        private ControllerCollection CreateControllerCollection() {
            NativeMethods.Profile profile = CapiPInvoke.GetProfile(0);
            List<Controller> controllerList = new List<Controller>();
            for (UInt16 i = 1; i <= profile.number; i++) {
                controllerList.Add(new Controller(this, i));
            }
            return new ControllerCollection(controllerList);
        }

        private bool _run = true;

        private void WaitForConfirmation() {
            while (_run) {
                try {
                    CapiPInvoke.WaitForSignal(_appID);
                    using (MemoryStream stream = CapiPInvoke.GetMessage(_appID)) {
                        MessageHeader header;
                        Message message = _serializer.Deserialize(stream, out header);

                        Trace.TraceInformation("CapiApplication#" + ValidationHelper.HashString(this)
                            + "WaitForConfirmation header  = " + header.Command.ToString() + "," + header.SubCommand.ToString());

                        message.Notify(this);
                    }
                } catch (Exception e) {
                    Trace.TraceError("CapiApplication#{0}::WaitForConfirmation, Exception = {1}", ValidationHelper.HashString(this), e);
                }
            }
        }

        internal Controller GetControllerByID(uint id) {
            return Controllers.GetControllerByID(id);
        }

        internal MessageAsyncResult GetMessageAsyncResult(short id) {
            return _asyncDictionary.GetSafe(id);
        }

        internal void RemoveMessageAsyncResult(short id) {
            _asyncDictionary.RemoveSafe(id);
        }

        public void SendMessage(Message message) {
            using (MemoryStream stream = new MemoryStream()) {
                if (message.Identity.SubCommand == SubCommand.Request && message.Number == 0) {
                    message.Number = GetUniqueMessageNumber();
                }
                _serializer.Serialize(stream, message);
                CapiPInvoke.PutMessage(_appID, stream);
            }
        }

        internal void SendRequestMessage(MessageAsyncResult result) {
            result.Request.Number = GetUniqueMessageNumber();
            _asyncDictionary.AddSafe(result.Request.Number, result);
            SendMessage(result.Request);
        }


        public Version CapiVersion {
            get {
                return CapiPInvoke.GetVersion();
            }
        }

        public string Manufacturer {
            get {
                return CapiPInvoke.GetManufacturer();
            }
        }

        public string SerialNumber {
            get {
                return CapiPInvoke.GetSerialNumber();
            }
        }

        private short GetUniqueMessageNumber() {
            lock (s_messageNumberLockObject) {
                return ++s_messageNumber;
            }
        }

        private static int GetBufferLenght(int logicalConnection) {
            return BlockLenght + (BlockLenght * logicalConnection);
        }
    }
}
