using System;
using System.Collections.Generic;
using System.Text;
using Mommosoft.Capi.Properties;

namespace Mommosoft.Capi {
    public class Protocol {
        private int _id;
        string _description;

        public Protocol(int id, string description) {
            _id = id;
            _description = description;
        }

        public int Id {
            get { return _id; }
            set { _id = value; }
        }

        public string Description {
            get { return _description; }
            set { _description = value; }
        }

        public override string ToString() {
            return string.Format(ProtocolStrings.Format, _id, _description);
        }
    }
}
