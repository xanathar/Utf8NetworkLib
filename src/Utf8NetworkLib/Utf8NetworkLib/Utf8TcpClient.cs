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
	/// <summary>
	/// Handles a client which connects to a server
	/// </summary>
	public class Utf8TcpClient : IUtf8SocketOwner, IDisposable
	{
		int m_PortNumber = 1111;
		string m_Address;

		Action<string> m_Logger;
		TcpClient m_Client;
		Utf8TcpPeer m_Peer = null;
		object m_ReconnectLock = new object();
		int m_AutoReconnect;
		bool m_Disposed = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Utf8TcpClient"/> class.
		/// </summary>
		/// <param name="address">The address to which to connect</param>
		/// <param name="port">The port to which to connect.</param>
		/// <param name="bufferSize">Size of the buffer. Messages longer than this cause problems.</param>
		/// <param name="packetSeparator">The packet separator. Must be a single byte character in UTF-8</param>
		/// <param name="autoReconnect">A number of milliseconds after which failed connections are automatically retried. -1 means no auto-reconnection.</param>
		public Utf8TcpClient(string address, int port, int bufferSize, char packetSeparator, int autoReconnect = -1)
		{
			m_Address = address;
			m_PortNumber = port;
			m_Logger = s => System.Diagnostics.Debug.WriteLine(s);
			PacketSeparator = packetSeparator;
			BufferSize = bufferSize;
			m_AutoReconnect = autoReconnect;
		}

		/// <summary>
		/// Gets the packet separator character. Must be a single byte character in UTF-8
		/// </summary>
		public char PacketSeparator { get; private set; }

		/// <summary>
		/// Gets the size of the buffer. Messages longer than this cause problems.
		/// </summary>
		public int BufferSize { get; private set; }

		/// <summary>
		/// A delegate called to log some messages
		/// </summary>
		public Action<string> Logger
		{
			get { return m_Logger; }
			set { m_Logger = value ?? (s => Console.WriteLine(s)); }
		}

		/// <summary>
		/// Gets the connected peer wrapped by this client
		/// </summary>
		public Utf8TcpPeer Peer
		{
			get { return m_Peer; }
		}

		/// <summary>
		/// Attempts a connection
		/// </summary>
		public void Connect()
		{
			lock (m_ReconnectLock)
			{
				m_Client = new TcpClient();

				if (!m_Disposed)
					m_Client.BeginConnect(m_Address, m_PortNumber, EndConnect, null);
			}
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
			catch (SocketException)
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

		/// <summary>
		/// Sends the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Send(string message)
		{
			var peer = m_Peer;

			if (peer != null)
				peer.Send(message);
		}

		/// <summary>
		/// Sends the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void Send(string message, params object[] args)
		{
			var peer = m_Peer;

			if (peer != null)
				peer.Send(message, args);
		}


		/// <summary>
		/// Occurs when a connection attempt fails (happens on whatever thread, be careful with synchronizations)
		/// </summary>
		public event EventHandler ConnectionFailed;
		/// <summary>
		/// Occurs when the client connects (happens on whatever thread, be careful with synchronizations)
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> ClientConnected;
		/// <summary>
		/// Occurs when the client gets disconnected (happens on whatever thread, be careful with synchronizations)
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> ClientDisconnected;
		/// <summary>
		/// Occurs when data is received (happens on whatever thread, be careful with synchronizations)
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> DataReceived;


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock (m_ReconnectLock)
			{
				if (m_Client != null)
				{
					m_Client.Close();
					m_Client = null;
				}
			}
		}
	}
}
