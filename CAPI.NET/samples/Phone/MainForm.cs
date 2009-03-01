using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mommosoft.Capi.Phone {
    public partial class MainForm : Form {
        CapiApplication _app = new CapiApplication();

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
                e.B1Protocol = B1Protocol.HDLC64;
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
                    textBoxCalledNumber.Text, CIPServices.Telephony, B1Protocol.HDLC64, B2Protocol.Transparent, B3Protocol.Transparent);
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
    }
}
