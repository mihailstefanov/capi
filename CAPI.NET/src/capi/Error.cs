using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    internal static class Error {
        internal static Exception NotSupported() {
            return new NotSupportedException();
        }
    }
}
