namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;

    internal class MessageAsyncResult : IAsyncResult {

        private const int HighBit = unchecked((int)0x80000000);
        private const int ForceAsyncCount = 50;

        //private object _asyncObject = null;       // Caller's async object.
        private object _asyncState;                 // Caller's state object.
        private AsyncCallback _asyncCallback;       // Caller's callback method.
        private bool _userEvent;                    // true if the event has been (or is about to be) handed to the user
        private object _event;                      // lazy allocated event to be returned in the IAsyncResult for the client to wait on
        private int _completed;                     // Sign bit indicates synchronous completion if set.
        // Remaining bits count the number of InvokeCallbak() calls.
        private object _result;
        private Message _request;
        private object _caller;

        public MessageAsyncResult(object caller, Message request, AsyncCallback callback, object state) {
            _caller = caller;
            _request = request;
            _asyncCallback = callback;
            _asyncState = state;
            Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::.ctor()");
        }


        private class ThreadContext {
            internal int _nestedCount;
        }

        [ThreadStatic]
        private static ThreadContext t_ThreadContext;


        private static ThreadContext CurrentThreadContext {
            get {
                ThreadContext threadContext = t_ThreadContext;
                if (threadContext == null) {
                    threadContext = new ThreadContext();
                    t_ThreadContext = threadContext;
                }
                return threadContext;
            }
        }

        public object AsyncState {
            get { return _asyncState; }
        }

        public WaitHandle AsyncWaitHandle {
            get {
                Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::get_AsyncWaitHandle()");

                ManualResetEvent asyncEvent;
                // Indicates that the user has seen the event; it can't be disposed.
                _userEvent = true;

                // The user has access to this object.  Lock-in CompletedSynchronously.
                if (_completed == 0) {
                    Interlocked.CompareExchange(ref _completed, HighBit, 0);
                }

                // Because InternalWaitForCompletion() tries to dispose this event, it's
                // possible for m_Event to become null immediately after being set, but only if
                // IsCompleted has become true.  Therefore it's possible for this property
                // to give different (set) events to different callers when IsCompleted is true.
                asyncEvent = (ManualResetEvent)_event;
                while (asyncEvent == null) {
                    LazilyCreateEvent(out asyncEvent);
                }
                Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::get_AsyncWaitHandle() m_Event:" + ValidationHelper.HashString(_event));
                return asyncEvent;
            }
        }

        // Returns true if this call created the event.
        // May return with a null handle.  That means it thought it got one, but it was disposed in the mean time.
        private bool LazilyCreateEvent(out ManualResetEvent waitHandle) {
            // lazy allocation of the event:
            // if this property is never accessed this object is never created
            waitHandle = new ManualResetEvent(false);
            try {
                if (Interlocked.CompareExchange(ref _event, waitHandle, null) == null) {
                    if (InternalPeekCompleted) {
                        waitHandle.Set();
                    }
                    return true;
                } else {
                    waitHandle.Close();
                    waitHandle = (ManualResetEvent)_event;
                    // There's a chance here that m_Event became null.  But the only way is if another thread completed
                    // in InternalWaitForCompletion and disposed it.  If we're in InternalWaitForCompletion, we now know
                    // IsCompleted is set, so we can avoid the wait when waitHandle comes back null.  AsyncWaitHandle
                    // will try again in this case.
                    return false;
                }
            } catch {
                // This should be very rare, but doing this will reduce the chance of deadlock.
                _event = null;
                if (waitHandle != null)
                    waitHandle.Close();
                throw;
            }
        }

        public bool CompletedSynchronously {
            get {
                // If this returns greater than zero, it means it was incremented by InvokeCallback before anyone ever saw it.
                int result = _completed;
                if (result == 0) {
                    result = Interlocked.CompareExchange(ref _completed, HighBit, 0);
                }
                return result > 0;
            }
        }

        public bool IsCompleted {
            get {
                Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::get_IsCompleted()");

                // Look at just the low bits to see if it's been incremented.  If it hasn't, set the high bit
                // to show that it's been looked at.
                int result = _completed;
                if (result == 0) {
                    result = Interlocked.CompareExchange(ref _completed, HighBit, 0);
                }
                return (result & ~HighBit) != 0;
            }
        }

        // Use to see if something's completed without fixing CompletedSynchronously
        internal bool InternalPeekCompleted {
            get {
                return (_completed & ~HighBit) != 0;
            }
        }

        // A method for completing the IO with a result
        // and invoking the user's callback.
        // Used by derived classes to pass context into an overridden Complete().  Useful
        // for determining the 'winning' thread in case several may simultaneously call
        // the equivalent of InvokeCallback().
        protected void ProtectedInvokeCallback(object result) {
            Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::ProtectedInvokeCallback() result = " +
                          (result is Exception ? ((Exception)result).Message : result == null ? "<null>" : result.ToString()));

            if ((_completed & ~HighBit) == 0 && (Interlocked.Increment(ref _completed) & ~HighBit) == 1) {
                // DBNull.Value is used to guarantee that the first caller wins,
                // even if the result was set to null.
                if (_result != DBNull.Value)
                    _result = result;

                // Does this need a memory barrier to be sure this thread gets the m_Event if it's set?  I don't think so
                // because the Interlockeds on m_IntCompleted/m_Event should serve as the barrier.
                ManualResetEvent asyncEvent = (ManualResetEvent)_event;
                if (asyncEvent != null) {
                    asyncEvent.Set();
                }

                Complete();
            }
        }

        // A method for completing the IO with a result
        // and invoking the user's callback.
        internal void InvokeCallback(object confirmation) {
            ProtectedInvokeCallback(confirmation);
        }

        // A method for completing the IO without a result
        // and invoking the user's callback.
        internal void InvokeCallback() {
            ProtectedInvokeCallback(null);
        }
        //
        //  MUST NOT BE CALLED DIRECTLY
        //  A protected method that does callback job and it is guaranteed to be called exactly once.
        //  A derived overriding method must call the base class somewhere or the completion is lost.
        //
        protected virtual void Complete() {
            bool offloaded = false;
            ThreadContext threadContext = CurrentThreadContext;
            try {
                ++threadContext._nestedCount;
                if (_asyncCallback != null) {
                    Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::Complete() invoking callback");

                    if (threadContext._nestedCount >= ForceAsyncCount) {
                        Trace.TraceInformation("MessageAsyncResult::Complete *** OFFLOADED the user callback ***");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(WorkerThreadComplete));
                        offloaded = true;
                    } else {
                        _asyncCallback(this);
                    }
                } else {
                    Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::Complete() no callback to invoke");
                }
            } finally {
                --threadContext._nestedCount;

                // Never call this method unless interlocked m_IntCompleted check has succeeded (like in this case)
                if (!offloaded) {
                    Cleanup();
                }
            }
        }

        // Only called in the above method
        void WorkerThreadComplete(object state) {
            try {
                _asyncCallback(this);
            } finally {
                Cleanup();
            }
        }

        // Custom instance cleanup method.
        // Derived types override this method to release unmanaged resources associated with an IO request.
        protected virtual void Cleanup() {
        }

        // Internal property for setting the IO result.
        internal object Result {
            get {
                return _result;
            }
            set {
                // Ideally this should never be called, since setting
                // the result object really makes sense when the IO completes.
                //
                // But if the result was set here (as a preemptive error or for some other reason),
                // then the "result" parameter passed to InvokeCallback() will be ignored.
                //

                // It's an error to call after the result has been completed or with DBNull.
                Debug.Assert(value != DBNull.Value, "MessageAsyncResult#{0}::set_Result()|Result can't be set to DBNull - it's a special internal value.", ValidationHelper.HashString(this));
                Debug.Assert(!InternalPeekCompleted, "MessageAsyncResult#{0}::set_Result()|Called on completed result.", ValidationHelper.HashString(this));

                _result = value;
            }
        }

        internal Message Request {
            get { return _request; }
        }


        internal object Caller {
            get { return _caller; }
        }

        internal object InternalWaitForCompletion() {
            return WaitForCompletion(true);
        }

        private object WaitForCompletion(bool snap) {
            ManualResetEvent waitHandle = null;
            bool createdByMe = false;
            bool complete = snap ? IsCompleted : InternalPeekCompleted;

            if (!complete) {
                // Not done yet, so wait:
                waitHandle = (ManualResetEvent)_event;
                if (waitHandle == null) {
                    createdByMe = LazilyCreateEvent(out waitHandle);
                }
            }

            if (waitHandle != null) {
                try {
                    Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::InternalWaitForCompletion() Waiting for completion _event#" + ValidationHelper.HashString(waitHandle));
                    waitHandle.WaitOne(Timeout.Infinite, false);
                } catch (ObjectDisposedException) {
                    // This can occur if this method is called from two different threads.
                    // This possibility is the trade-off for not locking.
                } finally {
                    // We also want to dispose the event although we can't unless we did wait on it here.
                    if (createdByMe && !_userEvent) {
                        // Does _userEvent need to be volatile (or _event set via Interlocked) in order
                        // to avoid giving a user a disposed event?
                        ManualResetEvent oldEvent = (ManualResetEvent)_event;
                        _event = null;
                        if (!_userEvent) {
                            oldEvent.Close();
                        }
                    }
                }
            }

            // A race condition exists because InvokeCallback sets m_IntCompleted before _result (so that m_Result
            // can benefit from the synchronization of _completed).  That means you can get here before m_Result got
            // set (although rarely - once every eight hours of stress).  Handle that case with a spin-lock.
            while (_result == DBNull.Value)
                Thread.SpinWait(1);

            Trace.TraceInformation("MessageAsyncResult#" + ValidationHelper.HashString(this) + "::InternalWaitForCompletion() done: " +
                            (_result is Exception ? ((Exception)_result).Message : _result == null ? "<null>" : _result.ToString()));

            return _result;
        }
    }
}
