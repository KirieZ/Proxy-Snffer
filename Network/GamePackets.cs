﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace RappelzSniffer.Network
{
	public static class GamePackets
	{
		private static Dictionary<short, Packets.Packet> packet_db;
		private static Dictionary<short, string> packet_names;
		private static char Src = ' ';

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

		internal static string GetPacketName(short id)
		{
			return (packet_names.ContainsKey(id) ? packet_names[id] : "Unknown");
		}

		internal static PacketStream PacketReceived(char src, PacketStream stream)
		{
			Src = src;
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
					Form1.PacketRecv('G', GetPacketName(PacketId), stream);
				else
					Form1.PacketSend('G', GetPacketName(PacketId), stream);
			}

			return stream;
		}

		internal static void TS_CS_LOGIN(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(19) CharName = " + stream.ReadString(0, 19));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

        internal static void TS_SC_WARP(ref PacketStream stream)
        {
            //#define TS_SC_WARP_DEF(_) \
            //            _(simple)(float, x) \
            //	_(simple)(float, y) \
            //	_(simple)(float, z) \
            //	_(simple)(char, layer)
        }

        internal static void TS_CS_QUERY(ref PacketStream stream)
        {
            //        define TS_CS_QUERY_DEF(_) \
            //_(simple)(uint32_t, handle)
        }


        internal static void TS_CS_ENTER_EVENT_AREA(ref PacketStream stream)
        {
            //#define TS_CS_ENTER_EVENT_AREA_DEF(_) \
            //            _(simple)(int32_t, event_area_id) \
            //	_(simple)(int32_t, area_index)

        }

        internal static void TS_CS_LEAVE_EVENT_AREA(ref PacketStream stream)
        {
            //#define TS_CS_LEAVE_EVENT_AREA_DEF(_) \
            //            _(simple)(int32_t, event_area_id) \
            //	_(simple)(int32_t, area_index)
        }

        internal static void TS_SC_MOVE_ACK(ref PacketStream stream)
        {
            //#define TS_SC_MOVE_ACK_DEF(_) \
            //            _(simple)(uint32_t, time) \
            //	_(simple)(char, speed)
            // *** NOTE : IT CAN BE A DUMMY AS STATED BELOW, BUT U CAN ALSO TRY THE EFFORT AND FILL TIME,SPEED ***
            //CREATE_PACKET(TS_SC_MOVE_ACK, 6);
        }

        internal static void TS_SC_LEAVE(ref PacketStream stream)
        {

            //#define TS_SC_LEAVE_DEF(_) \
            //            _(simple)(uint32_t, handle)
        }



        internal static void TS_CS_MOVE_REQUEST(ref PacketStream stream)
		{
            //#define MOVE_REQUEST_INFO_DEF(_) \
            //            _(simple)(float, tx) \
            //	_(simple)(float, ty)

            //CREATE_STRUCT(MOVE_REQUEST_INFO);

            //#define TS_CS_MOVE_REQUEST_DEF(_) \
            //            _(simple)(uint32_t, handle) \
            //	_(simple)(float, x) \
            //	_(simple)(float, y) \
            //	_(simple)(uint32_t, cur_time) \
            //	_(simple)(bool, speed_sync) \
            //	_(count)(uint16_t, count, move_infos) \
            //	_(dynarray)(MOVE_REQUEST_INFO, move_infos)

            //#define TS_CS_MOVE_REQUEST_ID(X) \
            //            X(5, version < EPIC_9_2) \
            //	X(65, version >= EPIC_9_2)

            StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 player_handle = " + stream.ReadUInt32());
			str.AppendLine("	Single from_x = " + stream.ReadFloat());
			str.AppendLine("	Single from_y = " + stream.ReadFloat());
			str.AppendLine("	UInt32 move_time = " + stream.ReadUInt32());
			str.AppendLine("	Byte speed_sync = " + stream.ReadByte());
			short c = stream.ReadInt16();
			str.AppendLine("	Int16 move_count = " + c);
			str.AppendLine("	struct move_positions[move_count]");
			str.AppendLine("	{");
			for (int i = 0; i < c; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			Single to_x = " + stream.ReadFloat());
				str.AppendLine("			Single to_y = " + stream.ReadFloat());
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_REGION_UPDATE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 update_time = " + stream.ReadInt32());
			str.AppendLine("	Single current_x = " + stream.ReadFloat());
			str.AppendLine("	Single current_y = " + stream.ReadFloat());
			str.AppendLine("	Single current_z = " + stream.ReadFloat());
			str.AppendLine("	Byte stop = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_CHAT_REQUEST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(21) target = " + stream.ReadString(0, 21));
			str.AppendLine("	byte request_id = " + stream.ReadByte());
			byte size = stream.ReadByte();
			str.AppendLine("	byte len = " + size);
			str.AppendLine("	byte type = " + stream.ReadByte());
			str.AppendLine("	String(len) Command = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_RETURN_LOBBY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_REQUEST_RETURN_LOBBY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_REQUEST_LOGOUT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_LOGOUT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_Equip(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 ItemHandle = " + stream.ReadUInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_Unequip(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int WearType = " + stream.ReadInt32());
			str.AppendLine("	Byte Unknown = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_WearChange(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 item_handle = " + stream.ReadUInt32());
			str.AppendLine("	Int16 wear_type = " + stream.ReadInt16());
			str.AppendLine("	UInt32 player_handle = " + stream.ReadUInt32());
			str.AppendLine("	Byte item_enhance = " + stream.ReadByte());
			str.AppendLine("	2B Unknown = " + stream.ReadInt16());
			str.AppendLine("	2B Unknown = " + stream.ReadInt16());
			str.AppendLine("	2B Unknown = " + stream.ReadInt16());
			str.AppendLine("	2B Unknown = " + stream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_SetProperty(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(16) PropertyName = " + stream.ReadString(0, 16));
			str.AppendLine("	String(?S)  = " + stream.ReadString(0, stream.GetSize() - 23));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_Target(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint target = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_ReqCharList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(60) userId = " + stream.ReadString(0, 60));
			str.AppendLine("	Byte Unknown = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_CreateChar(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 sex = " + stream.ReadInt32());
			str.AppendLine("	Int32 race = " + stream.ReadInt32());
			str.AppendLine("	Int32 hair_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 face_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 body_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 hands_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 feet_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 hair_color = " + stream.ReadInt32());

			// 32 ~ 39
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());

			str.AppendLine("	Int32 face_detail = " + stream.ReadInt32());

			// 44 ~ 51
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());

			str.AppendLine("	Int32 clothes_id = " + stream.ReadInt32());

			// 56 ~ 168
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());

			str.AppendLine("	String(19) name = " + stream.ReadString(0, 19));
			str.AppendLine("	Int32 skin_color = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_DelChar(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(19) CharName = " + stream.ReadString(0, 19));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_LoginToken(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(60) = " + stream.ReadString(0, 60));
			str.AppendLine("	Byte Unknown = " + stream.ReadByte());
			str.Append("	Byte[8] Token = ");
			for (int i = 0; i < 8; i++)
				str.Append(stream.ReadByte() + " ");
			str.Append("\r\n");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_CharName(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(19) name = " + stream.ReadString(0, 19));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_DialOpt(ref PacketStream stream)
		{
			short size = stream.ReadInt16();
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 size = " + size);
			str.AppendLine("	String(size) Function = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_Contact(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int NpcHandle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_LearnSkill(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	Int32 skill_id = " + stream.ReadInt32());
			str.AppendLine("	Int16 target_level = " + stream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}
		
		internal static void parse_JobLevelUp(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void parse_UseSkill(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt16 skill_id = " + stream.ReadUInt16());
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	4B unknown = " + stream.ReadInt32());
			str.AppendLine("	4B unknown = " + stream.ReadInt32());
			str.AppendLine("	4B unknown = " + stream.ReadInt32());
			str.AppendLine("	1B unknown = " + stream.ReadByte());
			str.AppendLine("	Byte level = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		//========== Send

		internal static void TS_SC_RESULT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 request_msg_id = " + stream.ReadInt16());
			str.AppendLine("	Int16 result = " + stream.ReadInt16());
			str.AppendLine("	UInt32 value = " + stream.ReadUInt32());
            //static const uint16_t packetID = 0;
            str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_ENTER(ref PacketStream stream)
		{
            //            enum TS_SC_ENTER__OBJ_TYPE : uint8_t
            //        {
            //            EOT_Player,
            //            EOT_NPC,
            //            EOT_Item,
            //            EOT_Monster,
            //            EOT_Summon,
            //            EOT_Skill,
            //            EOT_FieldProp,
            //            EOT_Pet
            //        };

            //#define TS_SC_ENTER__ITEM_PICK_UP_ORDER_DEF(_) \
            //    _(simple) (uint32_t, drop_time) \

            //    _(array)  (uint32_t, hPlayer, 3) \

            //    _(array)  (int32_t, nPartyID, 3)
            //CREATE_STRUCT(TS_SC_ENTER__ITEM_PICK_UP_ORDER);

            //#define TS_SC_ENTER__ITEM_INFO_DEF(_) \
            //    _(simple) (EncodedInt<EncodingRandomized>, code) \

            //    _(simple) (uint64_t, count) \

            //    _(simple) (TS_SC_ENTER__ITEM_PICK_UP_ORDER, pick_up_order)
            //CREATE_STRUCT(TS_SC_ENTER__ITEM_INFO);

            //#define TS_SC_ENTER__SKILL_INFO_DEF(_) \
            //    _(simple) (uint32_t, casterHandle) \

            //    _(simple) (uint32_t, startTime) \

            //    _(simple) (uint32_t, skillId)
            //CREATE_STRUCT(TS_SC_ENTER__SKILL_INFO);

            //#define TS_SC_ENTER__FIELD_PROP_INFO_DEF(_) \
            //    _(simple) (uint32_t, prop_id) \

            //    _(simple) (float, fZOffset) \

            //    _(simple) (float, fRotateX) \

            //    _(simple) (float, fRotateY) \

            //    _(simple) (float, fRotateZ) \

            //    _(simple) (float, fScaleX) \

            //    _(simple) (float, fScaleY) \

            //    _(simple) (float, fScaleZ) \

            //    _(simple) (bool, bLockHeight) \

            //    _(simple) (float, fLockHeight)
            //CREATE_STRUCT(TS_SC_ENTER__FIELD_PROP_INFO);

            //#define TS_SC_ENTER__CREATURE_INFO_DEF(_) \
            //    _(simple) (uint32_t, status) \

            //    _(simple) (float, face_direction) \

            //    _(simple) (int32_t, hp) \

            //    _(simple) (int32_t, max_hp) \

            //    _(simple) (int32_t, mp) \

            //    _(simple) (int32_t, max_mp) \

            //    _(simple) (int32_t, level) \

            //    _(simple) (uint8_t, race) \

            //    _(simple) (uint32_t, skin_color) \

            //    _(simple) (bool, is_first_enter) \

            //    _(simple) (int32_t, energy)
            //CREATE_STRUCT(TS_SC_ENTER__CREATURE_INFO);

            //#define TS_SC_ENTER__MONSTER_INFO_DEF(_) \
            //    _(simple) (TS_SC_ENTER__CREATURE_INFO, creatureInfo) \

            //    _(simple) (EncodedInt<EncodingScrambled>, monster_id) \

            //    _(simple) (bool, is_tamed)
            //CREATE_STRUCT(TS_SC_ENTER__MONSTER_INFO);

            //#define TS_SC_ENTER__SUMMON_INFO_DEF(_) \
            //    _(simple) (TS_SC_ENTER__CREATURE_INFO, creatureInfo) \

            //    _(simple) (uint32_t, master_handle) \

            //    _(simple) (EncodedInt<EncodingRandomized>, summon_code) \

            //    _(string) (szName, 19) \

            //    _(simple) (uint8_t, enhance)
            //CREATE_STRUCT(TS_SC_ENTER__SUMMON_INFO);

            //#define TS_SC_ENTER__NPC_INFO_DEF(_) \
            //    _(simple) (TS_SC_ENTER__CREATURE_INFO, creatureInfo) \

            //    _(simple) (EncodedInt<EncodingRandomized>, npc_id)
            //CREATE_STRUCT(TS_SC_ENTER__NPC_INFO);

            //#define TS_SC_ENTER__PLAYER_INFO_DEF(_) \
            //    _(simple) (TS_SC_ENTER__CREATURE_INFO, creatureInfo) \

            //    _(simple) (uint8_t, sex) \

            //    _(simple) (uint32_t, faceId) \

            //    _(simple) (uint32_t, faceTextureId) \

            //    _(simple) (uint32_t, hairId) \

            //    _(simple) (uint32_t, hairColorIndex) \

            //    _(simple) (uint32_t, hairColorRGB) \

            //    _(simple) (uint32_t, hideEquipFlag) \

            //    _(string) (szName, 19) \

            //    _(simple) (uint16_t, job_id) \

            //    _(simple) (uint32_t, ride_handle) \

            //    _(simple) (uint32_t, guild_id) \

            //    _(simple) (uint64_t, unknown, version >= EPIC_9_2)
            //CREATE_STRUCT(TS_SC_ENTER__PLAYER_INFO);

            //#define TS_SC_ENTER__PET_INFO_DEF(_) \
            //    _(simple) (TS_SC_ENTER__CREATURE_INFO, creatureInfo) \

            //    _(simple) (uint32_t, master_handle) \

            //    _(simple) (EncodedInt<EncodingRandomized>, pet_code) \

            //    _(string) (szName, 19)
            //CREATE_STRUCT(TS_SC_ENTER__PET_INFO);

            //#define TS_SC_ENTER_DEF(_) \
            //    _(simple) (uint8_t, type) /* 0 = static object, 1 = movable object, 2 = client object (ArObject::ObjectType) */ \

            //    _(simple) (uint32_t, handle) \

            //    _(simple) (float, x) \

            //    _(simple) (float, y) \

            //    _(simple) (float, z) \

            //    _(simple) (uint8_t, layer) \

            //    _(simple) (TS_SC_ENTER__OBJ_TYPE, objType) \

            //    _(simple) (TS_SC_ENTER__PLAYER_INFO    , playerInfo   , objType == EOT_Player) \

            //    _(simple) (TS_SC_ENTER__NPC_INFO       , npcInfo      , objType == EOT_NPC) \

            //    _(simple) (TS_SC_ENTER__ITEM_INFO      , itemInfo     , objType == EOT_Item) \

            //    _(simple) (TS_SC_ENTER__MONSTER_INFO   , monsterInfo  , objType == EOT_Monster) \

            //    _(simple) (TS_SC_ENTER__SUMMON_INFO    , summonInfo   , objType == EOT_Summon) \

            //    _(simple) (TS_SC_ENTER__SKILL_INFO     , skillInfo    , objType == EOT_Skill) \

            //    _(simple) (TS_SC_ENTER__FIELD_PROP_INFO, fieldPropInfo, objType == EOT_FieldProp) \

            //    _(simple) (TS_SC_ENTER__PET_INFO       , petInfo      , objType == EOT_Pet)

            //#define TS_SC_ENTER_ID(X) \
            //    X(3, version<EPIC_9_2) \
            //    X(63, version >= EPIC_9_2)

            //CREATE_PACKET_VER_ID(TS_SC_ENTER);
        StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			byte mainType = stream.ReadByte();
			str.Append("	Byte main_type = (" + mainType + ") ");
			switch (mainType)
			{
				case 0: str.Append("Player\r\n"); break;
				case 1: str.Append("NPC\r\n"); break;
				case 2: str.Append("Static Object\r\n"); break;
			}
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	Single x = " + stream.ReadFloat());
			str.AppendLine("	Single y = " + stream.ReadFloat());
			str.AppendLine("	Single z = " + stream.ReadFloat());
			str.AppendLine("	Byte layer = " + stream.ReadByte());
			byte subType = stream.ReadByte();
			str.Append("	Byte sub_type = (" + subType +") ");
			switch (subType)
			{
				case 0: str.Append("Player\r\n"); break;
				case 1: str.Append("NPC\r\n"); break;
				case 2: str.Append("Item\r\n"); break;
				case 3: str.Append("Mob\r\n"); break;
				case 4: str.Append("Summon\r\n"); break;
				case 5: str.Append("SkillProp\r\n"); break;
				case 6: str.Append("FieldProp\r\n"); break;
				case 7: str.Append("Pet\r\n"); break;
			}
			str.AppendLine("	");
			if (mainType == 0)
			{ // Player
				str.AppendLine("	[Extra Info]");
			}
			else if (mainType == 1)
			{ // NPC
				str.AppendLine("	uint status = " + stream.ReadUInt32());
				str.AppendLine("	float face_dir = " + stream.ReadFloat());
				str.AppendLine("	int hp = " + stream.ReadInt32());
				str.AppendLine("	int max_hp = " + stream.ReadInt32());
				str.AppendLine("	int mp = " + stream.ReadInt32());
				str.AppendLine("	int max_mp = " + stream.ReadInt32());
				str.AppendLine("	int level = " + stream.ReadInt32());
				
				str.AppendLine("	byte race = " + stream.ReadByte());
				str.AppendLine("	uint skin_color = " + stream.ReadUInt32());
				str.AppendLine("	bool is_first_enter = " + stream.ReadBool());
				str.AppendLine("	int energy = " + stream.ReadInt32());

				if (subType == 0)
				{ // Player

				}
				else if (subType == 1)
				{// NPC
					long encId = stream.ReadInt64();
					str.AppendLine("	Int64 encrypted_id = (" + encId  + ") " + EncryptedInt.Revert(encId));
				}
				else if (subType == 3)
				{// Mob
					long encId = stream.ReadInt64();
					str.AppendLine("	Int64 encrypted_id = (" + encId + ") " + EncryptedInt.Revert(encId));
				}
				else if (subType == 4)
				{// Summon
					str.AppendLine("	uint master_handle = " + stream.ReadUInt32());
					long encId = stream.ReadInt64();
					str.AppendLine("	Int64 encrypted_id = (" + encId + ") " + EncryptedInt.Revert(encId));
					str.AppendLine("	char[19] name = " + stream.ReadString(0, 19));
				}
				else if (subType == 7)
				{// Pet
					str.AppendLine("	uint master_handle = " + stream.ReadUInt32());
					long encId = stream.ReadInt64();
					str.AppendLine("	Int64 encrypted_id = (" + encId + ") " + EncryptedInt.Revert(encId));
					str.AppendLine("	char[19] name = " + stream.ReadString(0, 19));
				}
				else
				{
					str.AppendLine("	[Extra Info]");
				}
			}
			else if (mainType == 2)
			{ // Static

				if (subType == 2)
				{ // Item
					long encId = stream.ReadInt64();
					str.AppendLine("	Int64 encrypted_id = (" + encId + ") " + EncryptedInt.Revert(encId));
					str.AppendLine("	Int64 count = " + stream.ReadInt64());
					str.AppendLine("	uint drop_time = " + stream.ReadUInt32());
					str.AppendLine("	uint player1 = " + stream.ReadUInt32());
					str.AppendLine("	uint player2 = " + stream.ReadUInt32());
					str.AppendLine("	uint player3 = " + stream.ReadUInt32());
					str.AppendLine("	int party1 = " + stream.ReadInt32());
					str.AppendLine("	int party2 = " + stream.ReadInt32());
					str.AppendLine("	int party3 = " + stream.ReadInt32());
				}
				else if (subType == 5)
				{ //SkillProp
					str.AppendLine("	uint caster = " + stream.ReadUInt32());
					str.AppendLine("	uint start_time = " + stream.ReadUInt32());
					str.AppendLine("	int skill_id = " + stream.ReadInt32());
				}
				else if (subType == 6)
				{ // FieldProp
					str.AppendLine("	int prop_id = " + stream.ReadInt32());
					str.AppendLine("	float fZOffset = " + stream.ReadFloat());
					str.AppendLine("	float fRotateX = " + stream.ReadFloat());
					str.AppendLine("	float fRotateY = " + stream.ReadFloat());
					str.AppendLine("	float fRotateZ = " + stream.ReadFloat());
					str.AppendLine("	float fScaleX = " + stream.ReadFloat());
					str.AppendLine("	float fScaleY = " + stream.ReadFloat());
					str.AppendLine("	float fScaleZ = " + stream.ReadFloat());
					str.AppendLine("	bool bLockHeight = " + stream.ReadBool());
					str.AppendLine("	float fLockHeight = " + stream.ReadFloat());
				}
				else
				{
					str.AppendLine("	[Extra Info]");
				}
			}

			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_MOVE(ref PacketStream stream)
		{
            //#define MOVE_INFO_DEF(_) \
            //            _(simple)(float, tx) \
            //	_(simple)(float, ty)

            //CREATE_STRUCT(MOVE_INFO);

            //#define TS_SC_MOVE_DEF(_) \
            //            _(simple)(uint32_t, start_time) \
            //	_(simple)(uint32_t, handle) \
            //	_(simple)(char, tlayer) \
            //	_(simple)(char, speed) \
            //	_(count)(uint16_t, count, move_infos) \
            //	_(dynarray)(MOVE_INFO, move_infos)



            StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 time = " + stream.ReadUInt32());		/* 0 */
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());		/* 4 */
			str.AppendLine("	Byte layer = " + stream.ReadByte());			/* 8 */
			str.AppendLine("	Byte move_speed = " + stream.ReadByte());	/* 9 */
			short pcount = stream.ReadInt16();
			str.AppendLine("	Int16 point_count = " + pcount);	/* 10 */
			str.AppendLine("	struct move_positions[point_count]");
			str.AppendLine("	{");
			for (int i = 0; i < pcount; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			Single to_x = " + stream.ReadFloat());	/* 12 */
				str.AppendLine("			Single to_y = " + stream.ReadFloat());	/* 16 */
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
	}

		internal static void TS_SC_REGION_ACK(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 region_x = " + stream.ReadUInt32()); /* 0 */
			str.AppendLine("	UInt32 region_y = " + stream.ReadUInt32()); /* 4 */

			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_Property(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 player_handle = " + stream.ReadUInt32());
			bool as_int = stream.ReadBool();
			str.AppendLine("	Byte AsInt = " + as_int);
			str.AppendLine("	String(16) property_name = " + stream.ReadString(0, 16));
			str.AppendLine("	Int32 value = " + stream.ReadInt32());
			str.AppendLine("	Int32 unknown = " + stream.ReadInt32());
			if (!as_int)
				str.AppendLine("	String(?) value = " + stream.ReadString(0, stream.GetSize() - 28));
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_UpdateHPMP(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 hp_recover = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 mp_recover = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 new_hp = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 new_mp = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_UpdateStatus(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32()); /* 0 */
			str.AppendLine("	Int16 unknown1 = " + stream.ReadInt16());
			str.AppendLine("	Int16 strenght = " + stream.ReadInt16()); /* 6 */
			str.AppendLine("	Int16 vitality = " + stream.ReadInt16()); /* 8 */
			str.AppendLine("	Int16 dexterity = " + stream.ReadInt16()); /* 10 */
			str.AppendLine("	Int16 agility = " + stream.ReadInt16()); /* 12 */
			str.AppendLine("	Int16 int = " + stream.ReadInt16()); /* 14 */
			str.AppendLine("	Int16 wisdom = " + stream.ReadInt16()); /* 16 */
			str.AppendLine("	Int16 luck = " + stream.ReadInt16()); /* 18 */
			str.AppendLine("	Int16 crit_rate = " + stream.ReadInt16()); /* 20 */
			str.AppendLine("	Int16 crit_power = " + stream.ReadInt16()); /* 22 */
			str.AppendLine("	Int16 p_atk_right = " + stream.ReadInt16()); /* 24 */
			str.AppendLine("	Int16 p_atk_left = " + stream.ReadInt16()); /* 26 */
			str.AppendLine("	Int16 p_def = " + stream.ReadInt16());/* 28 */
			str.AppendLine("	Int16 block_def = " + stream.ReadInt16());/* 30 */
			str.AppendLine("	Int16 matk = " + stream.ReadInt16());/* 32 */
			str.AppendLine("	Int16 mdef = " + stream.ReadInt16());/* 34 */
			str.AppendLine("	Int16 accuracy_right = " + stream.ReadInt16());/* 36 */
			str.AppendLine("	Int16 accuracy_left = " + stream.ReadInt16()); /* 38 */
			str.AppendLine("	Int16 m_accuracy = " + stream.ReadInt16()); /* 40 */
			str.AppendLine("	Int16 evasion = " + stream.ReadInt16()); /* 42 */
			str.AppendLine("	Int16 m_res = " + stream.ReadInt16()); /* 44 */
			str.AppendLine("	Int16 block_per = " + stream.ReadInt16()); /* 46 */
			str.AppendLine("	Int16 mov_spd = " + stream.ReadInt16()); /* 48 */
			str.AppendLine("	Int16 atk_spd = " + stream.ReadInt16()); /* 50 */
			str.AppendLine("	Int16 unknown2 = " + stream.ReadInt16());
			str.AppendLine("	Int16 max_weight = " + stream.ReadInt16()); /* 54 */
			str.AppendLine("	Int16 unknown4 = " + stream.ReadInt16());
			str.AppendLine("	Int16 cast_spd = " + stream.ReadInt16());  /* 58 */
			str.AppendLine("	Int16 recast_spd = " + stream.ReadInt16());  /* 60 */
			str.AppendLine("	Int16 unknown5 = " + stream.ReadInt16());
			str.AppendLine("	Int16 hp_regen = " + stream.ReadInt16()); /* 64 */
			str.AppendLine("	Int16 hp_recov = " + stream.ReadInt16()); /* 66 */
			str.AppendLine("	Int16 mp_regen = " + stream.ReadInt16()); /* 68 */
			str.AppendLine("	Int16 mp_recov = " + stream.ReadInt16()); /* 70 */
			str.AppendLine("	Int16 perf_block = " + stream.ReadInt16()); /* 72 */
			str.AppendLine("	Int16 m_ignore = " + stream.ReadInt16()); /* 74 */
			str.AppendLine("	Int16 m_ignore_perc = " + stream.ReadInt16()); /* 76 */
			str.AppendLine("	Int16 p_ignore = " + stream.ReadInt16()); /* 78 */
			str.AppendLine("	Int16 p_ignore_perc = " + stream.ReadInt16()); /* 80 */
			str.AppendLine("	Int16 m_pierce = " + stream.ReadInt16()); /* 82 */
			str.AppendLine("	Int16 m_pierce_perc = " + stream.ReadInt16()); /* 84 */
			str.AppendLine("	Int16 p_pierce = " + stream.ReadInt16()); /* 86 */
			str.AppendLine("	Int16 p_pierce_perc = " + stream.ReadInt16()); /* 88 */
			str.AppendLine("	Byte is_temporary = " + stream.ReadByte()); /* 90 */
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_CharList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 unknown = " + stream.ReadInt16());
			str.AppendLine("	Int32 unknown = " + stream.ReadInt32());
			short count = stream.ReadInt16();
			str.AppendLine("	Int16 char_count = " + count);	/* 6 */
			str.AppendLine("	struct character[char_count]");
			str.AppendLine("	{");
			for (int i = 0; i < count; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			Int32 sex = " + stream.ReadInt32());				/* 8 */
				str.AppendLine("			Int32 race = " + stream.ReadInt32());				/* 12 */
				str.AppendLine("			Int32 model00 = " + stream.ReadInt32());			/* 16 */
				str.AppendLine("			Int32 model01 = " + stream.ReadInt32());			/* 20 */
				str.AppendLine("			Int32 model02 = " + stream.ReadInt32());			/* 24 */
				str.AppendLine("			Int32 model03 = " + stream.ReadInt32());			/* 28 */
				str.AppendLine("			Int32 model04 = " + stream.ReadInt32());			/* 32 */
				str.AppendLine("			Int32 hair_color = " + stream.ReadInt32());		/* 36 */
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());		/* 40 */
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());		/* 44 */
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());		/* 48 */
				str.AppendLine("			Int32 right_hand = " + stream.ReadInt32());		/* 52 */
				str.AppendLine("			Int32 left_hand = " + stream.ReadInt32());		/* 54 */
				str.AppendLine("			Int32 armor_id = " + stream.ReadInt32());			/* 58 */
				str.AppendLine("			Int32 cap_id = " + stream.ReadInt32());			/* 62 */
				str.AppendLine("			Int32 hand_id = " + stream.ReadInt32()); 			/* 66 */
				str.AppendLine("			Int32 feet_id = " + stream.ReadInt32());			/* 70 */
				str.AppendLine("			Int32 belt_id = " + stream.ReadInt32());			/* 74 */
				str.AppendLine("			Int32 cape_id = " + stream.ReadInt32());			/* 78 */
				str.AppendLine("			Int32 necklace_id = " + stream.ReadInt32());		/* 82 */
				str.AppendLine("			Int32 ring1 = " + stream.ReadInt32());			/* 86 */
				str.AppendLine("			Int32 ring2 = " + stream.ReadInt32());			/* 90 */
				str.AppendLine("			Int32 earring = " + stream.ReadInt32());			/* 94 */
				str.AppendLine("			Int32 mask_id = " + stream.ReadInt32());			/* 98 */
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());			/* 102 */
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());			/* 106 */
				str.AppendLine("			Int32 deco_shield_id = " + stream.ReadInt32());	/* 110 */
				str.AppendLine("			Int32 deco_costume_id = " + stream.ReadInt32());	/* 114 */
				str.AppendLine("			Int32 deco_head_id = " + stream.ReadInt32());		/* 118 */
				str.AppendLine("			Int32 deco_gloves_id = " + stream.ReadInt32());	/* 122 */
				str.AppendLine("			Int32 deco_shoes_id = " + stream.ReadInt32());	/* 126 */
				str.AppendLine("			Int32 deco_cloak_id = " + stream.ReadInt32());	/* 130 */
				str.AppendLine("			Int32 deco_bag_id = " + stream.ReadInt32());		/* 134 */
				str.AppendLine("			Int32 mount_id = " + stream.ReadInt32());			/* 138 */
				str.AppendLine("			Int32 bag_id = " + stream.ReadInt32());			/* 142 */
				str.AppendLine("			Int32 level = " + stream.ReadInt32());			/* 146 */
				str.AppendLine("			Int32 job = " + stream.ReadInt32());				/* 150 */
				str.AppendLine("			Int32 job_level = " + stream.ReadInt32());		/* 154 */
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());
				str.AppendLine("			Byte unknown = " + stream.ReadByte());
				str.AppendLine("			String(19) char_name = " + stream.ReadString(0, 19));	/* 175 */
				str.AppendLine("			Int32 skin_color = " + stream.ReadInt32());		/* 194 */
				str.AppendLine("			String(30) create_date = " + stream.ReadString(0, 30));	/* 198 */
				str.AppendLine("			String(30) delete_date = " + stream.ReadString(0, 30));
				for (int j = 0; j < 312; j += 4)
					str.AppendLine("			Int32 unknown = " + stream.ReadInt32());
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_Dialog(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 dialog_type = " + stream.ReadInt32());
			str.AppendLine("	UInt32 npc_handle = " + stream.ReadUInt32());
			short titleLen = stream.ReadInt16();
			short textLen = stream.ReadInt16();
			short menuLen = stream.ReadInt16();
			str.AppendLine("	Int16 TitleLen = " + titleLen);
			str.AppendLine("	Int16 TextLen = " + textLen);
			str.AppendLine("	Int16 MenuLen = " + menuLen);
			str.AppendLine("	String(TitleLen) Title = " + stream.ReadString(0, titleLen));
			str.AppendLine("	String(TextLen) Text = " + stream.ReadString(0, textLen));
			str.AppendLine("	struct menu_option[MenuLen]");
			str.AppendLine("	{");
			str.AppendLine("		{");
			// TODO
			str.AppendLine("		}");
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
}

		internal static void send_UrlList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			short size = stream.ReadInt16();
			str.AppendLine("	Int16 string_length = " + size);
			str.AppendLine("	String(string_length) url_list = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_MaxHPMPUpdate(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	4B unknown = " + stream.ReadInt32());
			str.AppendLine("	Int32 old_hp = " + stream.ReadInt32());
			str.AppendLine("	Int32 new_hp = " + stream.ReadInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadInt32());
			str.AppendLine("	Int32 old_mp = " + stream.ReadInt32());
			str.AppendLine("	Int32 new_mp = " + stream.ReadInt32());
			str.AppendLine("	Byte unknown = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_EntityState(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32()); /* 0 */
			str.AppendLine("	UInt32 state = " + stream.ReadUInt32()); /* 4 */
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_SkillList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			short count = stream.ReadInt16();
			str.AppendLine("	Int16 count = " + count);
			str.AppendLine("	Byte unknown = " + stream.ReadByte());
			str.AppendLine("	struct skill_info[count]");
			str.AppendLine("	{");
			for (int i = 0; i < count; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			Int32 skill_id = " + stream.ReadInt32());
				str.AppendLine("			Byte skill_lv = " + stream.ReadByte());
				str.AppendLine("			Byte skill_lv = " + stream.ReadByte());
				str.AppendLine("			UInt32 cooldown = " + stream.ReadInt32());
				str.AppendLine("			UInt32 unknown = " + stream.ReadInt32());
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_TIMESYNC(ref PacketStream stream)
		{
            //struct TS_TIMESYNC : public TS_MESSAGE
            //{
	        // uint32_t time;
            //        static const int packetID = 2;
            //    };
    StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			//str.AppendLine("	2B unknown = " + unk1);
			str.AppendLine("}");

			if (Src == 'S')
				Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream);
			else
				Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void TS_CS_VERSION(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(20) Version = " + stream.ReadString(0, 20));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void parse_1F7(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void parse_226(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void parse_384(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(20) Version = " + stream.ReadString(0, 20));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void parse_44C(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void parse_SystemSpecs(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(86) Specs = " + stream.ReadString(0, 86));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void parse_270F(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(20) Version = " + stream.ReadString(0, 20));
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream);
		}

		internal static void TS_SC_LOGIN_RESULT(ref PacketStream stream)
		{
            //#define TS_SC_LOGIN_RESULT_DEF(_) \
            //            _(simple)(def)(uint16_t, result) \
            //	_(simple)(impl)(uint16_t, result, version >= EPIC_7_1) \
            //	_(simple)(impl)(uint8_t, result, version < EPIC_7_1) \
            //	_(simple)(uint32_t, handle) \
            //	_(simple)(float, x) \
            //	_(simple)(float, y) \
            //	_(simple)(float, z) \
            //	_(simple)(uint8_t, layer) \
            //	_(simple)(float, face_direction) \
            //	_(simple)(int32_t, region_size) \
            //	_(simple)(int32_t, hp) \
            //	_(simple)(def)(int32_t, mp) \
            //	_(simple)(impl)(int32_t, mp, version >= EPIC_7_1) \
            //	_(simple)(impl)(int16_t, mp, version < EPIC_7_1) \
            //	_(simple)(int32_t, max_hp) \
            //	_(simple)(def)(int32_t, max_mp) \
            //	_(simple)(impl)(int32_t, max_mp, version >= EPIC_7_1) \
            //	_(simple)(impl)(int16_t, max_mp, version < EPIC_7_1) \
            //	_(simple)(int32_t, havoc, version >= EPIC_4_1 && version < EPIC_9_2) \
            //	_(simple)(int32_t, max_havoc, version >= EPIC_4_1 && version < EPIC_9_2) \
            //	_(simple)(int32_t, sex) \
            //	_(simple)(int32_t, race) \
            //	_(simple)(uint32_t, skin_color, version >= EPIC_4_1) \
            //	_(simple)(int32_t, faceId) \
            //	_(simple)(int32_t, hairId) \
            //	_(string)(name, 19) \
            //	_(simple)(int32_t, cell_size) \
            //	_(simple)(int32_t, guild_id, version >= EPIC_5_1) \
            //	_(simple)(int32_t, unknown, version >= EPIC_9_2)

            //#define TS_SC_LOGIN_RESULT_ID(X) \
            //            X(4, version < EPIC_9_2) \
            //	X(64, version >= EPIC_9_2)


            StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 result = " + stream.ReadInt16());
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	float x = " + stream.ReadFloat());
			str.AppendLine("	float y = " + stream.ReadFloat());
			str.AppendLine("	float z = " + stream.ReadFloat());
			str.AppendLine("	Byte layer = " + stream.ReadByte());
			str.AppendLine("	float face = " + stream.ReadFloat());
			str.AppendLine("	Int32 region_size = " + stream.ReadInt32());
			str.AppendLine("	Int32 hp = " + stream.ReadInt32());
			str.AppendLine("	Int32 mp = " + stream.ReadInt32());
			str.AppendLine("	Int32 max_hp = " + stream.ReadInt32());
			str.AppendLine("	Int32 max_mp = " + stream.ReadInt32());
			str.AppendLine("	Int32 havoc = " + stream.ReadInt32());
			str.AppendLine("	Int32 max_havoc = " + stream.ReadInt32());
			str.AppendLine("	Int32 sex = " + stream.ReadInt32());
			str.AppendLine("	Int32 race = " + stream.ReadInt32());
			str.AppendLine("	Int32 skin_color = " + stream.ReadInt32());
			str.AppendLine("	Int32 face_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 hair_id = " + stream.ReadInt32());
			str.AppendLine("	string(19) char_name = " + stream.ReadString(0, 19));
			str.AppendLine("	Int32 cell_size = " + stream.ReadInt32());
			str.AppendLine("	Int32 guild_id = " + stream.ReadInt32());

			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_SET_TIME(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 gap = " + stream.ReadUInt32());

			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_CHAT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(21) msg_type = " + stream.ReadString(0, 21));
			short size = stream.ReadInt16();
			str.AppendLine("	Int16 size = " + size);
			str.AppendLine("	String(size) = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_GAME_GUARD_AUTH_QUERY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");

			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		
		}

		internal static void send_CharView(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 player_handle = " + stream.ReadInt32());
			str.AppendLine("	Int32 weapon_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 shield_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 armor_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 cap_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 hand_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 feet_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 belt_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 cape_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 necklace_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 ring1 = " + stream.ReadInt32());
			str.AppendLine("	Int32 ring2 = " + stream.ReadInt32());
			str.AppendLine("	Int32 earring = " + stream.ReadInt32());
			str.AppendLine("	Int32 mask_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 unknown = " + stream.ReadInt32());
			str.AppendLine("	Int32 unknown = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_shield_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_costume_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_head_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_gloves_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_shoes_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_cloak_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 deco_bag_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 mount_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 bag_id = " + stream.ReadInt32());
			for (int i = 0; i < 312; i += 4)
			{
				str.Append("	4B Unknown = " + stream.ReadInt32());
			}
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_InventoryList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			short count = stream.ReadInt16();
			str.AppendLine("	Int16 count = " + count);
			str.AppendLine("	struct Item[count]");
			str.AppendLine("	{");
			for (int i = 0; i < count; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			UInt32 item_handle = " + stream.ReadUInt32());
				str.AppendLine("			Int32 item_id = " + stream.ReadInt32());
				str.AppendLine("			Int64 item_uid = " + stream.ReadInt64());
				str.AppendLine("			Int64 count = " + stream.ReadInt64());
				str.AppendLine("			4B unknown = " + stream.ReadInt32());
				str.AppendLine("			4B unknown = " + stream.ReadInt32());
				str.AppendLine("			Byte enhance = " + stream.ReadByte());
				str.AppendLine("			Byte level = " + stream.ReadByte());
				str.AppendLine("			1B unknown = " + stream.ReadByte());
				for (int j = 0; j < 80; j+=4)
					str.AppendLine("			4B unknown = " + stream.ReadInt32());
				str.AppendLine("			Int32 wear_info = " + stream.ReadInt32());
				str.AppendLine("			Int16 unknown // 0 = " + stream.ReadInt16());
				str.AppendLine("			Int32 idx = " + stream.ReadInt32());
				str.AppendLine("		}");
			}

			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_BeltSlotInfo(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 slot0_handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 slot1_handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 slot2_handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 slot3_handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 slot4_handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 slot5_handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		
		}

		internal static void send_194(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	2B unknown = " + stream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		
		}

		internal static void send_QuestList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			int count = stream.ReadInt32();
			str.AppendLine("	Int32 count = " + count);
			str.AppendLine("	struct Quest[count]");
			str.AppendLine("	{");
			for (int j = 0; j < count; j++)
			{
				str.AppendLine("		{");
				str.AppendLine("			Int32 quest_id = " + stream.ReadInt32());
				str.AppendLine("			Int32 status1 = " + stream.ReadInt32());
				str.AppendLine("			Int32 status2 = " + stream.ReadInt32());
				str.AppendLine("			Int32 status3 = " + stream.ReadInt32());
				str.AppendLine("			Int32 status4 = " + stream.ReadInt32());
				str.AppendLine("			Int32 status5 = " + stream.ReadInt32());
				str.AppendLine("			Int32 status6 = " + stream.ReadInt32());
				str.Append("			23B unknown = ");
				for (int i = 0; i < 23; i++)
					str.Append(stream.ReadByte() + " ");
				str.Append("\r\n");
				str.AppendLine("			Byte progress = " + stream.ReadByte());
				str.AppendLine("			Int32 unknown = " + stream.ReadInt32());
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_QuestUpdate(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 quest_id = " + stream.ReadInt32());
			str.AppendLine("	Int32 status1 = " + stream.ReadInt32());
			str.AppendLine("	Int32 status2 = " + stream.ReadInt32());
			str.AppendLine("	Int32 status3 = " + stream.ReadInt32());
			str.AppendLine("	Int32 status4 = " + stream.ReadInt32());
			str.AppendLine("	Int32 status5 = " + stream.ReadInt32());
			str.AppendLine("	Int32 status6 = " + stream.ReadInt32());
			str.AppendLine("	Byte progress = " + stream.ReadByte());
			str.AppendLine("	Int32 unknown = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		
		}

		internal static void send_LocationInfo(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 old_location = " + stream.ReadInt32());
			str.AppendLine("	Int32 new_location = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_WeatherInfo(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 local_id = " + stream.ReadInt32());
			str.AppendLine("	Int16 Weather = " + stream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_UpdateGoldChaos(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int64 gold = " + stream.ReadInt64());
			str.AppendLine("	Int32 chaos = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_UpdateLevel(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	Int32 level = " + stream.ReadInt32());
			str.AppendLine("	Int32 job_level = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_UpdateExp(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	Int64 char_exp = " + stream.ReadInt64());
			str.AppendLine("	Int64 jp = " + stream.ReadInt64());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_GameTime(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int32 start_time = " + stream.ReadInt32());
			str.AppendLine("	Int64 unix_timestamp = " + stream.ReadInt64());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_OpenPopup(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.Append("	11B unknown = ");
			for (int i = 0; i < 11; i++)
				str.Append(stream.ReadByte() + " ");
			str.Append("\r\n");
			str.AppendLine("	String url = " + stream.ReadString(0, stream.GetSize() - 18));
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_Unamed191(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt16 skill_id = " + stream.ReadUInt16());
			str.AppendLine("	Byte level = " + stream.ReadByte());
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	Float x = " + stream.ReadFloat());
			str.AppendLine("	Float y = " + stream.ReadFloat());
			str.AppendLine("	Float z = " + stream.ReadFloat());
			str.AppendLine("	Byte layer = " + stream.ReadByte());
			str.AppendLine("	---Unknown Data---");
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_CS_ATTACK_REQUEST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint source_handle = " + stream.ReadUInt32());
			str.AppendLine("	uint item_handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_ATTACK_EVENT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint src_handle = " + stream.ReadUInt32());
			str.AppendLine("	uint dst_handle = " + stream.ReadUInt32());
			str.AppendLine("	ushort attack_speed = " + stream.ReadInt16());
			str.AppendLine("	ushort attack_delay = " + stream.ReadInt16());
			str.AppendLine("	byte attack_action = " + stream.ReadByte());
			str.AppendLine("	byte attack_flag = " + stream.ReadByte());
			int count = stream.ReadByte();
			str.AppendLine("	byte count = " + count);
			for (int i = 0; i < count; i++)
			{
				str.AppendLine("	uint hp_damage = " + stream.ReadUInt32());
				str.AppendLine("	uint mp_damage = " + stream.ReadUInt32());
				str.AppendLine("	byte flag = " + stream.ReadByte());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint new_dst_hp = " + stream.ReadUInt32());
				str.AppendLine("	uint new_dst_mp = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint unknown = " + stream.ReadUInt32());
				str.AppendLine("	uint new_src_hp = " + stream.ReadUInt32());
				str.AppendLine("	uint new_src_mp = " + stream.ReadUInt32());
			}
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void send_ItemDrop(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint source_handle = " + stream.ReadUInt32());
			str.AppendLine("	uint item_handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}

		internal static void TS_SC_CHAT_LOCAL(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(stream.GetId()) + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint src_handle = " + stream.ReadUInt32());
			short size = stream.ReadInt16();
			str.AppendLine("	short size = " + size);
			str.AppendLine("	String(size) message = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketRecv('G', GetPacketName(stream.GetId()), stream, str.ToString());
		}
	}

	/// <summary>
	/// Creates an encrypted value
	/// of an integer
	/// </summary>
	public class EncryptedInt
	{
		public class HiLo
		{
			public short h;
			public short l;
		}

		public EncryptedInt(int n)
		{
			short r1 = 0;// (short)Globals.GetRandomInt32();
			short r2 = 0;// (short)Globals.GetRandomInt32();

			m_H.h = r1;
			m_H.l = (short)(ByteUtils.HiWord(n) + (2 * (r2 - r1)));
			m_L.h = r2;
			m_L.l = (short)(ByteUtils.LoWord(n) - (2 * (r2 + r1)));
		}

		public HiLo m_H = new HiLo();
		public HiLo m_L = new HiLo();

		public void WriteToPacket(PacketStream packet)
		{
			packet.WriteInt16(m_H.h);
			packet.WriteInt16(m_H.l);
			packet.WriteInt16(m_L.h);
			packet.WriteInt16(m_L.l);
		}

		public static int Revert(long value)
		{
			byte[] v = BitConverter.GetBytes(value);
			short hh = BitConverter.ToInt16(v, 0); // r1
			short hl = BitConverter.ToInt16(v, 2);
			short lh = BitConverter.ToInt16(v, 4); // r2
			short ll = BitConverter.ToInt16(v, 6);

			//hl = (short)(ByteUtils.HiWord(n) + (2 * (r2 - r1)));
			//m_L.l = (short)(ByteUtils.LoWord(n) - (2 * (r2 + r1)));

			//val1 = (short)(HiWord(n) + (2 * (r2 - r1)));
			short HighWord = (short)(hl - (2 * (lh - hh)));

			//val2 = (short)(LoWord(n) - (2 * (r2 + r1)));
			short LowWord = (short)(ll + (2 * (lh + hh)));

			return LowWord;
		}
	}

	public class ScrambleMap
	{
		public ScrambleMap()
		{
			int v3;
			int i;
			byte v5;

			for (i = 0; i < 32; ++i)
			{
				map[i] = (byte)i;
			}

			v3 = 3;
			for (i = 0; i < 32; ++i)
			{
				v5 = map[i];
				if (v3 >= 32)
					v3 += -32 * (v3 >> 5);
				map[i] = map[v3];
				map[v3] = v5;
				v3 += i + 3;
			}
			for (i = 0; i < 32; ++i)
			{
				dmap[map[i]] = (byte)i;
			}
		}

		private byte[] map = new byte[32];
		private byte[] dmap = new byte[32];

		private static ScrambleMap scram_map = new ScrambleMap();

		public static int bits_scramble(int c)
		{
			int result;
			uint v2;

			result = 0;
			v2 = 0;
			do
			{
				if ((((uint)1 << (int)v2) & c) != 0)
					result |= 1 << scram_map.map[v2];
				++v2;
			}
			while (v2 < 32);
			return result;
		}

		public static int bits_descramble(int c)
		{
			int result;
			uint v2;

			result = 0;
			v2 = 0;
			do
			{
				if ((((uint)1 << (int)v2) & c) != 0)
					result |= 1 << scram_map.dmap[v2];
				++v2;
			}
			while (v2 < 32);
			return result;
		}
	}
}
