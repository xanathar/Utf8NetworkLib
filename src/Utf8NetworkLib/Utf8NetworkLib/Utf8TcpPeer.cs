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
	/// This class represents a single connection
	/// </summary>
	public class Utf8TcpPeer
	{
		Socket m_Socket;
		IUtf8SocketOwner m_Owner;
		int m_PrevSize = 0;
		byte[] m_RecvBuffer;

		/// <summary>
		/// A unique Id associated with this object
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Occurs when the connection is closed
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> ConnectionClosed;
		/// <summary>
		/// Occurs when data is received
		/// </summary>
		public event EventHandler<Utf8TcpPeerEventArgs> DataReceived;


		internal Utf8TcpPeer(IUtf8SocketOwner owner, Socket socket)
		{
			m_Socket = socket;
			m_Owner = owner;
			m_RecvBuffer = new byte[m_Owner.BufferSize];
			Id = Guid.NewGuid().ToString();
		}

		internal void Start()
		{
			m_Socket.BeginReceive(m_RecvBuffer, 0, m_RecvBuffer.Length, SocketFlags.None, OnDataReceived, null);
		}

		private void OnDataReceived(IAsyncResult ar)
		{
			try
			{
				bool dataReceived = false;
				int size = m_Socket.EndReceive(ar);
				int ptr0 = m_PrevSize;
				m_PrevSize += size;

				do
				{
					dataReceived = false;

					char term = m_Owner.PacketSeparator;


					for (int i = ptr0; i < m_PrevSize; i++)
					{
						if (m_RecvBuffer[i] == term)
						{
							dataReceived = true;
							string message = Encoding.UTF8.GetString(m_RecvBuffer, 0, i);

							for (int j = i + 1; j < m_PrevSize; j++)
							{
								m_RecvBuffer[j - (i + 1)] = m_RecvBuffer[j];
							}

							ptr0 = 0;
							m_PrevSize = m_PrevSize - i - 1;

							if (DataReceived != null)
							{
								DataReceived(this, new Utf8TcpPeerEventArgs(this, message));
							}

							break;
						}
					}
				} while (dataReceived);

				m_Socket.BeginReceive(m_RecvBuffer, m_PrevSize, m_RecvBuffer.Length - m_PrevSize, SocketFlags.None, OnDataReceived, null);
			}
			catch (SocketException ex)
			{
				m_Owner.Logger(ex.Message);
				CloseConnection(ex.Message);
			}
			catch (ObjectDisposedException ex)
			{
				m_Owner.Logger(ex.Message);
				CloseConnection(ex.Message);
			}
		}


		private void CloseConnection(string reason)
		{
			if (DataReceived != null)
			{
				ConnectionClosed(this, new Utf8TcpPeerEventArgs(this, reason));
			}

			try
			{
				m_Socket.Close();
			}
			catch { }
		}

		/// <summary>
		/// Sends the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Send(string message)
		{
			SendTerminated(m_Owner.CompleteMessage(message));
		}

		/// <summary>
		/// Sends the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void Send(string message, params object[] args)
		{
			SendTerminated(m_Owner.CompleteMessage(string.Format(message, args)));
		}


		/// <summary>
		/// Sends the specified message, which is already terminated.
		/// </summary>
		/// <param name="message">The message.</param>
		public void SendTerminated(string message)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			SendBinary(bytes);
		}

		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		public void Disconnect()
		{
			m_Socket.Close();
		}

		/// <summary>
		/// Sends binary data
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		public void SendBinary(byte[] bytes)
		{
			try
			{
				m_Socket.Send(bytes);
			}
			catch (SocketException ex)
			{
				m_Owner.Logger(ex.Message);
				CloseConnection(ex.Message);
			}
			catch (ObjectDisposedException ex)
			{
				m_Owner.Logger(ex.Message);
				CloseConnection(ex.Message);
			}
		}
	}
}
