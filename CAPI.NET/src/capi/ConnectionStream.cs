using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mommosoft.Capi {
    public class ConnectionStream : Stream {
        private Connection _connection;

        public ConnectionStream(Connection connection) {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        public Connection Connection {
            get { return _connection; }
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
                // data block cannot be bigger than the value which we used in CAPI_REGISTER call.
                if (count < 0 || count > _connection.Application.BDataLenght) throw new ArgumentOutOfRangeException("count");
                if ((buffer.Length - offset) < count) throw new ArgumentException("Invalid offset");
                if (_connection.Status != ConnectionStatus.Connected)
                    throw new NotSupportedException();
                Debug.Assert(count <= short.MaxValue);
                if (count >= short.MaxValue) throw new NotSupportedException();

                ptr = Marshal.AllocHGlobal(count);
                Marshal.Copy(buffer, offset, ptr, count);
                // build request
                DataB3Request request = new DataB3Request();
                request.Identifier.NCCI = _connection.NCCI;
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
    }
}
