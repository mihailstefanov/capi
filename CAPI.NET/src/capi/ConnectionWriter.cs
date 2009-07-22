namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;

    public class ConnectionWriter : IDisposable {
        private const int WriteTimeout = 10000; // 10 sec
        ////max packet size for this application, 2048 bytes seems to be CAPI maximum.
        private ConnectionStream _outStream;
        private int _BDataLenght;
        private int _BDataBlocks;
        private bool _reverse;
        private Semaphore _syncObject;
        private class StateObject {
            public Stream Stream;
            public Semaphore SyncObject;
            public Exception Exception;
        }

        public ConnectionWriter(ConnectionStream stream)
            : this(stream, stream.Connection.Application.BDataBlocks,
            stream.Connection.Application.BDataLenght) {

        }

        public ConnectionWriter(ConnectionStream stream, int BDataBlocks, int BDataLenght) {
            if (stream == null) throw new ArgumentNullException("stream");
            if (BDataBlocks <= 0) throw new ArgumentException("BDataBlocks");
            if (BDataLenght <= 0) throw new ArgumentException("BDataLenght");
            _outStream = stream;
            _BDataBlocks = BDataBlocks;
            _BDataLenght = BDataLenght;
            _syncObject = new Semaphore(_BDataBlocks, _BDataBlocks);
        }

        void IDisposable.Dispose() {
            Dispose(true);
        }


        public bool Reverse {
            get { return _reverse; }
            set { _reverse = value; }
        }


        public void Close() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                _outStream.Close();
            }
        }

        public void Write(string filename) {
            using (FileStream stream = File.OpenRead(filename)) {
                Write(stream);
            }
        }

        public void Write(Stream stream) {
            StateObject state = new StateObject();
            state.Stream = _outStream;
            state.SyncObject = _syncObject;

            byte[] buf = new byte[_BDataLenght];
            int bytesRead = stream.Read(buf, 0, buf.Length);
            while (bytesRead > 0) {
                if (_reverse) ReverseBytes(buf, 0, bytesRead);
                IAsyncResult result = _outStream.BeginWrite(buf, 0, bytesRead, EndWriteAsyncCallback,
                    state);
                // if timeout or excheption is thrown it is may be better to break the loop.
                if (_syncObject.WaitOne(WriteTimeout) && state.Exception == null) {
                    bytesRead = stream.Read(buf, 0, buf.Length);
                } else {
                    break;
                }
            }
        }

        private static void EndWriteAsyncCallback(IAsyncResult result) {
            StateObject state = result.AsyncState as StateObject;
            try {
                state.Stream.EndWrite(result);

            } catch (Exception e) {
                Trace.TraceError("ConnectionWrite::EndWriteAsyncCallback, Exception = {0}", e);
                state.Exception = e;
            } finally {
                state.SyncObject.Release();
            }
        }

        private static void ReverseBytes(byte[] bytes, int offset, int count) {
            for (int i = offset; i < count; i++) {
                bytes[i] = ReverseByte(bytes[i]);
            }
        }

        private static byte ReverseByte(byte inByte) {
            //inByte |= 0x55;
            byte result = 0x00;
            byte mask = 0x00;

            for (mask = 0x80;
                                Convert.ToInt32(mask) > 0;
                                mask >>= 1) {
                result >>= 1;
                byte tempbyte = (byte)(inByte & mask);
                if (tempbyte != 0x00)
                    result |= 0x80;
            }
            return (result);
        }
    }
}
