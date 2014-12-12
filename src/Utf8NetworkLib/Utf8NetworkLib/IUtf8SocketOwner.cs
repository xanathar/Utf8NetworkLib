using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utf8NetworkLib
{
	interface IUtf8SocketOwner
	{
		int BufferSize { get; }
		char PacketSeparator { get; }
		Action<string> Logger { get; }
	}

	internal static class IUtf8SocketOwner_Extensions
	{
		public static string CompleteMessage(this IUtf8SocketOwner owner, string message)
		{
			if (string.IsNullOrEmpty(message))
				return owner.PacketSeparator.ToString();

			if (message[message.Length - 1] != owner.PacketSeparator)
				message = message + owner.PacketSeparator;

			return message;
		}
	}

}
