namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum B2Protocol {
        /// <summary>
        /// ISO 7776 (X.75 SLP) (default)
        /// </summary>
        ISO7776 = 0,
        /// <summary>
        /// Transparent.
        /// </summary>
        Transparent = 1,
        /// <summary>
        /// SDLC
        /// </summary>
        SDLC = 2,
        /// <summary>
        /// LAPD in accordance with Q.921 for D channel X.25 (SAPI 16)
        /// </summary>
        LAPDSAPI16 = 3
    }
}
