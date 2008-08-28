namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum B1Protocol {
        /// <summary>
        /// 64 kbit/s with HDLC framing (default).
        /// </summary>
        HDLC64 = 0,
        /// <summary>
        /// 64 kbit/s bit-transparent operation with byte framing from the network.
        /// </summary>
        HDLC64BFN = 1,
        /// <summary>
        /// V.110 asynchronous operation with start/stop byte framing (see Note 1)
        /// </summary>
        V110ASYNC = 2,
        /// <summary>
        /// V.110 synchronous operation with HDLC framing (see Note 2)
        /// </summary>
        V110SYNC = 3
    }
}
