namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;

    public class ConnectionWriter : IDisposable {
        ////max packet size for this application, 2048 bytes seems to be CAPI maximum.
        private ConnectionStream _outStream;
        private int _BDataLenght;
        private int _BDataBlocks;

        private class StateObject {
            public Stream Stream;
            public Semaphore SyncObject;
        }

        public ConnectionWriter(ConnectionStream stream) {
            if (stream == null) throw new ArgumentNullException("stream");
            _outStream = stream;
            _BDataBlocks = _outStream.Connection.Application.BDataBlocks;
            _BDataLenght = _outStream.Connection.Application.BDataLenght;
        }

        public ConnectionWriter(ConnectionStream stream, int BDataBlocks, int BDataLenght) {
            if (stream == null) throw new ArgumentNullException("stream");
            if (BDataBlocks <= 0) throw new ArgumentException("BDataBlocks");
            if (BDataLenght <= 0) throw new ArgumentException("BDataLenght");
            _outStream = stream;
            _BDataBlocks = BDataBlocks;
            _BDataLenght = BDataLenght;
        }

        public void Close() {
            Dispose(true);
        }

        void IDisposable.Dispose() {
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
            using (Semaphore syncObject = new Semaphore(_BDataBlocks, _BDataBlocks)) {
                StateObject state = new StateObject();
                state.Stream = _outStream;
                state.SyncObject = syncObject;

                byte[] buf = new byte[_BDataLenght];
                int bytesRead = stream.Read(buf, 0, buf.Length);
                while (bytesRead > 0) {
                    IAsyncResult result = _outStream.BeginWrite(buf, 0, bytesRead, EndWriteAsyncCallback,
                        state);
                    syncObject.WaitOne();
                    bytesRead = stream.Read(buf, 0, buf.Length);
                }
            }
        }

        private static void EndWriteAsyncCallback(IAsyncResult result) {
            StateObject state = result.AsyncState as StateObject;
            try {
                state.Stream.EndWrite(result);
            } catch (Exception e) {
                Trace.TraceError("ConnectionWrite::EndWriteAsyncCallback, Exception = {0}", e);
            }
            state.SyncObject.Release();
        }

    }
}
