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

		internal static void TS_AC_SERVER_LIST(ref PacketStream data)
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

			data.RewriteUInt16(300, Config.Game_ClientPort);

			Form1.PacketRecv('A', GetPacketName(data.GetId()), data, str.ToString());
		}

        #region Parse Packet

        internal static void TS_SC_RESULT(ref PacketStream pStream)
        {
            //struct TS_AC_RESULT : public TS_MESSAGE
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
            //        static const uint16_t packetID = 10000;
            //    };
            //}
        }

        internal static void TS_CA_RSA_PUBLIC_KEY(ref PacketStream pStream)
        {
            //struct TS_CA_RSA_PUBLIC_KEY : public TS_MESSAGE_WNA
            //{
	           // int key_size;
            //        unsigned char key[0];
            //        static const uint16_t packetID = 71;
            //};
        }

        internal static void TS_AC_AES_KEY_IV(ref PacketStream pStream)
        {
            //struct TS_AC_AES_KEY_IV : public TS_MESSAGE_WNA
            //{
	           // int data_size;
            //        unsigned char rsa_encrypted_data[0];
            //        static const uint16_t packetID = 72;
            //};
        }

        internal static void TS_DUMMY(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	????");
			str.AppendLine("}");

			Form1.PacketSend('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}

		internal static void TS_CA_VERSION(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String(20) Version = " + pStream.ReadString(0, 20));
			str.AppendLine("}");

			Form1.PacketSend('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}

		internal static void TS_CA_OTP_ACCOUNT(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	String UserID = " + pStream.ReadString(0, 60));
			str.AppendLine("	String UserPass = " + pStream.ReadString(0, 8));
			str.AppendLine("}");

			Form1.PacketSend('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}
		
		internal static void TS_CA_SERVER_LIST(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("}");

			Form1.PacketSend('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}

		internal static void TS_CA_SELECT_SERVER(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 index = " + pStream.ReadInt16());
			str.AppendLine("}");

			Form1.PacketSend('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
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
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 PacketId = " + pStream.ReadInt16());
			str.AppendLine("	Int16 Result = " + pStream.ReadInt16());
			str.AppendLine("	Int32 Unknown = " + pStream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}

		/// [0x2728] 10024 -> (AC) Join Game (Login Token)
		/// <unknown>.W <key>.8B <0A 00 00 00>.L
		internal static void TS_AC_SELECT_SERVER(ref PacketStream pStream)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("struct " + GetPacketName(pStream.GetId()) + " [" + pStream.GetId() + "]");
			pStream.ReadByte();

			str.AppendLine("{");
			str.AppendLine("	Int16 Unknown = " + pStream.ReadInt16());
			str.Append	  ("	byte[8] = ");
			for (int i = 0; i < 8; i++)
				str.Append(pStream.ReadByte() + " ");
			str.Append("\r\n");
			str.AppendLine("	Int32 Unknown = " + pStream.ReadInt32());
			str.AppendLine("}");

			Form1.PacketRecv('A', GetPacketName(pStream.GetId()), pStream, str.ToString());
		}
		
		#endregion
	}
}
