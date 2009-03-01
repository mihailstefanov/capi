namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Flags]
    public enum CIPMask : ushort {
        None = 0,
        AnyMatch = 1,
        Speech = 2,
        UnrestrictedDigital = 2 ^ 2,
        RestrictedDigital = 2 ^ 3,
        Audio31KHZ = 2 ^ 4,
        Audio7KHZ = 2 ^ 5,
        Video = 2 ^ 6,
        PacketMode = 2 ^ 7,
        KBIT56 = 2 ^ 8,
        Telephony = 2 ^ 16,
        Faxg3 = 2 ^ 17,
        Faxg4 = 2 ^ 18,
        TeletexBasicMixed = 2 ^ 19,
        TeletexBasicProcessable = 2 ^ 20,
        TeletexBasic = 2 ^ 21,
        Videotex = 2 ^ 22,
        Telex = 2 ^ 23,
        X400 = 2 ^ 24,
        OSIX200 = 2 ^ 25,
        Telephony7KHZ = 2 ^ 26,
        VideotelephonyFirst = 2 ^ 27,
        VideotelephonySecond = 2 ^ 28
    }
}
