using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utf8NetworkLib
{
	/// <summary>
	/// Event args for all socket events
	/// </summary>
	public class Utf8TcpPeerEventArgs : EventArgs
	{
		internal Utf8TcpPeerEventArgs(Utf8TcpPeer peer, string message = null)
		{
			Peer = peer;
			Message = message;
		}

		/// <summary>
		/// Gets the peer associated with this event.
		/// </summary>
		public Utf8TcpPeer Peer { get; private set; }
		/// <summary>
		/// Gets the message associated with this event. Is null for most events (except DataReceived).
		/// </summary>
		public string Message { get; private set; }
	}
}
