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
            public string name;
			public PacketAction func;
		}
        public static Dictionary<short, Packet> LoadAuthPackets()
		{
            Dictionary<short, Packet> packets_db = new Dictionary<short, Packet>();

            packets_db.Add(0x0, new Packet()    { name = "TS_SC_RESULT", func = AuthPackets.TS_SC_RESULT });
            packets_db.Add(0x47, new Packet()   { name = "TS_CA_RSA_PUBLIC_KEY", func = AuthPackets.TS_CA_RSA_PUBLIC_KEY });
            packets_db.Add(0x48, new Packet()   { name = "TS_AC_AES_KEY_IV", func = AuthPackets.TS_AC_AES_KEY_IV });
            packets_db.Add(0x270F, new Packet() { name = "TS_DUMMY", func = AuthPackets.TS_DUMMY });
            packets_db.Add(0x2710, new Packet() { name = "TS_AC_RESULT", func = AuthPackets.TS_AC_RESULT });
            packets_db.Add(0x2711, new Packet() { name = "TS_CA_VERSION", func = AuthPackets.TS_CA_VERSION });
            packets_db.Add(0x2712, new Packet() { name = "TS_AC_RESULT_WITH_STRING", func = AuthPackets.TS_AC_RESULT_WITH_STRING });
            packets_db.Add(0x271A, new Packet() { name = "TS_CA_OTP_ACCOUNT", func = AuthPackets.TS_CA_OTP_ACCOUNT });
            packets_db.Add(0x271C, new Packet() { name = "TS_CA_IMBC_ACCOUNT", func = AuthPackets.TS_CA_IMBC_ACCOUNT });
            packets_db.Add(0x2725, new Packet() { name = "TS_CA_SERVER_LIST", func = AuthPackets.TS_CA_SERVER_LIST });
            packets_db.Add(0x2726, new Packet() { name = "TS_AC_SERVER_LIST", func = AuthPackets.TS_AC_SERVER_LIST });
            packets_db.Add(0x2727, new Packet() { name = "TS_CA_SELECT_SERVER", func = AuthPackets.TS_CA_SELECT_SERVER });
            packets_db.Add(0x2728, new Packet() { name = "TS_AC_SELECT_SERVER", func = AuthPackets.TS_AC_SELECT_SERVER });
            packets_db.Add(0x272A, new Packet() { name = "TS_CA_DISTRIBUTION_INFO", func = AuthPackets.TS_CA_DISTRIBUTION_INFO });
			
			return packets_db;
		}

		public static Dictionary<short, Packet> LoadGamePackets()
		{
			Dictionary<short, Packet> packets_db = new Dictionary<short, Packet>();

			packets_db.Add(0x0000, new Packet() { name = "TS_SC_RESULT", func = GamePackets.TS_SC_RESULT });
			packets_db.Add(0x0001, new Packet() { name = "TS_LOGIN", func = GamePackets.TS_LOGIN });
			packets_db.Add(0x0002, new Packet() { name = "TS_TIMESYNC", func = GamePackets.TS_TIMESYNC });
			packets_db.Add(0x0003, new Packet() { name = "send_EntityAck", func = GamePackets.send_EntityAck });
			packets_db.Add(0x0004, new Packet() { name = "TS_LOGIN_RESULT", func = GamePackets.TS_LOGIN_RESULT });
			packets_db.Add(0x0005, new Packet() { name = "parse_PCMoveReq", func = GamePackets.parse_PCMoveReq });
			packets_db.Add(0x0007, new Packet() { name = "parse_PCMoveUpdt", func = GamePackets.parse_PCMoveUpdt });
			packets_db.Add(0x0008, new Packet() { name = "send_PCMove", func = GamePackets.send_PCMove });
			packets_db.Add(0x000A, new Packet() { name = "send_A", func = GamePackets.send_A });
			packets_db.Add(0x000B, new Packet() { name = "send_RegionAck", func = GamePackets.send_RegionAck });
			packets_db.Add(0x0014, new Packet() { name = "parse_Chat", func = GamePackets.parse_Chat });
			packets_db.Add(0x0015, new Packet() { name = "send_Chat", func = GamePackets.send_Chat });
			packets_db.Add(0x0016, new Packet() { name = "TS_SC_CHAT", func = GamePackets.TS_SC_CHAT });
			packets_db.Add(0x0017, new Packet() { name = "parse_LogoutToChar", func = GamePackets.parse_LogoutToChar });
			packets_db.Add(0x0019, new Packet() { name = "parse_LogoutToCharCheck", func = GamePackets.parse_LogoutToCharCheck });
			packets_db.Add(0x001A, new Packet() { name = "parse_QuitGameCheck", func = GamePackets.parse_QuitGameCheck });
			packets_db.Add(0x001B, new Packet() { name = "parse_QuitGame", func = GamePackets.parse_QuitGame });

            packets_db.Add(0x0032, new Packet() { name = "TS_CS_VERSION", func = GamePackets.TS_CS_VERSION });
			packets_db.Add(0x0033, new Packet() { name = "parse_ClientVersion", func = GamePackets.parse_ClientVersion });
			packets_db.Add(0x0037, new Packet() { name = "send_37", func = GamePackets.send_37 });

			packets_db.Add(0x0064, new Packet() { name = "parse_PCAttack", func = GamePackets.parse_PCAttack });
			packets_db.Add(0x0065, new Packet() { name = "send_Attack", func = GamePackets.send_Attack });

			packets_db.Add(0x00C8, new Packet() { name = "parse_Equip", func = GamePackets.parse_Equip });
			packets_db.Add(0x00C9, new Packet() { name = "parse_Unequip", func = GamePackets.parse_Unequip });
			packets_db.Add(0x00CA, new Packet() { name = "TS_WEAR_INFO", func = GamePackets.TS_WEAR_INFO });
			packets_db.Add(0x00CF, new Packet() { name = "TS_SC_INVENTORY", func = GamePackets.TS_SC_INVENTORY });

			packets_db.Add(0x00D8, new Packet() { name = "TS_SC_BELT_SLOT_INFO", func = GamePackets.TS_SC_BELT_SLOT_INFO });

			packets_db.Add(0x011A, new Packet() { name = "send_ItemDrop", func = GamePackets.send_ItemDrop });
			//packets_db.Add(0x011F, new Packet() { name = "parse_WearChange", func = GamePackets.parse_WearChange });

			//packets_db.Add(0x012F, new Packet() { name = "send_12F", func = GamePackets.send_12F });
			
			packets_db.Add(0x0190, new Packet() { name = "parse_UseSkill", func = GamePackets.parse_UseSkill });
			packets_db.Add(0x0191, new Packet() { name = "send_Unamed191", func = GamePackets.send_Unamed191 });
			packets_db.Add(0x0192, new Packet() { name = "parse_LearnSkill", func = GamePackets.parse_LearnSkill });
			packets_db.Add(0x0193, new Packet() { name = "send_SkillList", func = GamePackets.send_SkillList });
			packets_db.Add(0x0194, new Packet() { name = "send_194", func = GamePackets.send_194 });

			packets_db.Add(0x019A, new Packet() { name = "parse_JobLevelUp", func = GamePackets.parse_JobLevelUp });

			packets_db.Add(0x01F7, new Packet() { name = "parse_1F7", func = GamePackets.parse_1F7 });

			packets_db.Add(0x01F4, new Packet() { name = "TS_SC_STATUS_CHANGE", func = GamePackets.TS_SC_STATUS_CHANGE });
			packets_db.Add(0x01FB, new Packet() { name = "TS_SC_PROPERTY", func = GamePackets.TS_SC_PROPERTY });
			packets_db.Add(0x01FC, new Packet() { name = "parse_SetProperty", func = GamePackets.parse_SetProperty });
			packets_db.Add(0x01FD, new Packet() { name = "send_MaxHPMPUpdate", func = GamePackets.send_MaxHPMPUpdate });
			packets_db.Add(0x01FF, new Packet() { name = "parse_Target", func = GamePackets.parse_Target });
			packets_db.Add(0x0204, new Packet() { name = "send_UpdateHPMP", func = GamePackets.send_UpdateHPMP });

			packets_db.Add(0x0258, new Packet() { name = "TS_SC_QUEST_LIST", func = GamePackets.TS_SC_QUEST_LIST });
			packets_db.Add(0x0259, new Packet() { name = "send_QuestUpdate", func = GamePackets.send_QuestUpdate });


			packets_db.Add(0x0226, new Packet() { name = "parse_226", func = GamePackets.parse_226 });

			packets_db.Add(0x0384, new Packet() { name = "parse_384", func = GamePackets.parse_384 });
			packets_db.Add(0x0385, new Packet() { name = "TS_SC_CHANGE_LOCATION", func = GamePackets.TS_SC_CHANGE_LOCATION });
			packets_db.Add(0x0386, new Packet() { name = "TS_SC_WEATHER_INFO", func = GamePackets.TS_SC_WEATHER_INFO });

			packets_db.Add(0x03E8, new Packet() { name = "TS_SC_STAT_INFO", func = GamePackets.TS_SC_STAT_INFO });
			packets_db.Add(0x03E9, new Packet() { name = "TS_SC_GOLD_UPDATE", func = GamePackets.TS_SC_GOLD_UPDATE });
			packets_db.Add(0x03EA, new Packet() { name = "TS_SC_LEVEL_UPDATE", func = GamePackets.TS_SC_LEVEL_UPDATE });
			packets_db.Add(0x03EB, new Packet() { name = "TS_SC_EXP_UPDATE", func = GamePackets.TS_SC_EXP_UPDATE });

			packets_db.Add(0x044C, new Packet() { name = "parse_44C", func = GamePackets.parse_44C });
			packets_db.Add(0x044D, new Packet() { name = "TS_SC_GAME_TIME", func = GamePackets.TS_SC_GAME_TIME });

			packets_db.Add(0x07D1, new Packet() { name = "TS_CS_CHARACTER_LIST", func = GamePackets.TS_CS_CHARACTER_LIST });
			packets_db.Add(0x07D2, new Packet() { name = "TS_CS_CREATE_CHARACTER", func = GamePackets.TS_CS_CREATE_CHARACTER });
			packets_db.Add(0x07D3, new Packet() { name = "TS_CS_DELETE_CHARACTER", func = GamePackets.TS_CS_DELETE_CHARACTER });
			packets_db.Add(0x07D4, new Packet() { name = "TS_SC_CHARACTER_LIST", func = GamePackets.TS_SC_CHARACTER_LIST });
			packets_db.Add(0x07D5, new Packet() { name = "TS_CS_ACCOUNT_WITH_AUTH", func = GamePackets.TS_CS_ACCOUNT_WITH_AUTH });
			packets_db.Add(0x07D6, new Packet() { name = "TS_CS_CHECK_CHARACTER_NAME", func = GamePackets.TS_CS_CHECK_CHARACTER_NAME });

			packets_db.Add(0x0BB8, new Packet() { name = "send_Dialog", func = GamePackets.send_Dialog });
			packets_db.Add(0x0BB9, new Packet() { name = "parse_DialOpt", func = GamePackets.parse_DialOpt });
			packets_db.Add(0x0BBA, new Packet() { name = "parse_Contact", func = GamePackets.parse_Contact });

			packets_db.Add(0x1F40, new Packet() { name = "parse_SystemSpecs", func = GamePackets.parse_SystemSpecs });

			packets_db.Add(0x2328, new Packet() { name = "TS_SC_OPEN_URL", func = GamePackets.TS_SC_OPEN_URL });
			packets_db.Add(0x2329, new Packet() { name = "TS_SC_URL_LIST", func = GamePackets.TS_SC_URL_LIST });

            packets_db.Add(0x232C, new Packet() { name = "TS_SC_REQUEST_SECURITY_NO", func = GamePackets.TS_SC_REQUEST_SECURITY_NO });
            packets_db.Add(0x232D, new Packet() { name = "TS_CS_SECURITY_NO", func = GamePackets.TS_CS_SECURITY_NO });

            packets_db.Add(0x270F, new Packet() { name = "parse_270F", func = GamePackets.parse_270F });
			return packets_db;
		}

		internal enum LoginResult : short
		{
			LOGINRESULT_SUCCESS = 0x0,
			LOGINRESULT_FAIL = 0x1,
		}
	}
}
