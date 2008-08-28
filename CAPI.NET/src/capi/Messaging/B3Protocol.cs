namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum B3Protocol : short {
        Transparent = 0,
        /// <summary>
        /// T.90NL with compatibility to T.70NL in accordance with T.90 Appendix II.
        /// </summary>
        T90NL = 1,
        /// <summary>
        /// ISO 8208 (X.25 DTE-DTE)
        /// </summary>
        ISO8208 = 2,
        /// <summary>
        ///  X.25 DCE
        /// </summary>
        X25DCE = 3,
        /// <summary>
        ///  T.30 for Group 3 fax
        /// </summary>
        T30FAX = 4,
        /// <summary>
        ///  T.30 for Group 3 fax extended ,Includes support for fax-polling mode and
        /// detailed status information (see parameter NCPI).
        /// </summary>
        T30FAXExt = 5,
        /// <summary>
        /// Modem, Modem capability is also possible with B3 Protocol 0 (Transparent),
        /// but applications must use B3 Protocol 7 to obtain information
        /// about the results of modem negotiation
        /// </summary>
        Modem = 7
    }
}
