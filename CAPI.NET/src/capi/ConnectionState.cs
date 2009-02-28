namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum ConnectionStatus
    {
        Disconnected,
        D_ConnectPending,
        D_Connected,
        B_ConnectPending,
        Connected,
        B_DisconnectPending,
        D_DisconnectPending
    }
}
