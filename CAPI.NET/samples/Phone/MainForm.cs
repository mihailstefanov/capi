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
            foreach (Controller c in _app.Controllers) {
                comboBoxControllers.Items.Add(c);
            }
            if (comboBoxControllers.Items.Count > 0)
                comboBoxControllers.SelectedIndex = 0;

            _app.IncomingPhysicalConnection += new EventHandler<IncomingPhysicalConnectionEventArgs>(OnIncomingPhysicalConnection);
        }

        void OnIncomingPhysicalConnection(object sender, IncomingPhysicalConnectionEventArgs e) {
            if (listBoxInfo.InvokeRequired) {
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
            Controller c = comboBoxControllers.SelectedItem as Controller;
            if (c != null) {
                c.Listen(CIPMask.Telephony | CIPMask.Telephony7KHZ | CIPMask.Audio7KHZ | CIPMask.Audio31KHZ);
                comboBoxControllers.Enabled = false;
            }
        }
    }
}
