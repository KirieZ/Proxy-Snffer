using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace RappelzSniffer.Network
{
	public static class AuthPackets
	{
		private static Dictionary<short, Packets.Packet> packet_db;
		private static Dictionary<short, string> packet_names;

		static AuthPackets()
		{
			packet_db = Packets.LoadAuthPackets();
			LoadPacketNames();
		}

		static void LoadPacketNames()
		{
			packet_names = new Dictionary<short, string>();
			if (File.Exists("packets/auth_packets.txt"))
			{
				string[] lines = File.ReadAllLines("packets/auth_packets.txt");

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

		internal static string GetPacketName(short id)
		{
			return (packet_names.ContainsKey(id) ? packet_names[id] : "Unknown");
		}

		internal static PacketStream PacketReceived(char src, PacketStream stream)
		{
			// Header
			// [Size:4]
			// [ID:2]
			// [Checksum(?):1]
			short PacketId = stream.GetId();
			stream.ReadByte();

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

			if (packet_db.ContainsKey(PacketId))
				packet_db[PacketId].func(ref stream);
			else
			{
				if (src == 'S')
					Form1.PacketRecv('A', GetPacketName(PacketId), stream);
				else
					Form1.PacketSend('A', GetPacketName(PacketId), stream);
			}

			return stream;
		}

		internal static void send_ServerList(ref PacketStream data)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(data.GetId()) + " [" + data.GetId() + "]");
			data.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 Unknown = " + data.ReadInt16());
			short svCount = data.ReadInt16();
			str.AppendLine("	Int16 ServerCount = " + svCount);
			str.AppendLine("	struct Servers[ServerCount]");
			str.AppendLine("	{");
			
			for (int i = 0; i < svCount; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			Int16 Index = " + data.ReadInt16());
				str.AppendLine("			String(22) Name = " + data.ReadString(0, 22));
				str.AppendLine("			String(256) URL = " + data.ReadString(0, 256));
				str.AppendLine("			String(16) IP = " + data.ReadString(0, 16));
				str.AppendLine("			Int16 Port = " + data.ReadInt16());
				str.AppendLine("			Int16 Unknown = " + data.ReadInt16());
				str.AppendLine("			Int16 Unknown = " + data.ReadInt16());
				str.AppendLine("		}");
			}

			str.AppendLine("	}");
			str.AppendLine("}");

			data.RewriteInt16(300, Config.Game_ClientPort);

			Form1.PacketRecv('A', GetPacketName(data.GetId()), data, str.ToString());
		}
		
		#region Parse Packet
		
		/// [0x270F] 9999 -> (CA) Unknown1
		internal static void parse_Unknown1(ref PacketStream pStream) { return; }

		/// [0x2711] 10001 -> (CA) Client Version
		/// <version>.20S
		internal static void parse_ClientVersion(ref PacketStream pStream) { return; }

		/// [0x271A] 10010 -> (CA) Login
		/// <username>.60S <password>.8B
		internal static void parse_LoginTry(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String UserID = " + pStream.ReadString(0, 60));
			str.AppendLine("	String UserPass = " + pStream.ReadString(0, 8));
			str.AppendLine("}");

			Form1.PacketRecv('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}
		
		/*
		/// [0x2725] 10021 -> (CA) Request Server List
		internal static void parse_RequestServerList(ref PacketStream pStream, short[] pos)
		{
			Server.OnUserRequestServerList(client);
		}

		/// [0x2726] 10023 -> (CA) Request Game Server Connection
		/// <server index>.W
		internal static void parse_JoinGameServer(ref PacketStream pStream, short[] pos)
		{
			byte server_index = pStream.ReadByte(pos[0]);
			Server.OnUserJoinGame(client, server_index);
		}

		#endregion

		#region Send Packet

		/// [0x2710] 10000 -> (AC) Login Result
		/// <packet id>.W <result>.W <0>.L
		internal static void send_LoginResult(Client client, Packets.LoginResult result)
		{
			PacketStream data = new PacketStream((short)0x2710);
			data.WriteInt16(0x271A);
			data.WriteInt16((short)result);
			data.WriteInt32(0);
			ClientManager.Instance.Send(client, data);
		}

		/// [0x2726] 10022 -> (AC) Server List
		/// <unknown>.W <servers count>.W 
		/// { <index>.W <name>.22S <notice url>.256S <ip>.16S 
		///   <port>.W <status>.W <unknown2>.W}*(server count)
		internal static void send_ServerList(Client client, GameServer[] servers)
		{
			PacketStream data = new PacketStream((short)0x2726);
			// TODO: Check these values
			data.WriteInt16(1);
			data.WriteInt16(1); //servers.Length);

			foreach (GameServer sv in servers.Where(s => s != null))
			{
				data.WriteByte(sv.Index);
				data.WriteByte(0x00);
				data.WriteString(sv.Name, 21);
				data.WriteByte(0x00);
				data.WriteString(sv.NoticeUrl, 255);
				data.WriteByte(0x00);
				data.WriteString(sv.Ip.ToString(), 15);
				data.WriteByte(0x00);
				data.WriteInt16(sv.Port);

				data.WriteInt16(0); // Server Status
				data.WriteInt16(0);
			}

			ClientManager.Instance.Send(client, data);
		}

		/// [0x2728] 10024 -> (AC) Join Game (Login Token)
		/// <unknown>.W <key>.8B <0A 00 00 00>.L
		internal static void send_JoinGame(Client client, byte[] key)
		{
			PacketStream data = new PacketStream((short)0x2728);
			data.WriteInt16(0);
			data.Write(key, 0, 8);
			data.WriteInt32(10);

			ClientManager.Instance.Send(client, data);
		}
		*/
		#endregion
	}
}
