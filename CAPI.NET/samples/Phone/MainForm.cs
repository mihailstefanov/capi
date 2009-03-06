using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Mommosoft.Capi.Phone {
    public partial class MainForm : Form {
        CapiApplication _app = new CapiApplication();
        private string _answerSoundFileName;

        public MainForm() {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            toolStripStatusLabel1.Text = _app.Manufacturer;
            toolStripStatusLabel2.Text = string.Format("CAPI Version: {0}", _app.CapiVersion.ToString());

            foreach (Controller c in _app.Controllers) {
                comboBoxControllers.Items.Add(c);
            }
            if (comboBoxControllers.Items.Count > 0)
                comboBoxControllers.SelectedIndex = 0;

            _app.IncomingPhysicalConnection += new EventHandler<IncomingPhysicalConnectionEventArgs>(OnIncomingPhysicalConnection);
            _app.DTFMIndication += new EventHandler<DTFMEventArgs>(OnDTFMIndication);
            _app.ConnectionStatusChanged += new EventHandler<ConnectionEventArgs>(OnConnectionStatusChangedInternal);
        }

        // we need this method instead of directly call OnConnectionStatusChanged
        // because of deadlock which can happen if we initiate the call ( pressing Call button).
        void OnConnectionStatusChangedInternal(object sender, ConnectionEventArgs e) {
            EventHandler<ConnectionEventArgs> d = new EventHandler<ConnectionEventArgs>(OnConnectionStatusChanged);
            if(e.Connection.Status == ConnectionStatus.Connected && !string.IsNullOrEmpty(_answerSoundFileName))
                ThreadPool.QueueUserWorkItem(SendFile, e.Connection);
            d.BeginInvoke(sender, e, null, null);

        }

        void OnConnectionStatusChanged(object sender, ConnectionEventArgs e) {
            if (listBoxInfo.InvokeRequired) {
                this.Invoke(new EventHandler<ConnectionEventArgs>(OnConnectionStatusChanged), sender, e);
            } else {
                listBoxInfo.Items.Add(string.Format("{0} {1}", e.Connection.Status, e.Connection.CalledPartyNumber));
            }
        }

        void OnDTFMIndication(object sender, DTFMEventArgs e) {
            if (listBoxDTFMInfo.InvokeRequired) {
                this.Invoke(new EventHandler<DTFMEventArgs>(OnDTFMIndication), sender, e);
            } else {
                listBoxDTFMInfo.Items.Add(string.Format("{0}", e.Digits));
            }
        }

        void OnIncomingPhysicalConnection(object sender, IncomingPhysicalConnectionEventArgs e) {
            if (listBoxInfo.InvokeRequired) {
                // auto accpet the call... may be will be nice to have options like answer after 3th ring or ...
                e.Reject = Reject.Accept;
                e.B1Protocol = B1Protocol.HDLC64BFN;
                e.B2Protocol = B2Protocol.Transparent;
                e.B3Protocol = B3Protocol.Transparent;
                this.Invoke(new EventHandler<IncomingPhysicalConnectionEventArgs>(OnIncomingPhysicalConnection), sender, e);
            } else {
                listBoxInfo.Items.Add(string.Format("Incoming call {0}", e.Connection.CalledPartyNumber));
            }
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            _app.Dispose();
        }

        private void buttonListen_Click(object sender, EventArgs e) {
            Controller c = GetSelectedController();
            if (c != null) {
                c.Listen(CIPMask.Telephony | CIPMask.Telephony7KHZ | CIPMask.Audio7KHZ | CIPMask.Audio31KHZ);
                comboBoxControllers.Enabled = false;
            }
        }

        private void buttonCall_Click(object sender, EventArgs e) {
            Controller c = GetSelectedController();
            if (c != null) {

                buttonDisconnect.Tag = c.Connect(string.Empty,
                    textBoxCalledNumber.Text, CIPServices.Telephony, B1Protocol.HDLC64BFN, B2Protocol.Transparent, B3Protocol.Transparent);
                buttonDisconnect.Enabled = true;
            }
        }

        private Controller GetSelectedController() {
            Controller c = comboBoxControllers.SelectedItem as Controller;
            return c;
        }

        private void buttonDisconnect_Click(object sender, EventArgs e) {
            buttonDisconnect.Enabled = false;
            Connection c = buttonDisconnect.Tag as Connection;
            buttonDisconnect.Tag = null;
            if (c != null) {
                c.HangUp();
            }
        }

        private void buttonDTFMStart_Click(object sender, EventArgs e) {
            Controller c = GetSelectedController();
            foreach (Connection connection in c.Connections) {
                connection.DTFMListen = true;
            }
        }

        private void buttonDTFMSend_Click(object sender, EventArgs e) {
            Controller c = GetSelectedController();
            foreach (Connection connection in c.Connections) {
                connection.SendDTFMTone(textBoxDTFMChars.Text);
            }
        }

        private void buttonBrowseWave_Click(object sender, EventArgs e) {
            if (openFileDialogWave.ShowDialog(this) == DialogResult.OK)
                textBoxWaveFile.Text = openFileDialogWave.SafeFileName;
            _answerSoundFileName = openFileDialogWave.FileName;
        }


        private void SendFile(object obj) {
            Connection c = (Connection)obj;
            ConnectionStream stream = new ConnectionStream(c);
            using (FileStream fs = File.OpenRead(_answerSoundFileName)) {
                CopyBytes(fs, stream, 2048, false);
            }
        }

        private class WriteStatus {
            public WriteStatus(Stream stream, List<IAsyncResult> results) {
                Stream = stream;
                Results = results;
            }

            public Stream Stream;
            public List<IAsyncResult> Results; 
        }

        // idea is to send few request by time, this need some more work
        public static void CopyBytes(Stream input, Stream output, int buffSize, bool close) {
            byte[] buf = new byte[buffSize];
            List<IAsyncResult> results = new List<IAsyncResult>();
            try {

                int bytesRead = input.Read(buf, 0, buf.Length);
                while (bytesRead > 0) {
                    if (results.Count < 2) {
                        IAsyncResult result = output.BeginWrite(buf, 0, bytesRead, OnBytesWritten, new WriteStatus(output, results));
                        lock (results) {
                            results.Add(result);
                        }
                        //output.Write(buf, 0, bytesRead);
                        bytesRead = input.Read(buf, 0, buf.Length);
                    }
                }
            } finally {
                if (close) {
                    output.Close();
                    input.Close();
                }

            }
        }

        private static void OnBytesWritten(IAsyncResult result)
        {
            WriteStatus status = result.AsyncState as WriteStatus;
            status.Stream.EndWrite(result);
            lock (status.Results) {
                status.Results.Remove(result);
            }
        }
    }
}
