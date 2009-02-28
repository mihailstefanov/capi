using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mommosoft.Capi {
    public class ConnectionStream : Stream {
        //max packet size for this application, 2048 bytes seems to be CAPI maximum.
        private const int MaxBufferSize = 128;
        private Connection _connection;

        //private class StateObject {
        //    public byte[] Source;
        //    public int Position;
        //    public int Count;
        //}

        public ConnectionStream(Connection connection) {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanSeek {
            get { return false; }
        }

        public override bool CanWrite {
            get { return true; }
        }

        public override void Flush() {
            throw Error.NotSupported();
        }

        public override long Length {
            get { throw Error.NotSupported(); }
        }

        public override long Position {
            get {
                throw Error.NotSupported();
            }
            set {
                throw Error.NotSupported();
            }
        }

        public override int Read(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw Error.NotSupported();
        }

        public override void SetLength(long value) {
            throw Error.NotSupported();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            IAsyncResult result = BeginWrite(buffer, offset, count, null, null);
            EndWrite(result);
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            throw new NotImplementedException();
        }

        public override int EndRead(IAsyncResult asyncResult) {
            throw new NotImplementedException();
        }
        short _dataHandle = 0;

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            IntPtr ptr = IntPtr.Zero;
            try {

                if (buffer == null) throw new ArgumentNullException("buffer");
                if (offset < 0 || offset > short.MaxValue) throw new ArgumentOutOfRangeException("offset");
                if (count < 0 || count > short.MaxValue) throw new ArgumentOutOfRangeException("count");
                if ((buffer.Length - offset) < count) throw new ArgumentException("Invalid offset");
                //if (_connection.Status != ConnectionStatus.Connected)
                //    throw new NotSupportedException();
                Debug.Assert(count <= short.MaxValue);
                if (count >= short.MaxValue) throw new NotSupportedException();

                ptr = Marshal.AllocHGlobal(count);
                Marshal.Copy(buffer, offset, ptr, count);
                // build request
                DataB3Request request = new DataB3Request();
                request.Identifier.NCCI  = _connection.NCCI;
                request.Data = ptr;
                request.DataLength = (short)count;
                _dataHandle++;
                request.DataHandle = _dataHandle;

                MessageAsyncResult result = new MessageAsyncResult(this, request, callback, state);
                _connection.Application.SendRequestMessage(result);
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

        public override void EndWrite(IAsyncResult asyncResult) {
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
        //protected static byte[] BeginWriteAsync(StateObject state, int maxBufferSize) {


        //    int length = (state.Count < MaxBufferSize) ? (int)stream.Length : MaxBufferSize;
        //    byte[] buffer = new byte[MaxBufferSize];
        //    stream.Read(buffer, 0, buffer.Length);
        //    return buffer;
        //}



        //private void EndWriteAsyncCallback(IAsyncResult asyncResult) {
        //    StateObject o = null;
        //    MessageAsyncResult result = asyncResult as MessageAsyncResult;
        //    if (result != null) {
        //        try {
        //            o = (StateObject)asyncResult.AsyncState;
        //            byte[] bytes = BeginWriteAsync(o.Stream);
        //            if (bytes.Length != 0) {
        //                _connection.BeginWriteData(bytes, 0, EndWriteAsyncCallback, o);
        //            } else {
        //                o.Stream.Close();
        //            }
        //        } catch (Exception e) {
        //            if (o != null && o.Stream != null) {
        //                o.Stream.Close();
        //            }
        //            Trace.TraceError("CapiStreamWriter#{0}::EndWriteAsyncCallback, Exception = {1}", ValidationHelper.HashString(this), e);
        //        }
        //    }
        //}
    }
}
