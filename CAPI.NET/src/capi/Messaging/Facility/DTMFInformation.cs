namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum DTMFInformation : short {
        /// <summary>
        /// Sending of DTMF info successfully initiated.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Incorrect DTMF digit.
        /// </summary>
        IncorrectDigi = 1,
        /// <summary>
        /// Unknown DTMF request
        /// </summary>
        UnknownRequest = 2
    }
}
