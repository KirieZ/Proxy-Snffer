using RappelzSniffer.RC4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer.Network
{
	public class NetContainer
	{
		public Socket ClSocket { get; set; }
		public XRC4Cipher Encoder { get; set; }
		public XRC4Cipher Decoder { get; set; }
		public byte[] Buffer;
		public int PacketSize;
		public int Offset;
		public PacketStream Data;
	}
}
