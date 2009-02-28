namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

    public class CapiSerializer {

        private CapiApplication _application;

        public CapiSerializer(CapiApplication application) {
            _application = application;
        }

        public void Serialize(Stream stream, Message message) {
            Debug.Assert(message.Identity.SubCommand == SubCommand.Response ||
                message.Identity.SubCommand == SubCommand.Request, "Message send to the CAPI from application can be request or response.");
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);

            MessageHeader header = new MessageHeader();
            header.Command = message.Identity.Command;
            header.SubCommand = message.Identity.SubCommand;
            header.AppID = (short)_application.AppID;
            header.ID = message.Number;

            ((ICapiSerializable)header).Write(writer);
            ((ICapiSerializable)message).Write(writer);

            header.TotalLength = (Int16)stream.Length;

            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(header.TotalLength);
        }

        public Message Deserialize(Stream stream, out MessageHeader header) {
            BinaryReader reader = new BinaryReader(stream);

            header = new MessageHeader();
            ((ICapiSerializable)header).Read(reader);

            Trace.TraceInformation("CapiSerializer#" + ValidationHelper.HashString(this)
             + "Deserialize header  = " + header.Command.ToString() + "," + header.SubCommand.ToString());


            Type t = Message.GetMessageType(header.Command, header.SubCommand);
            Debug.Assert(t != null);
            if (t != null) {
                Message message = (Message)Activator.CreateInstance(t);
                ((ICapiSerializable)message).Read(reader);
                message.Number = header.ID;
                return message;
            }
            return null;
        }
    }
}
