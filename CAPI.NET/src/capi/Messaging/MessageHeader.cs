namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;

    [Serializable]
    public class MessageHeader : ICapiSerializable {
        private Int16 _totalLength;
        private Int16 _appID;
        private Command _command;
        private SubCommand _scommand;
        private Int16 _number;

        public Int16 ID {
            get { return _number; }
            set { _number = value; }
        }

        public Int16 AppID {
            get { return _appID; }
            set { _appID = value; }
        }

        public Command Command {
            get { return _command; }
            set { _command = value; }
        }

        public SubCommand SubCommand {
            get { return _scommand; }
            set { _scommand = value; }
        }

        public Int16 TotalLength {
            get { return _totalLength; }
            set { _totalLength = value; }
        }

        byte ICapiSerializable.Length {
            get { return 8; }
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            _totalLength = reader.ReadInt16();
            _appID = reader.ReadInt16();
            _command = (Command)reader.ReadByte();
            _scommand = (SubCommand)reader.ReadByte();
            _number = reader.ReadInt16();
        }
        void ICapiSerializable.Write(BinaryWriter writer) {
            writer.Write(_totalLength);
            writer.Write(_appID);
            writer.Write((byte)_command);
            writer.Write((byte)_scommand);
            writer.Write(_number);
        }

    }
}
