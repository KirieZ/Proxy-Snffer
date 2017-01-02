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

		static AuthPackets()
		{
			packet_db = Packets.LoadAuthPackets();
		}

		internal static PacketStream PacketReceived(char src, PacketStream stream)
		{
			// Header
			// [Size:4]
			// [ID:2]
			// [Checksum:1]
			short PacketId = stream.GetId();
			stream.ReadByte();

            if (packet_db.ContainsKey(PacketId))
            {
                packet_db[PacketId].func(ref stream);
            }
            else
            {
                if (src == 'S')
                    Form1.PacketRecv('A', "Unknown", stream);
                else
                    Form1.PacketSend('A', "Unknown", stream);
            }

			return stream;
		}

        internal static void TS_AC_SERVER_LIST(ref PacketStream data)
        {
            StringBuilder str = new StringBuilder();

            str.Append("struct TS_AC_SERVER_LIST [").Append(data.GetId()).Append("]\r\n");
            data.ReadByte();

            str.Append("{\r\n");
            str.Append("	ushort last_login_server_id = ").Append(data.ReadUInt16()).Append(";\r\n");
            ushort svCount = data.ReadUInt16();
            str.Append("	ushort count = ").Append(svCount).Append(";\r\n");
            str.Append("	struct servers[Count]\r\n");
            str.Append("    {\r\n");
            for (int i = 0; i < svCount; i++)
            {
                str.Append("		{\r\n");
                str.Append("			ushort index = ").Append(data.ReadUInt16()).Append("\r\n");
                str.Append("			string(22) name = ").Append(data.ReadString(0, 22)).Append("\r\n");
                str.Append("			string(256) url = ").Append(data.ReadString(0, 256)).Append("\r\n");
                str.Append("			string(16) ip = ").Append(data.ReadString(0, 16)).Append("\r\n");
                str.Append("			int port = ").Append(data.ReadInt32()).Append("\r\n");
                str.Append("			ushort user_ratio = ").Append(data.ReadInt16()).Append("\r\n");
                str.Append("		}\r\n");
            }
            str.Append("    }\r\n");
            str.Append("}");

			data.RewriteUInt16(300, Config.Game_ClientPort);
            data.RewriteString(284, Config.Game_ClientIp, 16);

			Form1.PacketRecv('A', "TS_AC_SERVER_LIST", data, str.ToString());
        }

        #region Parse Packet

        internal static void TS_SC_RESULT(ref PacketStream pStream)
        {
            // enum LOGIN_SUCCESS_FLAG
            //        {
            //            LSF_EULA_ACCEPTED = 0x1,
            //            LSF_ACCOUNT_BLOCK_WARNING = 0x2,
            //            LSF_DISTRIBUTION_CODE_REQUIRED = 0x4
            //        };
            
            // Extends: TS_MESSAGE
            StringBuilder str = new StringBuilder();
            str.Append("struct TS_AC_RESULT [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.Append("{\r\n");
            str.Append("	ushort request_msg_id; = ").Append(pStream.ReadUInt16()).Append("\r\n");
            str.Append("	ushort result; = ").Append(pStream.ReadUInt16()).Append("\r\n");
            str.Append("	int login_flag; = ").Append(pStream.ReadInt32()).Append("\r\n");
            str.Append("	static const uint16_t packetID = 10000;").Append("\r\n");
            str.Append("}\r\n");
            str.Append("}\r\n");

            Form1.PacketRecv('A', "TS_AC_RESULT", pStream, str.ToString());
        }

        internal static void TS_CA_RSA_PUBLIC_KEY(ref PacketStream pStream)
        {
            // Extends: TS_MESSAGE_WNA
            StringBuilder str = new StringBuilder();
            str.Append("struct TS_CA_RSA_PUBLIC_KEY [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.Append("{\r\n");
            int keySize = pStream.ReadInt32();
            str.Append("	int key_size; = ").Append(keySize).Append("\r\n");
            str.Append("	byte key[key_size] = {\r\n      ");
            for (int i = 0; i < keySize; i++)
            {
                if (i % 16 == 0)
                    str.Append("\r\n        ");
                str.Append(pStream.ReadByte()).Append(" ");
            }
            str.Append("\r\n    }");
            str.Append("	static const uint16_t packetID = 71;").Append("\r\n");
            str.Append("}\r\n");

            Form1.PacketSend('A', "TS_CA_RSA_PUBLIC_KEY", pStream, str.ToString());
        }

        internal static void TS_AC_AES_KEY_IV(ref PacketStream pStream)
        {
            // Extends: TS_MESSAGE_WNA
            StringBuilder str = new StringBuilder();
            str.Append("struct TS_AC_AES_KEY_IV [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.Append("{\r\n");
            int dataSize = pStream.ReadInt32();
            str.Append("	int data_size; = ").Append(dataSize).Append("\r\n");
            str.Append("	byte[] rsa_encrypted_data[data_size] = {");
            for (int i = 0; i < dataSize; i++) {
                if (i % 16 == 0)
                    str.Append("\r\n        ");
                str.Append(pStream.ReadByte()).Append(" ");
            }
            str.Append("\r\n    }");
            str.Append("	static const uint16_t packetID = 72;").Append("\r\n");
            str.Append("}\r\n");

            Form1.PacketRecv('A', "TS_AC_AES_KEY_IV", pStream, str.ToString());
        }

        internal static void TS_DUMMY(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.Append("struct TS_DUMMY [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.AppendLine("{");
			str.AppendLine("	????");
			str.AppendLine("}");

			Form1.PacketSend('A', "TS_DUMMY", pStream, str.ToString());
		}

		internal static void TS_CA_VERSION(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.Append("struct TS_CA_VERSION [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

			str.Append("{\r\n");
			str.Append("	String(20) Version = ").Append(pStream.ReadString(0, 20)).Append("\r\n");
			str.Append("}");

			Form1.PacketSend('A', "TS_CA_VERSION", pStream, str.ToString());
		}

		internal static void TS_CA_OTP_ACCOUNT(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
            str.Append("struct TS_CA_OTP_ACCOUNT [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.Append("{\r\n");
			str.Append("	char[] user_id[60] = ").Append(pStream.ReadString(0, 60)).Append("\r\n");
			str.Append("	char[] user_pass[8] = ").Append(pStream.ReadString(0, 8)).Append("\r\n");
			str.Append("}");

			Form1.PacketSend('A', "TS_CA_OTP_ACCOUNT", pStream, str.ToString());
		}
		
		internal static void TS_CA_SERVER_LIST(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.Append("struct TS_CA_SERVER_LIST [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('A', "TS_CA_SERVER_LIST", pStream, str.ToString());
		}

		internal static void TS_CA_SELECT_SERVER(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct TS_CA_SELECT_SERVER [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.Append("{\r\n");
			str.Append("	short index = ").Append(pStream.ReadInt16()).Append("\r\n");
			str.Append("}");

			Form1.PacketSend('A', "TS_CA_SELECT_SERVER", pStream, str.ToString());
		}

        internal static void TS_AC_RESULT_WITH_STRING(ref PacketStream pStream)
        {
            //struct TS_AC_RESULT_WITH_STRING : public TS_MESSAGE_WNA
            //{
	           // enum LOGIN_SUCCESS_FLAG
            //        {
            //            LSF_EULA_ACCEPTED = 0x1,
            //            LSF_ACCOUNT_BLOCK_WARNING = 0x2,
            //            LSF_DISTRIBUTION_CODE_REQUIRED = 0x4
            //        };

            //        uint16_t request_msg_id;
            //        uint16_t result;
            //        int32_t login_flag;
            //        int32_t strSize;

            //        char string[0];

	           // static const uint16_t packetID = 10002;
            //    };
        }
        
        internal static void TS_CA_IMBC_ACCOUNT(ref PacketStream pStream)
        {
            //struct TS_CA_IMBC_ACCOUNT : public TS_MESSAGE
            //{
            //	char account[61];
            //        unsigned char password[48];
            //        static const uint16_t packetID = 10012;
            //    };

            //    struct TS_CA_IMBC_ACCOUNT_RSA : public TS_MESSAGE
            //{
            //		char account[61];
            //    unsigned int password_size;
            //    unsigned char password[64];
            //    static const int packetID = 10012;

            //    struct AdditionalInfo
            //    {
            //        char type;
            //        unsigned short size;
            //    };
            //};
        }

        internal static void TS_CA_DISTRIBUTION_INFO(ref PacketStream pStream)
        {
            // struct TS_CA_DISTRIBUTION_INFO : public TS_MESSAGE
            //{
	           // static const uint16_t packetID = 10026;
            //    };
        }

        #endregion

        #region Send Packet

        /// [0x2710] 10000 -> (AC) Login Result
        /// <packet id>.W <result>.W <0>.L
        internal static void TS_AC_RESULT(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.Append("struct TS_AC_RESULT [").Append(pStream.GetId()).Append("]\r\n");
            pStream.ReadByte();

            str.Append("{\r\n");
			str.Append("	short packet_id = ").Append(pStream.ReadInt16()).Append("\r\n");
			str.Append("	short result = ").Append(pStream.ReadInt16()).Append("\r\n");
			str.Append("	int unknown = ").Append(pStream.ReadInt32()).Append("\r\n");
			str.Append("}");

			Form1.PacketRecv('A', "TS_AC_RESULT", pStream, str.ToString());

        }

		/// [0x2728] 10024 -> (AC) Join Game (Login Token)
		/// <unknown>.W <key>.8B <0A 00 00 00>.L
		internal static void TS_AC_SELECT_SERVER(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
            str.Append("struct TS_AC_SELECT_SERVER [").Append(pStream.GetId()).Append("]\r\n");

			str.Append("{\r\n");
			str.Append("	ushort result = ").Append(pStream.ReadUInt16()).Append("\r\n");
			str.Append("	ulong one_time_key = ").Append(pStream.ReadInt64()).Append("\r\n");
			str.Append("	uint pending_time = ").Append(pStream.ReadUInt32()).Append("\r\n");
			str.Append("}");

			Form1.PacketRecv('A', "TS_AC_SELECT_SERVER", pStream, str.ToString());

        }
		
		#endregion
	}
}
