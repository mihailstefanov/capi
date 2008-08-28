namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum Command : byte {
        Undefined = 0x00,
        Alert = 0x01,
        Connect = 0x02,
        ConnectActive = 0x03,
        ConnectB3Active = 0x83,
        ConnectB3 = 0x82,
        ConnectB3T90Active = 0x88,
        DataB3 = 0x86,
        DisconnectB3 = 0x84,
        Disconnect = 0x04,
        Facility = 0x80,
        Info = 0x08,
        Listen = 0x05,
        Manufacturer = 0xFF,
        ResetB3 = 0x87,
        SelectBProtocol = 0x41
    }
}
