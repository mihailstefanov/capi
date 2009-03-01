using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    [Flags]
    public enum CIPServices {
        NoServices = 0x00000000,
        All = 0x1FFF03FF,
        Speech = 0x00000002,
        DataTransfer = 0x00000004,
        Audio31Khz = 0x00000010,
        Telephony = 0x00010000,
        FaxGroup23 = 0x00020000,
    }
}
