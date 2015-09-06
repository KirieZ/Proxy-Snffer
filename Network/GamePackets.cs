using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer.Network
{
	public static class GamePackets
	{
		private static Dictionary<short, Packets.Packet> packet_db;

		static GamePackets()
		{
			packet_db = Packets.LoadAuthPackets();
		}

		internal static PacketStream PacketReceived(PacketStream stream)
		{
			// Header
			// [Size:4]
			// [ID:2]
			// [Checksum(?):1]
			short PacketId = stream.GetId();

			/*if (!packet_db.ContainsKey(PacketId))
			{
				ConsoleUtils.HexDump(stream.ToArray(), "Unknown Packet Received", PacketId, stream.GetSize());
				return;
			}

			ConsoleUtils.HexDump(
				stream.ToArray(),
				"Packet Received",
				PacketId,
				stream.GetSize()
			);*/

			if(packet_db.ContainsKey(PacketId))
				packet_db[PacketId].func(ref stream, packet_db[PacketId].pos);

			return stream;
		}
	}
}
