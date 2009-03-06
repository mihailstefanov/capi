namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DTFMEventArgs : ConnectionEventArgs {
        private readonly string _digits;

        public DTFMEventArgs(Connection connection, string digits)
            : base(connection) {
            _digits = digits;
        }

        public string Digits {
            get { return _digits; }
        }

    }
}
