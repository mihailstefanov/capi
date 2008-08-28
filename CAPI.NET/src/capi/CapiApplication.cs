namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;
    using System.Threading;
    using System.IO;
    using System.Diagnostics;

    public partial class CapiApplication : Component {
        private static short s_messageNumber = 0;
        private static object s_messageNumberLockObject = new object();
        private Thread _messageQueueThread;
        /// <summary>
        /// The maximum number of logical connections this application can maintain concurrently.
        /// </summary>
        private const int MaxLogicalConnection = 10;

        /// <summary>
        /// The maximum number of received data
        /// blocks that can be reported to the application simultaneously for each logical connection.
        /// </summary>
        private const int MaxBDataBlocks = 2;

        /// <summary>
        /// The maximum size of the application data block to be transmitted and received.
        /// </summary>
        private const int MaxBDataLen = 128;

        private const int BlockLenght = 1024;

        private const int AppIDPlaceHolder = 0;

        private int _appID;

        private int _bufferLength;

        private ControllerCollection _controllers;

        private CapiSerializer _serializer;

        private AsyncResultDictionary _asyncDictionary = new AsyncResultDictionary();

        public CapiApplication() {
            _bufferLength = GetBufferLenght(MaxLogicalConnection);
            _appID = CapiPInvoke.Register(_bufferLength, MaxLogicalConnection, MaxBDataBlocks, MaxBDataLen);
            _serializer = new CapiSerializer(this);
            _messageQueueThread = new Thread(WaitForConfirmation);
            _messageQueueThread.IsBackground = true;
            _messageQueueThread.Name = string.Format("CAPI Application: {0} message queue", _appID);
            _messageQueueThread.Start();
        }

        protected override void Dispose(bool disposing) {
            if (_appID != AppIDPlaceHolder) {
                CapiPInvoke.Release(_appID);
                _appID = AppIDPlaceHolder;
            }
            base.Dispose(disposing);
        }



        public int AppID {
            get { return _appID; }
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
            for (Int16 i = 1; i <= profile.number; i++) {
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

        internal Controller GetControllerByID(int id) {
            return Controllers.GetControllerByID(id);
        }

        internal MessageAsyncResult GetMessageAsyncResult(short id) {
            return _asyncDictionary.GetSafe(id);
        }

        internal void RemoveMessageAsyncResult(short id) {
            _asyncDictionary.RemoveSafe(id);
        }

        //private void OnConfirmation(MessageHeader header, Message message)
        //{ 


        //                case Command.Facility:
        //                    result.Caller.FacilityConfirmation(header, (FacilityConfirmation)message, result);
        //                    break;
        //                case Command.DisconnectB3:
        //                    result.Caller.DisconnectB3Confirmation(header, (DisconnectB3Confirmation)message, result);
        //                    break;
        //                case Command.DataB3:
        //                    result.Caller.DataB3Confirmation(header, (DataB3Confirmation)message, result);
        //                    break;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Trace.TraceError("CapiApplication#{0}::OnConfirmation, Exception = {1}", ValidationHelper.HashString(this), e);

        //            // HACK check to see if result is complited.
        //            if (!result.IsCompleted)
        //            {
        //                result.InvokeCallback(e);
        //            }
        //            throw;
        //        }

        //        _asyncDictionary.RemoveSafe(header.ID);
        //    }
        //}

        internal void SendMessage(Message message) {
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


        public Version GetVersion() {
            return CapiPInvoke.GetVersion();
        }

        public string GetManufacturer() {
            return CapiPInvoke.GetManufacturer();
        }

        public string GetSerialNumber() {
            return CapiPInvoke.GetSerialNumber();
        }


        private short GetUniqueMessageNumber() {
            lock (s_messageNumberLockObject) {
                return ++s_messageNumber;
            }
        }

        private int GetBufferLenght(int logicalConnection) {
            return BlockLenght + (BlockLenght * logicalConnection);
        }
    }
}
