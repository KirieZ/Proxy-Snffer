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

            packets_db.Add(0x0, new Packet()    { func = AuthPackets.TS_SC_RESULT });
            packets_db.Add(0x47, new Packet()   { func = AuthPackets.TS_CA_RSA_PUBLIC_KEY });
            packets_db.Add(0x48, new Packet()   { func = AuthPackets.TS_AC_AES_KEY_IV });
            packets_db.Add(0x270F, new Packet() { func = AuthPackets.TS_DUMMY });
            packets_db.Add(0x2710, new Packet() { func = AuthPackets.TS_AC_RESULT });
            packets_db.Add(0x2711, new Packet() { func = AuthPackets.TS_CA_VERSION });
            packets_db.Add(0x2712, new Packet() { func = AuthPackets.TS_AC_RESULT_WITH_STRING });
            packets_db.Add(0x271A, new Packet() { func = AuthPackets.TS_CA_OTP_ACCOUNT });
            packets_db.Add(0x271C, new Packet() { func = AuthPackets.TS_CA_IMBC_ACCOUNT });
            packets_db.Add(0x2725, new Packet() { func = AuthPackets.TS_CA_SERVER_LIST });
            packets_db.Add(0x2726, new Packet() { func = AuthPackets.TS_AC_SERVER_LIST });
            packets_db.Add(0x2727, new Packet() { func = AuthPackets.TS_CA_SELECT_SERVER });
            packets_db.Add(0x2728, new Packet() { func = AuthPackets.TS_AC_SELECT_SERVER });
            packets_db.Add(0x272A, new Packet() { func = AuthPackets.TS_CA_DISTRIBUTION_INFO });
			
			return packets_db;
		}

		public static Dictionary<short, Packet> LoadGamePackets()
		{
			Dictionary<short, Packet> packets_db = new Dictionary<short, Packet>();

            #region Packets 0 to 16
            packets_db.Add(0x0000, new Packet() { func = GamePackets.TS_SC_RESULT });
			packets_db.Add(0x0001, new Packet() { func = GamePackets.TS_CS_LOGIN });
			packets_db.Add(0x0002, new Packet() { func = GamePackets.TS_TIMESYNC });
			packets_db.Add(0x0003, new Packet() { func = GamePackets.TS_SC_ENTER });
			packets_db.Add(0x0004, new Packet() { func = GamePackets.TS_SC_LOGIN_RESULT });
			packets_db.Add(0x0005, new Packet() { func = GamePackets.TS_CS_MOVE_REQUEST });
            packets_db.Add(0x0006, new Packet() { func = GamePackets.TS_SC_MOVE_ACK });
            packets_db.Add(0x0007, new Packet() { func = GamePackets.TS_CS_REGION_UPDATE });
			packets_db.Add(0x0008, new Packet() { func = GamePackets.TS_SC_MOVE });
            packets_db.Add(0x0009, new Packet() { func = GamePackets.TS_SC_LEAVE });
            packets_db.Add(0x000A, new Packet() { func = GamePackets.TS_SC_SET_TIME });
			packets_db.Add(0x000B, new Packet() { func = GamePackets.TS_SC_REGION_ACK });
            packets_db.Add(0x000C, new Packet() { func = GamePackets.TS_SC_WARP });
            packets_db.Add(0x000D, new Packet() { func = GamePackets.TS_CS_QUERY });
            packets_db.Add(0x000F, new Packet() { func = GamePackets.TS_CS_ENTER_EVENT_AREA });
            packets_db.Add(0x0010, new Packet() { func = GamePackets.TS_CS_LEAVE_EVENT_AREA });
            #endregion
            #region Packets 20 to 150
            packets_db.Add(0x0014, new Packet() { func = GamePackets.TS_CS_CHAT_REQUEST });
			packets_db.Add(0x0015, new Packet() { func = GamePackets.TS_SC_CHAT_LOCAL });
			packets_db.Add(0x0016, new Packet() { func = GamePackets.TS_SC_CHAT });
			packets_db.Add(0x0017, new Packet() { func = GamePackets.TS_CS_RETURN_LOBBY });
            //18 TS_SC_CHAT_RESULT
            packets_db.Add(0x0019, new Packet() { func = GamePackets.TS_CS_REQUEST_RETURN_LOBBY });
			packets_db.Add(0x001A, new Packet() { func = GamePackets.TS_CS_REQUEST_LOGOUT });
			packets_db.Add(0x001B, new Packet() { func = GamePackets.TS_CS_LOGOUT });
            //1c TS_SC_DISCONNECT_DESC
            //1e TS_SC_CHANGE_NAME
            //1f TS_CS_CHANGE_ALIAS
            packets_db.Add(0x0033, new Packet() { func = GamePackets.TS_CS_VERSION });
            //35 TS_SC_ANTI_HACK
            //36 TS_CS_ANTI_HACK
            packets_db.Add(0x0037, new Packet() { func = GamePackets.TS_SC_GAME_GUARD_AUTH_QUERY });
            //38 TS_CS_GAME_GUARD_AUTH_ANSWER
            //39 TS_CS_CHECK_ILLEGAL_USER
            //3a TS_SC_XTRAP_CHECK
            //3b TS_CS_XTRAP_CHECK
            //3d TS_CS_LOGIN_2
            //3e TS_CS_LOGIN_3
            //3f TS_SC_ENTER_2
            //40 TS_SC_LOGIN_RESULT_2
            //41 TS_CS_MOVE_REQUEST_2
            //43 TS_CS_REGION_UPDATE_2
            packets_db.Add(0x0064, new Packet() { func = GamePackets.TS_CS_ATTACK_REQUEST });
			packets_db.Add(0x0065, new Packet() { func = GamePackets.TS_SC_ATTACK_EVENT });
            //66 TS_SC_CANT_ATTACK
            //67 TS_SC_DOUBLE_WEAPON_ATTACK_EVENT
            //96 TS_CS_CANCEL_ACTION
            #endregion

            packets_db.Add(0x00C8, new Packet() { func = GamePackets.parse_Equip });
			packets_db.Add(0x00C9, new Packet() { func = GamePackets.parse_Unequip });
			packets_db.Add(0x00CA, new Packet() { func = GamePackets.send_CharView });
			packets_db.Add(0x00CF, new Packet() { func = GamePackets.send_InventoryList });

			packets_db.Add(0x00D8, new Packet() { func = GamePackets.send_BeltSlotInfo });

			packets_db.Add(0x011A, new Packet() { func = GamePackets.send_ItemDrop });
			//packets_db.Add(0x011F, new Packet() { func = GamePackets.parse_WearChange });

			//packets_db.Add(0x012F, new Packet() { func = GamePackets.send_12F });
			
			packets_db.Add(0x0190, new Packet() { func = GamePackets.parse_UseSkill });
			packets_db.Add(0x0191, new Packet() { func = GamePackets.send_Unamed191 });
			packets_db.Add(0x0192, new Packet() { func = GamePackets.parse_LearnSkill });
			packets_db.Add(0x0193, new Packet() { func = GamePackets.send_SkillList });
			packets_db.Add(0x0194, new Packet() { func = GamePackets.send_194 });

			packets_db.Add(0x019A, new Packet() { func = GamePackets.parse_JobLevelUp });

			packets_db.Add(0x01F7, new Packet() { func = GamePackets.parse_1F7 });

			packets_db.Add(0x01F4, new Packet() { func = GamePackets.send_EntityState });
			packets_db.Add(0x01FB, new Packet() { func = GamePackets.send_Property });
			packets_db.Add(0x01FC, new Packet() { func = GamePackets.parse_SetProperty });
			packets_db.Add(0x01FD, new Packet() { func = GamePackets.send_MaxHPMPUpdate });
			packets_db.Add(0x01FF, new Packet() { func = GamePackets.parse_Target });
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
