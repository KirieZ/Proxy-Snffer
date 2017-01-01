using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer
{
	public static class Config
	{
        #region Packet Configuration
        public const int MaxBuffer = 4096;
		public const int HeaderLength = 7;
        #endregion

        #region Server Configuration
        public static string Auth_ServerIp = "182.162.85.10"; //77.161.174.249
        public static string Auth_ClientIp = "127.0.0.1";

		public static string Game_ServerIp = "182.162.85.10";
		public static string Game_ClientIp = "127.0.0.1";

		public static ushort Auth_ServerPort = 4500; //8841
        public static ushort Auth_ClientPort = 8842; //8842

        public static ushort Game_ServerPort = 4514; //6900
        public static ushort Game_ClientPort = 4505; //4505
        #endregion

        #region PrivateKey Configuration
        public static string RC4Key = "}h79q~B%al;k'y $E";
        #endregion
    }
}
