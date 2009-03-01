namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum Info : ushort {
        Success = 0,
        /// <summary>
        /// Illegal controller.
        /// </summary>
        IllegalController = 0x2002,
        /// <summary>
        /// No Listen resources available.
        /// </summary>
        NoListenResourcesAvailable = 0x2005,
        /// <summary>
        /// Illegal message parameter coding.
        /// </summary>
        IllegalMessageParameterCoding = 0x2007,
        /// <summary>
        /// Abort D-channel, level 2
        /// </summary>
        AbortDChannelLevel2 = 0x3306

    }
}
