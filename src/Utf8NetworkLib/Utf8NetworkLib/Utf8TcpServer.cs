using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace Utf8NetworkLib
{
	/// <summary>
	/// Handles a server 
	/// </summary>
	public class Utf8TcpServer : IUtf8SocketOwner, IDisposable
	{
		int m_PortNumber = 1912;
		IPAddress m_IPAddress;
		TcpListener m_Listener = null;
		Action<string> m_Logger;
		List<Utf8TcpPeer> m_PeerList = new List<Utf8TcpPeer>();
		object m_PeerListLock = new object();



		/// <summary>
		/// Gets the packet separator character. Must be a single byte character in UTF-8
		/// </summary>
		public char PacketSeparator { get; private set; }

		/// <summary>
		/// Gets the options with which this server was initialized
		/// </summary>
		public Utf8TcpServerOptions Options { get; private set; }

		/// <summary>
		/// Occurs when a client connects
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> ClientConnected;
		/// <summary>
		/// Occurs when data is received
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> DataReceived;
		/// <summary>
		/// Occurs when a client disconnects
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> ClientDisconnected;

		/// <summary>
		/// Gets the port number the server is listening to
		/// </summary>
		public int PortNumber
		{
			get { return m_PortNumber; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Utf8TcpServer"/> class.
		/// </summary>
		/// <param name="port">The port to listen to.</param>
		/// <param name="bufferSize">Size of the buffer. Messages longer than this cause problems.</param>
		/// <param name="packetSeparator">The packet separator. Must be a single byte character in UTF-8</param>
		/// <param name="options">The options.</param>
		public Utf8TcpServer(int port, int bufferSize, char packetSeparator, Utf8TcpServerOptions options)
        {
			m_IPAddress = ((options & Utf8TcpServerOptions.LocalHostOnly) != 0) ? IPAddress.Loopback : IPAddress.Any;
            m_PortNumber = port;
			m_Logger = s => System.Diagnostics.Debug.WriteLine(s);
			PacketSeparator = packetSeparator;
			BufferSize = bufferSize;
			Options = options;
        }

		/// <summary>
		/// A delegate called to log some messages
		/// </summary>
		public Action<string> Logger
		{
			get { return m_Logger; }
			set { m_Logger = value ?? (s => Console.WriteLine(s)); }
		}

		/// <summary>
		/// Starts listening for client connections
		/// </summary>
        public void Start()
        {
			m_Listener = new TcpListener(m_IPAddress, m_PortNumber);
			m_Listener.Start();
			m_Listener.BeginAcceptSocket(OnAcceptSocket, null);
        }

		/// <summary>
		/// Gets the size of the buffer. Messages longer than this cause problems.
		/// </summary>
		public int BufferSize { get; private set; }


		private void OnAcceptSocket(IAsyncResult ar)
		{
			try
			{
				Socket s = m_Listener.EndAcceptSocket(ar);
				AddNewClient(s);
				m_Listener.BeginAcceptSocket(OnAcceptSocket, null);
			}
			catch (SocketException ex)
			{
				Logger("OnAcceptSocket : " + ex.Message);
			}
			catch (ObjectDisposedException ex)
			{
				Logger("OnAcceptSocket : " + ex.Message);
			}
		}

		/// <summary>
		/// Gets the number of connected clients.
		/// </summary>
		public int GetConnectedClients()
		{
			lock (m_PeerListLock) 
				return m_PeerList.Count;
		}


		private void AddNewClient(Socket socket)
		{
			if ((Options & Utf8TcpServerOptions.SingleClientOnly) != 0)
			{
				lock (m_PeerListLock)
				{
					foreach (var pp in m_PeerList)
					{
						pp.Disconnect();
					}
				}
			}

			Utf8TcpPeer peer = new Utf8TcpPeer(this, socket);

			lock (m_PeerListLock)
			{
				m_PeerList.Add(peer);
				peer.ConnectionClosed += OnPeerDisconnected;
				peer.DataReceived += OnPeerDataReceived;
			}


			if (ClientConnected != null)
			{
				Utf8TcpPeerEventArgs args = new Utf8TcpPeerEventArgs(peer);
				ClientConnected(this, args);
			} 
			
			peer.Start();
		}

		private void OnPeerDataReceived(object sender, Utf8TcpPeerEventArgs e)
		{
			if (DataReceived != null)
			{
				DataReceived(this, e);
			}
		}

		void OnPeerDisconnected(object sender, Utf8TcpPeerEventArgs e)
        {
            try
            {
				if (ClientDisconnected != null)
				{
					ClientDisconnected(this, e);
				}

                lock (m_PeerListLock)
                {
                    m_PeerList.Remove(e.Peer);
					e.Peer.ConnectionClosed -= OnPeerDisconnected;
					e.Peer.DataReceived -= OnPeerDataReceived;
				}
            }
            catch
            {
            }
        }

		/// <summary>
		/// Broadcasts a  message to all connected clients
		/// </summary>
		/// <param name="message">The message.</param>
        public void BroadcastMessage(string message)
        {
			List<Utf8TcpPeer> peers;

            lock (m_PeerListLock)
            {
				peers = m_PeerList.ToList();
			}

			message = this.CompleteMessage(message);

			if (message == null)
				return;

			foreach (Utf8TcpPeer peer in peers)
            {
                try
                {
					peer.SendTerminated(message);
                }
                catch { }
            }
        }

		/// <summary>
		/// Stops this instance.
		/// </summary>
        public void Stop()
        {
			m_Listener.Stop();
        }


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Stop();
		}
	}
}
