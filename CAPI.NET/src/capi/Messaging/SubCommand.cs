namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum SubCommand : byte {
        Undefined = 0x00,
        Request = 0x80,
        Confirmation = 0x81,
        Indication = 0x82,
        Response = 0x83
    }
}
