using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utf8NetworkLib;

namespace TestApplication
{
	public partial class MainForm : Form
	{
		Utf8TcpServer m_Server;
		Utf8TcpClient m_Client;

		public MainForm()
		{
			InitializeComponent();
		}

		private void btnFork_Click(object sender, EventArgs e)
		{
			Process.Start(Application.ExecutablePath);
		}

		private void txtSend_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
				btnSend.PerformClick();
		}

		private void DisableConnectionControls()
		{
			txtAddress.Enabled = txtPort.Enabled = btnConnect.Enabled = btnListen.Enabled = false;
			txtSend.Enabled = btnSend.Enabled = true;
		}

		private void Log(string format, params object[] args)
		{
			string msg = string.Format(format, args);
			// note : here we are likely in some random thread.. use BeginInvoke to marshal the call to the UI thread
			this.BeginInvoke((Action)(() => { lstReceived.Items.Insert(0, msg); }));
		}

		private void LogStatus(string format, params object[] args)
		{
			string msg = string.Format(format, args);
			// note : here we are likely in some random thread.. use BeginInvoke to marshal the call to the UI thread
			this.BeginInvoke((Action)(() => { lstReceived.Items.Insert(0, msg); lblStatus.Text = msg; }));
		}

		#region Server code

		private void btnListen_Click(object sender, EventArgs e)
		{
			DisableConnectionControls();
			lblStatus.Text = "INITIALIZING SERVER..";
			// create the server object
			m_Server = new Utf8TcpServer(int.Parse(txtPort.Text), 1000000, '\n', Utf8TcpServerOptions.Default);
			// attach event handlers.. most are just for logging purposes
			m_Server.ClientConnected += m_Server_ClientConnected;
			m_Server.ClientDisconnected += m_Server_ClientDisconnected;
			m_Server.DataReceived += m_Server_DataReceived;
			// start the server
			m_Server.Start();

			LogStatus("{0} connected clients", m_Server.GetConnectedClients());
			btnSend.Click += Server_btnSend_Click;
		}

		void Server_btnSend_Click(object sender, EventArgs e)
		{
			m_Server.BroadcastMessage(txtSend.Text);
			txtSend.Text = "";
		}

		void m_Server_DataReceived(object sender, Utf8TcpPeerEventArgs e)
		{
			Log("Client {0} sent: '{1}'", e.Peer.Id, e.Message);
		}

		void m_Server_ClientDisconnected(object sender, Utf8TcpPeerEventArgs e)
		{
			Log("Client Disconnected {0}", e.Peer.Id);
			LogStatus("{0} connected clients", m_Server.GetConnectedClients());
		}

		void m_Server_ClientConnected(object sender, Utf8TcpPeerEventArgs e)
		{
			Log("Client Connected {0}", e.Peer.Id);
			LogStatus("{0} connected clients", m_Server.GetConnectedClients());
		}

		#endregion

		#region Client Code

		private void btnConnect_Click(object sender, EventArgs e)
		{
			DisableConnectionControls();
			lblStatus.Text = "INITIALIZING CONNECTION..";
			// create the client object with a 1 sec auto-reconnect
			m_Client = new Utf8TcpClient(txtAddress.Text, int.Parse(txtPort.Text), 1000000, '\n', 1000);
			// attach event handlers.. most are just for logging purposes as we have auto-reconnect turned on
			m_Client.ClientConnected += m_Client_ClientConnected;
			m_Client.ClientDisconnected += m_Client_ClientDisconnected;
			m_Client.ConnectionFailed += m_Client_ConnectionFailed;
			m_Client.DataReceived += m_Client_DataReceived;

			lblStatus.Text = "CONNECTING..";

			// and now, connect
			m_Client.Connect();

			btnSend.Click += Client_btnSend_Click;
		}

		void Client_btnSend_Click(object sender, EventArgs e)
		{
			m_Client.Send(txtSend.Text);
			txtSend.Text = "";
		}


		void m_Client_ConnectionFailed(object sender, EventArgs e)
		{
			LogStatus("Connection failed");
		}

		void m_Client_DataReceived(object sender, Utf8TcpPeerEventArgs e)
		{
			Log("Server sent: '{0}'", e.Message);
		}

		void m_Client_ClientDisconnected(object sender, Utf8TcpPeerEventArgs e)
		{
			LogStatus("Client Disconnected");
		}

		void m_Client_ClientConnected(object sender, Utf8TcpPeerEventArgs e)
		{
			LogStatus("Client Connected");
		}

		#endregion


	




	}
}
