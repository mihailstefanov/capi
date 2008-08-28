namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    public class CapiStreamWriter {
        //max packet size for this application, 2048 bytes seems to be CAPI maximum.
        private const int MaxBufferSize = 128;
        private Connection _connection;

        private class StateObject {
            public Stream Stream;
        }

        public CapiStreamWriter(Connection connection) {
            _connection = connection;
        }

        public void Write(string filename) {
            FileStream stream = File.OpenRead(filename);
            Write(stream);
        }

        public void Write(Stream stream) {
            StateObject o = new StateObject();
            o.Stream = stream;
            byte[] bytes = StreamToBytes(stream);
            if (bytes.Length != 0) {
                _connection.BeginWriteData(bytes, 0, EndWriteAsyncCallback, o);
            }
        }

        protected static byte[] StreamToBytes(Stream stream) {

            if (stream.Length == 0 || stream.Position == stream.Length) {
                return new byte[0];
            }
            int length = ((int)stream.Length < MaxBufferSize) ? (int)stream.Length : MaxBufferSize;
            byte[] buffer = new byte[MaxBufferSize];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }



        private void EndWriteAsyncCallback(IAsyncResult asyncResult) {
            StateObject o = null;
            MessageAsyncResult result = asyncResult as MessageAsyncResult;
            if (result != null) {
                try {
                    o = (StateObject)asyncResult.AsyncState;
                    byte[] bytes = StreamToBytes(o.Stream);
                    if (bytes.Length != 0) {
                        _connection.BeginWriteData(bytes, 0, EndWriteAsyncCallback, o);
                    } else {
                        o.Stream.Close();
                    }
                } catch (Exception e) {
                    if (o != null && o.Stream != null) {
                        o.Stream.Close();
                    }
                    Trace.TraceError("CapiStreamWriter#{0}::EndWriteAsyncCallback, Exception = {1}", ValidationHelper.HashString(this), e);
                }
            }
        }
    }
}
