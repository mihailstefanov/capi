namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Flags]
    public enum Reasons : short {
        /// <summary>
        /// No cause available.
        /// </summary>
        NoCauseAvailable = 0,
        /// <summary>
        /// Protocol error, Layer 1.
        /// </summary>
        ProtocolErrorLayer1 = 0x3301,
        /// <summary>
        /// Protocol error, Layer 2.
        /// </summary>
        ProtocolErrorLayer2 = 0x3302,
        /// <summary>
        /// Protocol error, Layer 3.
        /// </summary>
        ProtocolErrorLayer3 = 0x3303,
        /// <summary>
        /// Another application got that call.
        /// </summary>
        AnotherGotTheCall = 0x3304,
        /// <summary>
        /// Cleared by Call Control Supervision.
        /// </summary>
        Cleared = 0x3305
    }
}
