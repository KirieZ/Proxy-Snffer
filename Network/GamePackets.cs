using System;
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
		public static Dictionary<short, Packets.Packet> packet_db;
		private static char Src = ' ';

		static GamePackets()
		{
			packet_db = Packets.LoadGamePackets();
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

			if (packet_db.ContainsKey(PacketId))
				packet_db[PacketId].func(ref stream);
			else
			{
				if (src == 'S')
					Form1.PacketRecv('G', "Unknown", stream);
				else
					Form1.PacketSend('G', "Unknown", stream);
			}

			return stream;
		}

		internal static void TS_LOGIN(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_LOGIN [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	char name[19] = " + stream.ReadString(0, 19));
            str.AppendLine("    byte race = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_LOGIN", stream, str.ToString());
		}

		internal static void TS_CS_MOVE_REQUEST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_MOVE_REQUEST [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadUInt32());
			str.AppendLine("	float x = " + stream.ReadFloat());
			str.AppendLine("	float y = " + stream.ReadFloat());
			str.AppendLine("    uint cur_time = " + stream.ReadUInt32());
			str.AppendLine("	bool speed_sync = " + stream.ReadBool());
			ushort c = stream.ReadUInt16();
			str.AppendLine("	uint count = " + c);
			str.AppendLine("	struct points[count]");
			str.AppendLine("	{");
			for (int i = 0; i < c; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			float tx = " + stream.ReadFloat());
				str.AppendLine("			float ty = " + stream.ReadFloat());
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_MOVE_REQUEST", stream, str.ToString());
		}

		internal static void TS_REGION_UPDATE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_REGION_UPDATE [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint update_time = " + stream.ReadInt32());
			str.AppendLine("	float x = " + stream.ReadFloat());
			str.AppendLine("	float y = " + stream.ReadFloat());
			str.AppendLine("	float z = " + stream.ReadFloat());
			str.AppendLine("	bool bIsStopMessage = " + stream.ReadBool());
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_REGION_UPDATE", stream, str.ToString());
		}

        internal static void TS_CS_CHAT_REQUEST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_CHAT_REQUEST [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	char target[21] = " + stream.ReadString(0, 21));
			str.AppendLine("	byte request_id = " + stream.ReadByte());
			byte size = stream.ReadByte();
			str.AppendLine("	byte len = " + size);
            ChatType type = (ChatType)stream.ReadByte();
			str.AppendLine("	byte type = " + (byte) type + " (" + type.ToString() + ")");
			str.AppendLine("	char message[len] = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_CHAT_REQUEST", stream, str.ToString());
		}

        internal static void TS_LEAVE(ref PacketStream stream)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("struct TS_LEAVE [" + stream.GetId() + "]");
            stream.ReadByte();

            str.AppendLine("{");
            str.AppendLine("	uint handle = " + stream.ReadUInt32());
            str.AppendLine("}");

            Form1.PacketSend('G', "TS_CS_VERSION", stream, str.ToString());
        }

        internal static void TS_CS_VERSION(ref PacketStream stream)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("struct TS_CS_VERSION [" + stream.GetId() + "]");
            stream.ReadByte();

            str.AppendLine("{");
            str.AppendLine("	char version[20] = " + stream.ReadString(0, 20));
            str.AppendLine("}");

            Form1.PacketSend('G', "TS_CS_VERSION", stream, str.ToString());
        }

        internal static void TS_CS_RETURN_LOBBY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_RETURN_LOBBY [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_RETURN_LOBBY", stream, str.ToString());
		}

		internal static void TS_CS_REQUEST_RETURN_LOBBY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_REQUEST_RETURN_LOBBY [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_REQUEST_RETURN_LOBBY", stream, str.ToString());
		}

		internal static void TS_CS_REQUEST_LOGOUT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_REQUEST_LOGOUT [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_REQUEST_LOGOUT", stream, str.ToString());
		}

		internal static void TS_CS_LOGOUT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_LOGOUT [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_LOGOUT", stream, str.ToString());
		}

		internal static void parse_Equip(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 ItemHandle = " + stream.ReadUInt32());
			str.AppendLine("	4B Unknown = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_Unequip(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int WearType = " + stream.ReadInt32());
			str.AppendLine("	Byte Unknown = " + stream.ReadByte());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_WearChange(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_SetProperty(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(16) PropertyName = " + stream.ReadString(0, 16));
			str.AppendLine("	String(?S)  = " + stream.ReadString(0, stream.GetSize() - 23));
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

        internal static void TS_SC_REQUEST_SECURITY_NO(ref PacketStream stream)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("struct TS_SC_REQUEST_SECURITY_NO [" + stream.GetId() + "]");
            stream.ReadByte();

            str.AppendLine("{");
            str.AppendLine("	int mode = " + stream.ReadInt32());
            str.AppendLine("}");

            Form1.PacketSend('G', "TS_SC_REQUEST_SECURITY_NO", stream, str.ToString());
        }

        internal static void TS_CS_SECURITY_NO(ref PacketStream stream)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("struct TS_CS_SECURITY_NO [" + stream.GetId() + "]");
            stream.ReadByte();

            str.AppendLine("{");
            str.AppendLine("	int mode = " + stream.ReadInt32());
            str.AppendLine("    char security_no[19] = " + stream.ReadString(0, 19));
            str.AppendLine("}");

            Form1.PacketSend('G', "TS_CS_SECURITY_NO", stream, str.ToString());
        }

        internal static void parse_Target(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint target = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void TS_CS_CHARACTER_LIST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_CHARACTER_LIST [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	char account[61] = " + stream.ReadString(0, 61));
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_CHARACTER_LIST", stream, str.ToString());
		}

		internal static void TS_CS_CREATE_CHARACTER(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_CREATE_CHARACTER [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
            str.AppendLine("    LOBBY_CHARACTER_INFO info = {");
            str.AppendLine("        ModelInfo model_info = {");
			str.AppendLine("	        int sex = " + stream.ReadInt32());
			str.AppendLine("	        int race = " + stream.ReadInt32());
            str.AppendLine("	        int model_id[5] = {");
            str.AppendLine("                " + stream.ReadInt32() +", " + stream.ReadInt32() + ", "+ stream.ReadInt32() +", " + stream.ReadInt32() + ", " + stream.ReadInt32() + ", " + stream.ReadInt32());
            str.AppendLine("            };");
			str.AppendLine("	        int texture_id = " + stream.ReadInt32());
            str.AppendLine("	        int wear_info[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.Append("\r\n	            ");
                str.Append(stream.ReadInt32() + " ");
            }
            str.AppendLine("            };");
            str.AppendLine("        };");
			str.AppendLine("	    int level = " + stream.ReadInt32());
            str.AppendLine("	    int job = " + stream.ReadInt32());
			str.AppendLine("	    int job_level = " + stream.ReadInt32());
			str.AppendLine("	    int exp_percentage = " + stream.ReadInt32());
            str.AppendLine("	    int hp = " + stream.ReadInt32());
			str.AppendLine("	    int mp = " + stream.ReadInt32());
			str.AppendLine("	    int permission = " + stream.ReadInt32());
			str.AppendLine("        bool is_banned = " + stream.ReadBool());
			str.AppendLine("    	char name[19] = " + stream.ReadString(0, 19));

            str.AppendLine("	    uint skin_color = " + stream.ReadUInt32());
            str.AppendLine("	    char szCreateTime[30] = " + stream.ReadString(0, 30));
            str.AppendLine("	    char szDeleteTime[30] = " + stream.ReadString(0, 30));
            str.AppendLine("	    int wear_item_enhance_info[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.Append("\r\n	        ");
                str.Append(stream.ReadInt32() + " ");
            }
            str.AppendLine("        };");
            str.AppendLine("	    int wear_item_level_info[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.Append("\r\n	        ");
                str.Append(stream.ReadInt32() + " ");
            }
            str.AppendLine("        };");
            str.AppendLine("	    byte wear_item_elemental_type[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.Append("\r\n	        ");
                str.Append(stream.ReadByte() + " ");
            }
            str.AppendLine("        };");
            str.AppendLine("    };");
            str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_CREATE_CHARACTER", stream, str.ToString());
		}

		internal static void TS_CS_DELETE_CHARACTER(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_DELETE_CHARACTER [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	char name[19] = " + stream.ReadString(0, 19));
            str.AppendLine("	char security_no[19] = " + stream.ReadString(0, 19));
            str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_DELETE_CHARACTER", stream, str.ToString());
		}

		internal static void TS_CS_ACCOUNT_WITH_AUTH(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_ACCOUNT_WITH_AUTH [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	char account[61] = " + stream.ReadString(0, 61));
			str.AppendLine("	long one_time_key = " + stream.ReadInt64());
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_ACCOUNT_WITH_AUTH", stream, str.ToString());
		}

		internal static void TS_CS_CHECK_CHARACTER_NAME(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_CHECK_CHARACTER_NAME [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(19) name = " + stream.ReadString(0, 19));
			str.AppendLine("}");

			Form1.PacketSend('G', "TS_CS_CHECK_CHARACTER_NAME", stream, str.ToString());
		}

		internal static void parse_DialOpt(ref PacketStream stream)
		{
			short size = stream.ReadInt16();
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 size = " + size);
			str.AppendLine("	String(size) Function = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_Contact(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int NpcHandle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_LearnSkill(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	Int32 skill_id = " + stream.ReadInt32());
			str.AppendLine("	Int16 target_level = " + stream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}
		
		internal static void parse_JobLevelUp(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_UseSkill(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		//========== Send

		internal static void TS_SC_RESULT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_RESULT [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	ushort request_msg_id = " + stream.ReadInt16());
			str.AppendLine("	ushort result = " + stream.ReadInt16());
			str.AppendLine("	int value = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_RESULT", stream, str.ToString());
		}

        internal static void TS_SC_ENTER(ref PacketStream stream)
		{
            StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_ENTER [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
            ArObject__ObjectType type = (ArObject__ObjectType) stream.ReadByte();
			str.AppendLine("	byte type = " + (byte) type + " (" + type.ToString() + ")");
			str.AppendLine("	uint handle = " + stream.ReadUInt32());
			str.AppendLine("	float x = " + stream.ReadFloat());
			str.AppendLine("	float y = " + stream.ReadFloat());
			str.AppendLine("	float z = " + stream.ReadFloat());
			str.AppendLine("    byte layer = " + stream.ReadByte());
			TS_SC_ENTER__OBJ_TYPE objType = (TS_SC_ENTER__OBJ_TYPE) stream.ReadByte();
			str.AppendLine("	byte objType = " + (byte) objType +" ("+ objType.ToString() +")");
			str.AppendLine("	");

            if (objType == TS_SC_ENTER__OBJ_TYPE.EOT_Monster ||
                objType == TS_SC_ENTER__OBJ_TYPE.EOT_NPC ||
                objType == TS_SC_ENTER__OBJ_TYPE.EOT_Pet ||
                objType == TS_SC_ENTER__OBJ_TYPE.EOT_Player ||
                objType == TS_SC_ENTER__OBJ_TYPE.EOT_Summon
                )
            { // Creatures
                str.AppendLine("    // Creatures");
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
            }

            switch (objType)
            {
                case TS_SC_ENTER__OBJ_TYPE.EOT_Player:
                    {
                        str.AppendLine("    // Player");
                        long encId = stream.ReadInt64();
                        str.AppendLine("	byte sex = " + stream.ReadByte());
                        // Does epics later than 6.2 began using uint?
                        str.AppendLine("	int faceId = " + stream.ReadInt32());
                        str.AppendLine("	int faceTextureId = " + stream.ReadInt32());
                        str.AppendLine("	int hairId = " + stream.ReadInt32());
                        // TODO : These are not on EPIC 6.2
                        //str.AppendLine("	uint hairColorIndex = " + stream.ReadUInt32());
                        //str.AppendLine("	uint hairColorRGB = " + stream.ReadUInt32());
                        //str.AppendLine("	uint hideEquipFlag = " + stream.ReadUInt32());
                        str.AppendLine("	char name[19] = " + stream.ReadString(0, 19));
                        str.AppendLine("	ushort job_id = " + stream.ReadUInt16());
                        str.AppendLine("	uint ride_handle = " + stream.ReadUInt32());
                        str.AppendLine("	int guild_id = " + stream.ReadInt32());
                        // TOOD :EPIC 9.2+
                        //str.AppendLine("	uint64 unknown = " + stream.ReadUInt64());
                    }
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_NPC:
                    {
                        str.AppendLine("    // NPC");
                        long encId = stream.ReadInt64();
                        str.AppendLine("	Int64 npc_id = (" + encId + ") " + EncryptedInt.Revert(encId) + " // Encrypted");
                    }
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_Monster:
                    {
                        str.AppendLine("    // Monster");
                        long encId = stream.ReadInt64();
                        str.AppendLine("	Int64 monster_id = (" + encId + ") " + EncryptedInt.Revert(encId) + " // Encrypted");
                        str.AppendLine("	bool is_tamed = " + stream.ReadBool());
                    }
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_Item:
                    {
                        str.AppendLine("    // ItemInfo");
                        long encId = stream.ReadInt64();
                        str.AppendLine("	Int64 code = (" + encId + ") " + EncryptedInt.Revert(encId) + " // Encrypted");
                        str.AppendLine("	Int64 count = " + stream.ReadInt64());
                        str.AppendLine("    // ITEM_PICK_UP_ORDER");
                        str.AppendLine("	uint drop_time = " + stream.ReadUInt32());
                        str.AppendLine("	uint player[3] = { ");
                        str.AppendLine("        " + stream.ReadUInt32() + ",");
                        str.AppendLine("        " + stream.ReadUInt32() + ",");
                        str.AppendLine("        " + stream.ReadUInt32() + "");
                        str.AppendLine("    };");
                        str.AppendLine("	int partyId[3] = { ");
                        str.AppendLine("        " + stream.ReadInt32() + ",");
                        str.AppendLine("        " + stream.ReadInt32() + ",");
                        str.AppendLine("        " + stream.ReadInt32() + "");
                        str.AppendLine("    };");
                    }
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_Summon:
                    {
                        str.AppendLine("    // Summon");
                        str.AppendLine("	uint master_handle = " + stream.ReadUInt32());
                        long encId = stream.ReadInt64();
                        str.AppendLine("	Int64 summon_code = (" + encId + ") " + EncryptedInt.Revert(encId));
                        str.AppendLine("	char name[19] = " + stream.ReadString(0, 19));
                        // TODO : This is not in epic 6.2
                        //str.AppendLine("	byte enhance = " + stream.ReadByte());
                    }
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_Skill:
                    {
                        str.AppendLine("    // Skill");
                        str.AppendLine("	uint caster = " + stream.ReadUInt32());
                        str.AppendLine("	uint start_time = " + stream.ReadUInt32());
                        // Does later epics uses uint there?
                        str.AppendLine("	int skill_id = " + stream.ReadInt32());
                    }
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_FieldProp:
                    {
                        str.AppendLine("    // FieldProp");
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
                    break;
                case TS_SC_ENTER__OBJ_TYPE.EOT_Pet:
                    {
                        str.AppendLine("    // Pet");
                        str.AppendLine("	uint master_handle = " + stream.ReadUInt32());
                        long encId = stream.ReadInt64();
                        str.AppendLine("	Int64 pet_id = (" + encId + ") " + EncryptedInt.Revert(encId) + " // Encrypted");
                        str.AppendLine("	char[19] name = " + stream.ReadString(0, 19));
                    }
                    break;
                default:
                    str.AppendLine("    UNKNOWN??"); // Should never happen
                    break;
            }

			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_ENTER", stream, str.ToString());
		}

		internal static void TS_SC_MOVE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_MOVE [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint start_time = " + stream.ReadUInt32());		/* 0 */
			str.AppendLine("	uint handle = " + stream.ReadUInt32());		/* 4 */
			str.AppendLine("	byte tlayer = " + stream.ReadByte());			/* 8 */
			str.AppendLine("	byte speed = " + stream.ReadByte());	/* 9 */
			ushort pcount = stream.ReadUInt16();
			str.AppendLine("	ushorrt count = " + pcount);	/* 10 */
			str.AppendLine("	struct points[count]");
			str.AppendLine("	{");
			for (int i = 0; i < pcount; i++)
			{
				str.AppendLine("		{");
				str.AppendLine("			float tx = " + stream.ReadFloat());	/* 12 */
				str.AppendLine("			float ty = " + stream.ReadFloat());	/* 16 */
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_MOVE", stream, str.ToString());
	}

		internal static void TS_REGION_ACK(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_REGION_ACK [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint rx = " + stream.ReadUInt32()); /* 0 */
			str.AppendLine("	uint ry = " + stream.ReadUInt32()); /* 4 */

			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_REGION_ACK", stream, str.ToString());
		}

		internal static void TS_SC_PROPERTY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_PROPERTY [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadUInt32());
			bool is_number = stream.ReadBool();
			str.AppendLine("	bool is_number = " + is_number);
			str.AppendLine("	char name[16] = " + stream.ReadString(0, 16));
			str.AppendLine("	long value = " + stream.ReadInt64());
			if (!is_number)
				str.AppendLine("	char value[PacketSize - 28] = " + stream.ReadString(0, stream.GetSize() - 28));
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_PROPERTY", stream, str.ToString());
		}

		internal static void send_UpdateHPMP(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 hp_recover = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 mp_recover = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 new_hp = " + stream.ReadUInt32());
			str.AppendLine("	UInt32 new_mp = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void TS_SC_STAT_INFO(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_STAT_INFO [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadUInt32()); /* 0 */
            str.AppendLine("    CreatureStat stat = {");
            str.AppendLine("        short stat_id = " + stream.ReadInt16());
            str.AppendLine("        short strength = " + stream.ReadInt16());
            str.AppendLine("        short vital = " + stream.ReadInt16());
            str.AppendLine("        short dexterity = " + stream.ReadInt16());
            str.AppendLine("        short agility = " + stream.ReadInt16());
            str.AppendLine("        short intelligence = " + stream.ReadInt16());
            str.AppendLine("        short mentality = " + stream.ReadInt16());
            str.AppendLine("        short luck = " + stream.ReadInt16());
            str.AppendLine("    }");
            str.AppendLine("    CreatureAttribute attribute = {");
            str.AppendLine("        short nCritical = " + stream.ReadInt16());
            str.AppendLine("        short nCriticalPower = " + stream.ReadInt16());
            str.AppendLine("        short nAttackPointRight = " + stream.ReadInt16());
            str.AppendLine("        short nAttackPointLeft = " + stream.ReadInt16());
            str.AppendLine("        short nDefence = " + stream.ReadInt16());
            str.AppendLine("        short nBlockDefence = " + stream.ReadInt16());
            str.AppendLine("        short nMagicPoint = " + stream.ReadInt16());
            str.AppendLine("        short nMagicDefence = " + stream.ReadInt16());
            str.AppendLine("        short nAccuracyRight = " + stream.ReadInt16());
            str.AppendLine("        short nAccuracyLeft = " + stream.ReadInt16());
            str.AppendLine("        short nMagicAccuracy = " + stream.ReadInt16());
            str.AppendLine("        short nAvoid = " + stream.ReadInt16());
            str.AppendLine("        short nMagicAvoid = " + stream.ReadInt16());
            str.AppendLine("        short nBlockChance = " + stream.ReadInt16());
            str.AppendLine("        short nMoveSpeed = " + stream.ReadInt16());
            str.AppendLine("        short nAttackSpeed = " + stream.ReadInt16());
            str.AppendLine("        short nAttackRange = " + stream.ReadInt16());
            str.AppendLine("        short nMaxWeight = " + stream.ReadInt16());
            str.AppendLine("        short nCastingSpeed = " + stream.ReadInt16());
            str.AppendLine("        short nCoolTimeSpeed = " + stream.ReadInt16());
            str.AppendLine("        short nItemChance = " + stream.ReadInt16());
            str.AppendLine("        short nHPRegenPercentage = " + stream.ReadInt16());
            str.AppendLine("        short nHPRegenPoint = " + stream.ReadInt16());
            str.AppendLine("        short nMPRegenPercentage = " + stream.ReadInt16());
            str.AppendLine("        short nMPRegenPoint = " + stream.ReadInt16());
            str.AppendLine("    }");
            str.AppendLine("    byte type = " + stream.ReadByte());
            str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_STAT_INFO", stream, str.ToString());
		}

		internal static void TS_SC_CHARACTER_LIST(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_CHARACTER_LIST [" + pStream.GetId() + "]");
			pStream.ReadByte();

			// Extends: 
			str.Append("{\r\n");
			str.Append("	uint current_server_time = ").Append(pStream.ReadUInt32()).Append("\r\n");
			str.Append("	ushort last_login_index = ").Append(pStream.ReadUInt16()).Append("\r\n");
			int count = pStream.ReadUInt16();
			str.Append("	ushort count = ").Append(count).Append("\r\n");
			str.Append("	struct LOBBY_CHARACTER_INFO[] char_list[count] = {\r\n");
			for (int i = 0; i < count; i++)
			{
				str.Append("	struct MODEL_INFO model_info = {\r\n");
				str.Append("		int sex = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("		int race = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("		int model_id[5] = {\r\n         ");
				for (int j = 0; j < 5; j++)
					str.Append(pStream.ReadInt32()).Append(",");
				str.Append("\r\n		}\r\n");
				str.Append("		int texture_id = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("		int wear_info[24] = {\r\n           ");
				for (int j = 0; j < 24; j++)
					str.Append(pStream.ReadInt32()).Append(",");
				str.Append("\r\n		}\r\n");
				str.Append("	}\r\n");
				str.Append("	int level = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	int job = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	int job_level = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	int exp_percentage = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	int hp = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	int mp = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	int permission = ").Append(pStream.ReadInt32()).Append("\r\n");
				str.Append("	bool is_banned = ").Append(pStream.ReadBool()).Append("\r\n");
				str.Append("	char name[19] = ").Append(pStream.ReadString(0, 19)).Append("\r\n");
				str.Append("	uint skin_color = ").Append(pStream.ReadUInt32()).Append("\r\n");
				str.Append("	char szCreateTime[30] = ").Append(pStream.ReadString(0, 30)).Append("\r\n");
				str.Append("	char szDeleteTime[30] = ").Append(pStream.ReadString(0, 30)).Append("\r\n");
				str.Append("	int wear_item_enhance_info[24] = {\r\n      ");
				for (int j = 0; j < 24; j++)
					str.Append(pStream.ReadInt32()).Append(",");
				str.Append("\r\n	}\r\n");
				str.Append("	int wear_item_level_info[24] = {\r\n        ");
				for (int j = 0; j < 24; j++)
					str.Append(pStream.ReadInt32()).Append(",");
				str.Append("\r\n	}\r\n");
				str.Append("	char wear_item_elemental_type[24] = {\r\n       ");
				for (int j = 0; j < 24; j++)
					str.Append(pStream.ReadByte()).Append(",");
				str.Append("\r\n	}\r\n");
			}
			str.Append("}\r\n");

			Form1.PacketRecv('G', "TS_SC_CHARACTER_LIST", pStream, str.ToString());
		}

		internal static void send_Dialog(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
}

		internal static void TS_SC_URL_LIST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_URL_LIST [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			ushort size = stream.ReadUInt16();
			str.AppendLine("	ushort url_list_len = " + size);
			str.AppendLine("    char url_list[url_list_len] = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_URL_LIST", stream, str.ToString());
		}

		internal static void send_MaxHPMPUpdate(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void TS_SC_STATUS_CHANGE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_STATUS_CHANGE [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadUInt32());
			str.AppendLine("	uint status = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_STATUS_CHANGE", stream, str.ToString());
		}

		internal static void send_SkillList(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void TS_TIMESYNC(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_TIMESYNC [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint time = " + stream.ReadUInt32());
			str.AppendLine("}");

			if (Src == 'S')
				Form1.PacketRecv('G', "TS_TIMESYNC", stream, str.ToString());
			else
				Form1.PacketSend('G', "TS_TIMESYNC", stream, str.ToString());
		}

		internal static void parse_ClientVersion(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(20) Version = " + stream.ReadString(0, 20));
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream);
		}

		internal static void parse_1F7(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream);
		}

		internal static void parse_226(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream);
		}

		internal static void TS_CS_CHANGE_LOCATION(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CS_CHANGE_LOCATION [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	float x = " + stream.ReadFloat());
            str.AppendLine("	float y = " + stream.ReadFloat());
            str.AppendLine("}");

        Form1.PacketSend('G', "TS_CS_CHANGE_LOCATION", stream);
		}

		internal static void parse_44C(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream);
		}

		internal static void parse_SystemSpecs(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(86) Specs = " + stream.ReadString(0, 86));
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream);
		}

		internal static void parse_270F(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(20) Version = " + stream.ReadString(0, 20));
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream);
		}

		internal static void TS_LOGIN_RESULT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_LOGIN_RESULT [" + stream.GetId() + "]");
			stream.ReadByte();

            // TODO : Structure differences
			str.AppendLine("{");
            
            // EPIC < 7.1
			str.AppendLine("	bool bIsAccepted = " + stream.ReadBool());
            // Epic >= 7.1
            //str.AppendLine("	ushort result = " + stream.ReadUInt16());

            str.AppendLine("	uint handle = " + stream.ReadUInt32());
			str.AppendLine("	float x = " + stream.ReadFloat());
			str.AppendLine("	float y = " + stream.ReadFloat());
			str.AppendLine("	float z = " + stream.ReadFloat());
			str.AppendLine("	byte layer = " + stream.ReadByte());
			str.AppendLine("	float face_direction = " + stream.ReadFloat());
			str.AppendLine("	int region_size = " + stream.ReadInt32());
			str.AppendLine("	int hp = " + stream.ReadInt32());
            
            // EPIC < 7.1
			str.AppendLine("	short mp = " + stream.ReadInt16());
            // EPIC >= 7.1
            //str.AppendLine("	int mp = " + stream.ReadInt32());

            str.AppendLine("	int max_hp = " + stream.ReadInt32());
            
            // EPIC < 7.1
			str.AppendLine("	short max_mp = " + stream.ReadInt16());
            // EPIC >= 7.1
            //str.AppendLine("	int max_mp = " + stream.ReadInt32());
            
            // EPIC >= 4.1 && < 9.2
            str.AppendLine("	int havoc = " + stream.ReadInt32());
            str.AppendLine("	int max_havoc = " + stream.ReadInt32());
            
			str.AppendLine("	int sex = " + stream.ReadInt32());
			str.AppendLine("	int race = " + stream.ReadInt32());
            
            // EPIC >= 4.1
			str.AppendLine("	uint skin_color = " + stream.ReadInt32());

            str.AppendLine("	int face_id = " + stream.ReadInt32());
			str.AppendLine("	int hair_id = " + stream.ReadInt32());
			str.AppendLine("	char name[19] = " + stream.ReadString(0, 19));
			str.AppendLine("	int cell_size = " + stream.ReadInt32());

            // Epic >= 5.1
			str.AppendLine("	int guild_id = " + stream.ReadInt32());

            // Epic >= 9.2
            //str.AppendLine("	int unknown = " + stream.ReadInt32());

            str.AppendLine("}");

			Form1.PacketRecv('G', "TS_LOGIN_RESULT", stream, str.ToString());
		}

		internal static void send_A(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 unknown = " + stream.ReadUInt32());

			str.AppendLine("}");

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void TS_SC_CHAT(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_CHAT [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	char sender[21] = " + stream.ReadString(0, 21));
			ushort size = stream.ReadUInt16();
			str.AppendLine("	ushort size = " + size);
            ChatType type = (ChatType)stream.ReadByte();
            str.AppendLine("	byte type = " + (byte) type + " (" + type.ToString() + ")");
            str.AppendLine("    char msg[size] = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_CHAT", stream, str.ToString());
		}

		internal static void send_37(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");

			str.AppendLine("}");

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		
		}

		internal static void TS_WEAR_INFO(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_WEAR_INFO [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadInt32());
            str.AppendLine("	int itemCode[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.AppendLine("\r\n        ");
                str.Append(stream.ReadInt32() + " ");
            }
            str.AppendLine("\r\n    }");
            str.AppendLine("	int itemEnhance[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.AppendLine("\r\n        ");
                str.Append(stream.ReadInt32() + " ");
            }
            str.AppendLine("\r\n    }");
            str.AppendLine("	int itemLevel[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.AppendLine("\r\n        ");
                str.Append(stream.ReadInt32() + " ");
            }
            str.AppendLine("\r\n    }");
            str.AppendLine("	byte elemental_effect_type[24] = {");
            for (int i = 0; i < 24; i++)
            {
                if (i % 8 == 0)
                    str.AppendLine("        ");
                str.Append(stream.ReadByte() + " ");
            }
            str.AppendLine("\r\n    }");
            str.AppendLine("}");

			Form1.PacketRecv('G', "TS_WEAR_INFO", stream, str.ToString());
		}

		internal static void TS_SC_INVENTORY(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_INVENTORY [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			short count = stream.ReadInt16();
			str.AppendLine("    ushort count = " + count);
			str.AppendLine("	TS_ITEM_INFO items[count]");
			str.AppendLine("	{");
			for (int i = 0; i < count; i++)
			{
				str.AppendLine("		{");
                str.AppendLine("            TS_ITEM_BASE_INFO base =");
                str.AppendLine("            {");
				str.AppendLine("			    uint handle = " + stream.ReadUInt32());
				str.AppendLine("			    int code = " + stream.ReadInt32());
				str.AppendLine("			    long uid = " + stream.ReadInt64());
				str.AppendLine("			    long count = " + stream.ReadInt64());
				str.AppendLine("			    int ethereal_durability = " + stream.ReadInt32());
				str.AppendLine("		        uint endurance = " + stream.ReadUInt32());
				str.AppendLine("			    byte enhance = " + stream.ReadByte());
				str.AppendLine("			    byte level = " + stream.ReadByte());
				str.AppendLine("			    int flag = " + stream.ReadInt32());
                str.AppendLine("                int socket[4] = {");
                str.Append("                    ");
                for (int j = 0; j < 4; j++)
					 str.Append(stream.ReadInt32() + "; ");
                str.AppendLine("}");
                str.AppendLine("			    int remain_time = " + stream.ReadInt32());
                str.AppendLine("			    byte elemental_effect_type; = " + stream.ReadByte());
                str.AppendLine("			    int elemental_effect_remain_time = " + stream.ReadInt32());
                str.AppendLine("			    int elemental_effect_attack_point = " + stream.ReadInt32());
                str.AppendLine("			    int elemental_effect_magic_point = " + stream.ReadInt32());
                str.AppendLine("            }");
                str.AppendLine("			short wear_position = " + stream.ReadInt16());
				str.AppendLine("			uint own_summon_handle = " + stream.ReadUInt32());
				str.AppendLine("			int idx = " + stream.ReadInt32());
				str.AppendLine("		}");
			}
            str.AppendLine("    }");
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_INVENTORY", stream, str.ToString());
		}

		internal static void TS_SC_BELT_SLOT_INFO(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_BELT_SLOT_INFO [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
            str.AppendLine("    uint handle[6] = {");
            str.Append("        ");
            for (int i = 0; i < 6; i++)
            {
                str.Append(stream.ReadInt32() + ", ");
            }
            str.AppendLine("");
            str.AppendLine("    };");
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_BELT_SLOT_INFO", stream, str.ToString());
		
		}

		internal static void send_194(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	UInt32 handle = " + stream.ReadUInt32());
			str.AppendLine("	2B unknown = " + stream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		
		}

		internal static void TS_SC_QUEST_LIST(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_QUEST_LIST [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			ushort count_active = stream.ReadUInt16();
            ushort count_pending = stream.ReadUInt16();
            str.AppendLine("	ushort count_active = " + count_active);
            str.AppendLine("    ushort count_pending = " + count_pending);
			str.AppendLine("	TS_QUEST_INFO actives[count_active]");
			str.AppendLine("	{");
			for (int j = 0; j < count_active; j++)
			{
				str.AppendLine("		{");
				str.AppendLine("		    int code = " + stream.ReadInt32());
				str.AppendLine("			int nStartID = " + stream.ReadInt32());
                str.AppendLine("            int nRandomValue[6] = {");
                str.Append("                ");
                for (int i = 0; i < 6; i++)
                {
                    str.Append(stream.ReadInt32() + " ");
                }
                str.Append("\r\n            }");
				str.AppendLine("			byte nProgress = " + stream.ReadByte());
				str.AppendLine("			uint nTimeLimit = " + stream.ReadUInt32());
				str.AppendLine("		}");
			}
			str.AppendLine("	}");
            str.AppendLine("	TS_PENDING_QUEST_INFO pendings[count_pending]");
            str.AppendLine("	{");
            for (int j = 0; j < count_pending; j++)
            {
                str.AppendLine("		{");
                str.AppendLine("		    int code = " + stream.ReadInt32());
                str.AppendLine("			int nStartID = " + stream.ReadInt32());
                str.AppendLine("		}");
            }
            str.AppendLine("	}");
            str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_QUEST_LIST", stream, str.ToString());
		}

		internal static void send_QuestUpdate(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		
		}

		internal static void TS_SC_CHANGE_LOCATION(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_CHANGE_LOCATION [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	int prev_location_id = " + stream.ReadInt32());
			str.AppendLine("	int cur_location_id = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_CHANGE_LOCATION", stream, str.ToString());
		}

		internal static void TS_SC_WEATHER_INFO(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_WEATHER_INFO [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("    uint region_id = " + stream.ReadUInt32());
			str.AppendLine("	ushort weather_id = " + stream.ReadUInt16());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_WEATHER_INFO", stream, str.ToString());
		}

		internal static void TS_SC_GOLD_UPDATE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_GOLD_UPDATE [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	long gold = " + stream.ReadInt64());
			str.AppendLine("	int chaos = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_GOLD_UPDATE", stream, str.ToString());
		}

		internal static void TS_SC_LEVEL_UPDATE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_LEVEL_UPDATE [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadUInt32());
			str.AppendLine("	int level = " + stream.ReadInt32());
			str.AppendLine("	int job_level = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_LEVEL_UPDATE", stream, str.ToString());
		}

		internal static void TS_SC_EXP_UPDATE(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_EXP_UPDATE [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint handle = " + stream.ReadUInt32());
			str.AppendLine("	long char_exp = " + stream.ReadInt64());
			str.AppendLine("	int jp = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_EXP_UPDATE", stream, str.ToString());
		}

		internal static void TS_SC_GAME_TIME(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_GAME_TIME [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	int t = " + stream.ReadInt32());
			str.AppendLine("	int game_time = " + stream.ReadInt32());
            str.AppendLine("    4B unknown = " + stream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_GAME_TIME", stream, str.ToString());
		}

		internal static void TS_SC_OPEN_URL(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_OPEN_URL [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
            str.AppendLine("    bool wait_for_event_scene = " + stream.ReadBool());
            str.AppendLine("    int width = " + stream.ReadInt32());
            str.AppendLine("    int height = " + stream.ReadInt32());
            ushort len = stream.ReadUInt16();
            str.AppendLine("    ushort url_len = " + len);
			str.AppendLine("	char url[len] = " + stream.ReadString(0, len));
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_OPEN_URL", stream, str.ToString());
		}

		internal static void send_Unamed191(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void parse_PCAttack(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint source_handle = " + stream.ReadUInt32());
			str.AppendLine("	uint item_handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketSend('G', "PacketName", stream, str.ToString());
		}

		internal static void send_Attack(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
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

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void send_ItemDrop(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + "PacketName" + " [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint source_handle = " + stream.ReadUInt32());
			str.AppendLine("	uint item_handle = " + stream.ReadUInt32());
			str.AppendLine("}");

			Form1.PacketRecv('G', "PacketName", stream, str.ToString());
		}

		internal static void TS_SC_CHAT_LOCAL(ref PacketStream stream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_SC_CHAT_LOCAL [" + stream.GetId() + "]");
			stream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	uint src_handle = " + stream.ReadUInt32());
			short size = stream.ReadInt16();
			str.AppendLine("	short size = " + size);
			str.AppendLine("	chat message[size] = " + stream.ReadString(0, size));
			str.AppendLine("}");

			Form1.PacketRecv('G', "TS_SC_CHAT_LOCAL", stream, str.ToString());
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
