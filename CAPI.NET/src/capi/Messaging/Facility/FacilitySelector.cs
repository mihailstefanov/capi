namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum FacilitySelector : short {
        /// <summary>
        /// Handset.
        /// </summary>
        Handset = 0x0000,
        /// <summary>
        /// DTMF.
        /// </summary>
        DTMF = 0x0001,
        /// <summary>
        /// V.42 bis.
        /// </summary>
        V42 = 0x0002,
        /// <summary>
        ///  Supplementary Services (see Part III) of CAPI ref.
        /// </summary>
        SupplementaryServices = 0x0003,
        /// <summary>
        /// Power management wakeup.
        /// </summary>
        PowerManagementWakeup = 0x0004,
        /// <summary>
        /// Line Interconnect.
        /// </summary>
        LineInterconnect = 0x0005
    }
}
