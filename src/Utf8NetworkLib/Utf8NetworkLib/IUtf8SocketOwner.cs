using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utf8NetworkLib
{
	/// <summary>
	/// Common interface for Utf8TcpPeer owners
	/// </summary>
	interface IUtf8SocketOwner
	{
		/// <summary>
		/// Gets the size of the buffer. Messages longer than this cause problems.
		/// </summary>
		int BufferSize { get; }
		/// <summary>
		/// Gets the packet separator character. Must be a single byte character in UTF-8
		/// </summary>
		char PacketSeparator { get; }
		/// <summary>
		/// A delegate called to log some messages
		/// </summary>
		Action<string> Logger { get; }
	}

	internal static class IUtf8SocketOwner_Extensions
	{
		/// <summary>
		/// Completes a message with proper termination
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		internal static string CompleteMessage(this IUtf8SocketOwner owner, string message)
		{
			if (string.IsNullOrEmpty(message))
				return owner.PacketSeparator.ToString();

			if (message[message.Length - 1] != owner.PacketSeparator)
				message = message + owner.PacketSeparator;

			return message;
		}
	}

}
