namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum FacilityFunction : short {
        /// <summary>
        /// Start DTMF listen on B channel data.
        /// </summary>
        StartListen = 1,
        /// <summary>
        /// Stop DTMF listen
        /// </summary>
        StopListen = 2,
        /// <summary>
        /// Send DTMF digits
        /// </summary>
        Send = 3
    }
}
