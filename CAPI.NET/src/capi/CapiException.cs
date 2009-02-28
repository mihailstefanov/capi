namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.Serialization;
    using Mommosoft.Capi.Properties;

    [Serializable]
    public class CapiException : Exception {
        private Info _code;

        public CapiException() { }
        public CapiException(string message) : base(message) { }
        public CapiException(Info code, string message)
            : base(message) {
            _code = code;
        }

        public CapiException(Info code) {
            _code = code;
        }

        public override string Message {
            get {
                if (string.IsNullOrEmpty(base.Message)) {
                    string codeKey = string.Format("INFO_{0:X4}", (short)_code);
                    return InfoStrings.ResourceManager.GetString(codeKey);
                }
                return base.Message;
            }
        }

        protected CapiException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) {
        }

    }
}
