namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum Reject : short {
        /// <summary>
        /// Accept call.
        /// </summary>
        Accept = 0,
        Ignore = 1,
        /// <summary>
        /// normal call clearing.
        /// </summary>
        NormalCallClearing = 2,
        /// <summary>
        /// user busy.
        /// </summary>
        UserBusy = 3,
        /// <summary>
        /// requested circuit/channel not available.
        /// </summary>
        NotAvailable = 4,
        /// <summary>
        /// facility rejected.
        /// </summary>
        FacilityRejected = 5,
        /// <summary>
        /// channel unacceptable.
        /// </summary>
        Unacceptable = 6,
        /// <summary>
        /// incompatible destination.
        /// </summary>
        IncompatibleDestination = 7,
        /// <summary>
        /// destination out of order.
        /// </summary>
        OutOfOrder = 8
    }
}
