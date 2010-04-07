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
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

namespace Mommosoft.Capi.Phone {
    public partial class MainForm : Form {
        private CapiApplication _app = new CapiApplication();
        private string _answerSoundFileName;
        private string _textToSpeak;
        private Connection _currentIncommingConnection;

        public MainForm() {
            InitializeComponent();
            buttonAnswer.Enabled = false;
            buttonHangup.Enabled = false;
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
            if (e.Connection.Status == ConnectionStatus.Connected) {
                _currentIncommingConnection = e.Connection;
                if (!string.IsNullOrEmpty(_answerSoundFileName)) {
                    ThreadPool.QueueUserWorkItem(SendFile, e.Connection);
                }
            } else {
                _currentIncommingConnection = null;
            }
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
                this.Invoke(new EventHandler<IncomingPhysicalConnectionEventArgs>(OnIncomingPhysicalConnection), sender, e);
            } else {
                buttonAnswer.Tag = e.Connection;
                buttonAnswer.Enabled = true;
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

        private void SendTextToSpeech(object obj) {
            Connection c = (Connection)obj;
            ConnectionStream stream = new ConnectionStream(c);
            SpeechSynthesizer s = new SpeechSynthesizer();
            using (MemoryStream memStream = new MemoryStream()) {
                s.SetOutputToAudioStream(memStream, new SpeechAudioFormatInfo(EncodingFormat.ALaw,
                    8000, 8, 1, 8000, 1, null));
                s.Speak(_textToSpeak);
                memStream.Seek(0, SeekOrigin.Begin);

                using (ConnectionWriter writer = new ConnectionWriter(stream)) {
                    writer.Reverse = true;
                    writer.Write(memStream);
                }
            }
        }

        private void SendFile(object obj) {
            Connection c = (Connection)obj;
            ConnectionStream stream = new ConnectionStream(c);
            using (FileStream fs = File.OpenRead(_answerSoundFileName)) {
                using (ConnectionWriter writer = new ConnectionWriter(stream)) {
                    writer.Write(fs);
                }
            }
        }

        private void buttonSpeak_Click(object sender, EventArgs e) {
            Connection c = buttonDisconnect.Tag as Connection;
            c = (c == null) ? _currentIncommingConnection : c;
            if (c != null && c.Status == ConnectionStatus.Connected && !string.IsNullOrEmpty(textBoxTextToSpeak.Text)) {
                _textToSpeak = textBoxTextToSpeak.Text;
                ThreadPool.QueueUserWorkItem(SendTextToSpeech, c);
            }
        }

        private void buttonAnswer_Click(object sender, EventArgs e) {
            Connection c = buttonAnswer.Tag as Connection;
            if (c != null) {
                buttonAnswer.Tag = null;
                buttonAnswer.Enabled = false;
                buttonHangup.Tag = c;
                buttonHangup.Enabled = true;
                c.Answer(Reject.Accept, B1Protocol.HDLC64BFN, B2Protocol.Transparent, B3Protocol.Transparent);
            }
        }

        private void buttonHangup_Click(object sender, EventArgs e) {
              Connection c = buttonHangup.Tag as Connection;
              if (c != null) {
                  buttonHangup.Tag = null;
                  buttonHangup.Enabled = false;
                  c.HangUp();
              }
        }
    }
}
