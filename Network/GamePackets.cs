using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer.Network
{
	public static class GamePackets
	{
		private static Dictionary<short, Packets.Packet> packet_db;
		private static Dictionary<short, string> packet_names;

		static GamePackets()
		{
			packet_db = Packets.LoadGamePackets();
			LoadPacketNames();
		}

		static void LoadPacketNames()
		{
			packet_names = new Dictionary<short, string>();
			if (File.Exists("packets/game_packets.txt"))
			{
				string[] lines = File.ReadAllLines("packets/game_packets.txt");

				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].StartsWith("//")) continue;
					else if (!lines[i].Contains('=')) continue;

					string[] val = lines[i].Split('=');
					short id;
					if (!short.TryParse(val[0].Trim(' '), out id))
					{
						// If can't convert from int, try Hex (0x)
						try
						{
							id = (short)new System.ComponentModel.Int16Converter().ConvertFromString(val[0].Trim(' '));
						}
						catch (Exception)
						{
							continue;
						}
					}

					if (!packet_names.ContainsKey(id))
					{
						packet_names.Add(id, val[1].TrimStart(' '));
					}
				}
			}
		}

		internal static PacketStream PacketReceived(char src, PacketStream stream)
		{
			// Header
			// [Size:4]
			// [ID:2]
			// [Checksum(?):1]
			short PacketId = stream.GetId();
			string name = (packet_names.ContainsKey(PacketId) ? packet_names[PacketId] : "Unknown");

			if (src == 'S')
				Form1.PacketRecv('G', name, stream);
			else
				Form1.PacketSend('G', name, stream);
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
				packet_db[PacketId].func(ref stream);

			return stream;
		}
	}
}
