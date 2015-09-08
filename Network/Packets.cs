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

			// [0x270F] 9999 -> Unknown1 (Client)
			packets_db.Add(0x270F, new Packet() { func = AuthPackets.parse_Unknown1 });
			// [0x2710] 10000 -> Login Result (Server)
			packets_db.Add(0x2710, new Packet() { func = AuthPackets.send_LoginResult });
			// [0x2711] 10001 -> Client Version (?) (Client)
			packets_db.Add(0x2711, new Packet() { func = AuthPackets.parse_ClientVersion });

			// [0x271A] 10010 -> Login Try (Client)
			packets_db.Add(0x271A, new Packet() { func = AuthPackets.parse_LoginTry });

			// [0x2725] 10021 -> Request Server List (Client)
			packets_db.Add(0x2725, new Packet() { func = AuthPackets.parse_RequestServerList });
			// [0x2726] 10022 -> Server List (Server)
			packets_db.Add(0x2726, new Packet() {  func = AuthPackets.send_ServerList });
			// [0x2727] 10023 -> Join Game Server (Client)
			packets_db.Add(0x2727, new Packet() { func = AuthPackets.parse_JoinGameServer });
			// [0x2728] 10024 -> Allow Join (?) (Server)
			packets_db.Add(0x2728, new Packet() { func = AuthPackets.send_JoinGame });
			
			return packets_db;
		}

		public static Dictionary<short, Packet> LoadGamePackets()
		{
			Dictionary<short, Packet> packets_db = new Dictionary<short, Packet>();

			packets_db.Add(0x0000, new Packet() { func = GamePackets.send_PacketResponse });
			packets_db.Add(0x0001, new Packet() { func = GamePackets.parse_JoinGame });
			packets_db.Add(0x0002, new Packet() { func = GamePackets.packet2 });
			packets_db.Add(0x0003, new Packet() { func = GamePackets.send_EntityAck });
			packets_db.Add(0x0004, new Packet() { func = GamePackets.send_LoginResult });
			packets_db.Add(0x0005, new Packet() { func = GamePackets.parse_PCMoveReq });
			packets_db.Add(0x0007, new Packet() { func = GamePackets.parse_PCMoveUpdt });
			packets_db.Add(0x0008, new Packet() { func = GamePackets.send_PCMove });
			packets_db.Add(0x000A, new Packet() { func = GamePackets.send_A });
			packets_db.Add(0x000B, new Packet() { func = GamePackets.send_RegionAck });
			packets_db.Add(0x0014, new Packet() { func = GamePackets.parse_ClientCmd });
			packets_db.Add(0x0016, new Packet() { func = GamePackets.parse_Scripts });
			packets_db.Add(0x0017, new Packet() { func = GamePackets.parse_LogoutToChar });
			packets_db.Add(0x0019, new Packet() { func = GamePackets.parse_LogoutToCharCheck });
			packets_db.Add(0x001A, new Packet() { func = GamePackets.parse_QuitGameCheck });
			packets_db.Add(0x001B, new Packet() { func = GamePackets.parse_QuitGame });

			packets_db.Add(0x0033, new Packet() { func = GamePackets.parse_ClientVersion });
			packets_db.Add(0x0037, new Packet() { func = GamePackets.send_37 });

			packets_db.Add(0x00C8, new Packet() { func = GamePackets.parse_Equip });
			packets_db.Add(0x00C9, new Packet() { func = GamePackets.parse_Unequip });
			packets_db.Add(0x00CA, new Packet() { func = GamePackets.send_CharView });
			packets_db.Add(0x00CF, new Packet() { func = GamePackets.send_InventoryList });

			packets_db.Add(0x00D8, new Packet() { func = GamePackets.send_BeltSlotInfo });

			packets_db.Add(0x011F, new Packet() { func = GamePackets.parse_WearChange });

			//packets_db.Add(0x012F, new Packet() { func = GamePackets.send_12F });
			packets_db.Add(0x0194, new Packet() { func = GamePackets.send_194 });

			packets_db.Add(0x0192, new Packet() { func = GamePackets.parse_LearnSkill });
			packets_db.Add(0x0193, new Packet() { func = GamePackets.send_SkillList });
			
			packets_db.Add(0x019A, new Packet() { func = GamePackets.parse_JobLevelUp });

			packets_db.Add(0x01F7, new Packet() { func = GamePackets.parse_1F7 });

			packets_db.Add(0x01F4, new Packet() { func = GamePackets.send_EntityState });
			packets_db.Add(0x01FB, new Packet() { func = GamePackets.send_Property });
			packets_db.Add(0x01FC, new Packet() { func = GamePackets.parse_SetProperty });
			packets_db.Add(0x01FD, new Packet() { func = GamePackets.send_MaxHPMPUpdate });
			packets_db.Add(0x01FF, new Packet() { func = GamePackets.parse_01FF });
			packets_db.Add(0x0204, new Packet() { func = GamePackets.send_UpdateHPMP });

			packets_db.Add(0x0258, new Packet() { func = GamePackets.send_QuestList });
			packets_db.Add(0x0259, new Packet() { func = GamePackets.send_QuestUpdate });


			packets_db.Add(0x0226, new Packet() { func = GamePackets.parse_226 });

			packets_db.Add(0x0384, new Packet() { func = GamePackets.parse_384 });
			packets_db.Add(0x0385, new Packet() { func = GamePackets.send_LocationInfo });
			packets_db.Add(0x0386, new Packet() { func = GamePackets.send_WeatherInfo });

			packets_db.Add(0x03E8, new Packet() { func = GamePackets.send_UpdateStatus });
			packets_db.Add(0x03E9, new Packet() { func = GamePackets.send_UpdateGoldChaos });
			packets_db.Add(0x03EA, new Packet() { func = GamePackets.send_UpdateLevel });
			packets_db.Add(0x03EB, new Packet() { func = GamePackets.send_UpdateExp });

			packets_db.Add(0x044C, new Packet() { func = GamePackets.parse_44C });
			packets_db.Add(0x044D, new Packet() { func = GamePackets.send_GameTime });

			packets_db.Add(0x07D1, new Packet() { func = GamePackets.parse_ReqCharList });
			packets_db.Add(0x07D2, new Packet() { func = GamePackets.parse_CreateChar });
			packets_db.Add(0x07D3, new Packet() { func = GamePackets.parse_DelChar });
			packets_db.Add(0x07D4, new Packet() { func = GamePackets.send_CharList });
			packets_db.Add(0x07D5, new Packet() { func = GamePackets.parse_LoginToken });
			packets_db.Add(0x07D6, new Packet() { func = GamePackets.parse_CharName });

			packets_db.Add(0x0BB8, new Packet() { func = GamePackets.send_Dialog });
			packets_db.Add(0x0BB9, new Packet() { func = GamePackets.parse_DialOpt });
			packets_db.Add(0x0BBA, new Packet() { func = GamePackets.parse_Contact });

			packets_db.Add(0x1F40, new Packet() { func = GamePackets.parse_SystemSpecs });

			packets_db.Add(0x2328, new Packet() { func = GamePackets.send_OpenPopup });
			packets_db.Add(0x2329, new Packet() { func = GamePackets.send_UrlList });

			packets_db.Add(0x270F, new Packet() { func = GamePackets.parse_270F });
			return packets_db;
		}

		internal enum LoginResult : short
		{
			LOGINRESULT_SUCCESS = 0x0,
			LOGINRESULT_FAIL = 0x1,
		}
	}
}
