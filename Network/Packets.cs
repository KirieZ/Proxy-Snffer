// Copyright (c) Tartarus Dev Team, licensed under GNU GPL.
// See the LICENSE file
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer.Network
{
	public class Packets
	{
		public delegate void PacketAction(ref PacketStream stream);

		public struct Packet
		{
			public PacketAction func;
		}

		public static Dictionary<short, Packet> LoadAuthPackets()
		{
			Dictionary<short, Packet> packets_db = new Dictionary<short, Packet>();

			packets_db.Add(0x2726, new Packet() { func = AuthPackets.send_ServerList });
			/*
			// [0x270F] 9999 -> Unknown1 (Client)
			packets_db.Add(0x270F, new Packet() { lenght = 11, func = AuthPackets.parse_Unknown1, pos = null });
			// [0x2710] 10000 -> Login Result (Server)
			packets_db.Add(0x2710, new Packet() { lenght = 15 });
			// [0x2711] 10001 -> Client Version (?) (Client)
			packets_db.Add(0x2711, new Packet() { lenght = 27, func = AuthPackets.parse_ClientVersion, pos = new short[] { 0 } });

			// [0x271A] 10010 -> Login Try (Client)
			packets_db.Add(0x271A, new Packet() { lenght = 129, func = AuthPackets.parse_LoginTry, pos = new short[] { 0, 61 } });

			// [0x2725] 10021 -> Request Server List (Client)
			packets_db.Add(0x2725, new Packet() { lenght = 7, func = AuthPackets.parse_RequestServerList });
			// [0x2726] 10022 -> Server List (Server)
			packets_db.Add(0x2726, new Packet() { lenght = 313, func = null });
			// [0x2727] 10023 -> Join Game Server (Client)
			packets_db.Add(0x2727, new Packet() { lenght = 9, func = AuthPackets.parse_JoinGameServer, pos = new short[] { 0 } });
			// [0x2728] 10024 -> Allow Join (?) (Server)
			packets_db.Add(0x2728, new Packet() { lenght = 21 });
			*/
			return packets_db;
		}

		public static Dictionary<short, Packet> LoadGamePackets()
		{
			Dictionary<short, Packet> packets_db = new Dictionary<short, Packet>();

			
			return packets_db;
		}

		internal enum LoginResult : short
		{
			LOGINRESULT_SUCCESS = 0x0,
			LOGINRESULT_FAIL = 0x1,
		}
	}
}
