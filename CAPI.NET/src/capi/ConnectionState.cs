namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum ConnectionStatus {
        Idle = 0,
        /// <summary>
        ///  Waiting for activation signal (channel up).
        /// </summary>
        WaitActivation,
        /// <summary>
        /// Wait for activate of "B" channel protocol.
        /// </summary>
        WaitB3ProtocolActivation,
        /// <summary>
        ///  Wait for B3 activation signal (can now receive data).
        /// </summary>
        ActivationB3Indication,
        /// <summary>
        /// "B" channel protocol up, channel fully activated.
        /// </summary>
        Connected,
        /// <summary>
        /// In the process of tearing down "B" protocol.
        /// </summary>
        Disconnecting,
        /// <summary>
        /// Tearing down "B" channel
        /// </summary>
        Disconnected
    }
}
