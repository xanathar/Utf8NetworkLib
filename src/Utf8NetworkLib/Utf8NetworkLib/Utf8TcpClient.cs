using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace Utf8NetworkLib
{
	public class Utf8TcpClient : IUtf8SocketOwner
	{
		int m_PortNumber = 1111;
		string m_Address;
		
		Action<string> m_Logger;
		TcpClient m_Client;
		Utf8TcpPeer m_Peer = null;
		object m_PeerListLock = new object();
		int m_AutoReconnect;

		public char PacketSeparator { get; private set; }
		public int BufferSize { get; private set; }

		public Utf8TcpClient(string address, int port, int bufferSize, char packetSeparator, int autoReconnect = -1)
        {
			m_Address = address;
            m_PortNumber = port;
			m_Logger = s => System.Diagnostics.Debug.WriteLine(s);
			PacketSeparator = packetSeparator;
			BufferSize = bufferSize;
			m_AutoReconnect = autoReconnect;
        }

		public Action<string> Logger
		{
			get { return m_Logger; }
			set { m_Logger = value ?? (s => Console.WriteLine(s)); }
		}

		public Utf8TcpPeer Peer
		{
			get { return m_Peer; }
		}

		public void Connect()
		{
			m_Client = new TcpClient();

			if (!m_Client.Connected)
				m_Client.BeginConnect(m_Address, m_PortNumber, EndConnect, null);
		}

		private void EndConnect(IAsyncResult res)
		{
			try
			{
				m_Client.EndConnect(res);
				m_Peer = new Utf8TcpPeer(this, m_Client.Client);

				if (ClientConnected != null)
					ClientConnected(this, new Utf8TcpPeerEventArgs(m_Peer));

				m_Peer.ConnectionClosed += m_Peer_ConnectionClosed;
				m_Peer.DataReceived += m_Peer_DataReceived;
			}
			catch(SocketException)
			{
				if (ConnectionFailed != null)
					ConnectionFailed(this, EventArgs.Empty);

				RetryConnectionIfNeeded();
			}
		}

		private void RetryConnectionIfNeeded()
		{
			m_Client = null;

			if (m_AutoReconnect >= 0)
			{
				ThreadPool.QueueUserWorkItem(_ =>
				{
					Thread.Sleep(m_AutoReconnect);
					Connect();
				});
			}
		}

		void m_Peer_DataReceived(object sender, Utf8TcpPeerEventArgs e)
		{
			if (DataReceived != null)
				DataReceived(this, e);
		}

		void m_Peer_ConnectionClosed(object sender, Utf8TcpPeerEventArgs e)
		{
			m_Peer = null;

			if (ClientDisconnected != null)
				ClientDisconnected(this, e);

			RetryConnectionIfNeeded();
		}

		public void Send(string message)
		{
			var peer = m_Peer;

			if (peer != null)
				peer.Send(message);
		}

		public void Send(string message, params object[] args)
		{
			var peer = m_Peer;

			if (peer != null)
				peer.Send(message, args);
		}


		public event EventHandler ConnectionFailed;
		public event EventHandler<Utf8TcpPeerEventArgs> ClientConnected;
		public event EventHandler<Utf8TcpPeerEventArgs> ClientDisconnected;
		public event EventHandler<Utf8TcpPeerEventArgs> DataReceived;




	}
}
