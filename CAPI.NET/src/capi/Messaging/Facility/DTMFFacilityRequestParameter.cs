namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    public class DTMFFacilityRequestParameter : IParameter {
        private ParameterCollection _parameters;

        public DTMFFacilityRequestParameter() {
            _parameters = new ParameterCollection();
            // FacilityFunction
            _parameters.Add(new Parameter<short>());
            // Tone Duration
            _parameters.Add(new Parameter<short>(40));
            // Gap Duration
            _parameters.Add(new Parameter<short>(40));
            // DTMF digits
            _parameters.Add(new Parameter<string>());
            // DTMF Characteristics
            _parameters.Add(new Parameter<short>());
        }



        public FacilityFunction FacilityFunction {
            get { return (FacilityFunction)((Parameter<short>)_parameters[0]).Value; }
            set { ((Parameter<short>)_parameters[0]).Value = (short)value; }
        }

        /// <summary>
        /// Gets or sets time in ms for one digit; default is 40 ms
        /// </summary>
        public short ToneDuration {
            get { return ((Parameter<short>)_parameters[1]).Value; }
            set { ((Parameter<short>)_parameters[1]).Value = value; }
        }

        /// <summary>
        /// Gets or sets time in ms between digits; default is 40 ms
        /// </summary>
        public short GapDuration {
            get { return ((Parameter<short>)_parameters[2]).Value; }
            set { ((Parameter<short>)_parameters[2]).Value = value; }
        }

        /// <summary>
        /// Gets or sets characters to be sent, coded as IA5-char. '0' to '9', '*', '#', 'A','B', 'C' or 'D'. Each character generates a unique DTMF signal.
        /// </summary>
        public string Digits {
            get { return ((Parameter<string>)_parameters[3]).Value; }
            set { ((Parameter<string>)_parameters[3]).Value = value; }
        }

        /// <summary>
        /// Gets or sets characteristics of DTMF recognition (interpreted for StartListen only).
        /// </summary>
        public short Characteristics {
            get { return ((Parameter<short>)_parameters[4]).Value; }
            set { ((Parameter<short>)_parameters[4]).Value = value; }
        }

        public byte Length {
            get { return _parameters.Length; }
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            ((ICapiSerializable)_parameters).Read(reader);
        }

        void ICapiSerializable.Write(BinaryWriter writer) {
            writer.Write(Length);
            ((ICapiSerializable)_parameters).Write(writer);
        }
    }
}
