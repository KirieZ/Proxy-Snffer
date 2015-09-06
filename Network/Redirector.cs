using RappelzSniffer.RC4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RappelzSniffer.Network
{
	class Redirector
	{
		public static ManualResetEvent allDone = new ManualResetEvent(false);

		private string ServerIp;
		private int ServerPort;
		private string ClientIp;
		private int ClientPort;

		private NetContainer Server;
		private NetContainer Client;

		private BackgroundWorker bgWorker;

		private Func<char, PacketStream, PacketStream> PacketFunc;

		public Redirector(string serverIp, int serverPort, string clientIp, int clientPort, Func<char, PacketStream, PacketStream> pFun)
		{
			this.ServerIp = serverIp;
			this.ServerPort = serverPort;
			this.ClientIp = clientIp;
			this.ClientPort = clientPort;
			this.PacketFunc = pFun;
		}

		public void Start(object sender, DoWorkEventArgs evArgs)
		{
			bgWorker = (BackgroundWorker)evArgs.Argument;

			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(this.ClientIp), this.ClientPort);

			Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
			try
			{
				Server = new NetContainer();
				Server.ClSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
				Server.ClSocket.Connect(this.ServerIp, this.ServerPort);
				Server.Encoder = new XRC4Cipher(Config.RC4Key);
				Server.Decoder = new XRC4Cipher(Config.RC4Key);
				Server.Buffer = new byte[Config.MaxBuffer];
				Server.Data = new PacketStream();

				Server.ClSocket.BeginReceive(Server.Buffer, 0, Config.MaxBuffer, 0, new AsyncCallback(ServerReadCallback), Server);

				listener.Bind(localEndPoint);
				listener.Listen(1);
				listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

				Form1.Log("Initialized");
			}
			catch (Exception e)
			{
				bgWorker.ReportProgress(0, e.Message.ToString());
				allDone.Set();
			}

			allDone.WaitOne();
		}

		// ===========================================
		//				CLIENT
		// ===========================================

		private void AcceptCallback(IAsyncResult ar)
		{
			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			Client = new NetContainer();
			Client.ClSocket = handler;
			Client.Encoder = new XRC4Cipher(Config.RC4Key);
			Client.Decoder = new XRC4Cipher(Config.RC4Key);
			Client.Buffer = new byte[Config.MaxBuffer];
			Client.Data = new PacketStream();

			bgWorker.ReportProgress(0, "Client Connect");

			handler.BeginReceive(
				Client.Buffer,
				0,
				Config.MaxBuffer,
				0,
				new AsyncCallback(ReadCallback),
				Client
			);
		}

		// Client ---> Redirector
		private void ReadCallback(IAsyncResult ar)
		{
			//try
			//{
			// Read data from the client socket. 
			int bytesRead = Client.ClSocket.EndReceive(ar);
			if (bytesRead > 0)
			{
				byte[] decode = Client.Decoder.DoCipher(ref Client.Buffer, bytesRead);
				int curOffset = 0;
				int bytesToRead = 0;

				do
				{
					if (Client.PacketSize == 0)
					{
						if (Client.Offset + bytesRead > 3)
						{
							bytesToRead = (4 - Client.Offset);
							Client.Data.Write(decode, curOffset, bytesToRead);
							curOffset += bytesToRead;
							Client.Offset = bytesToRead;
							Client.PacketSize = BitConverter.ToInt32(Client.Data.ReadBytes(0, 4, true), 0);
						}
						else
						{
							Client.Data.Write(decode, 0, bytesRead);
							Client.Offset += bytesRead;
							curOffset += bytesRead;
						}
					}
					else
					{
						int needBytes = Client.PacketSize - Client.Offset;

						// If there's enough bytes to complete this packet
						if (needBytes <= (bytesRead - curOffset))
						{
							Client.Data.Write(decode, curOffset, needBytes);
							curOffset += needBytes;
							// Packet is done, send to server to be parsed
							// and continue.
							PacketReceived(Client.Data);
							// Do needed clean up to start a new packet
							Client.Data = new PacketStream();
							Client.PacketSize = 0;
							Client.Offset = 0;
						}
						else
						{
							bytesToRead = (bytesRead - curOffset);
							Client.Data.Write(decode, curOffset, bytesToRead);
							Client.Offset += bytesToRead;
							curOffset += bytesToRead;
						}
					}
				} while (bytesRead - 1 > curOffset);

			}
			else
			{
				//ConsoleUtils.Write(ConsoleMsgType.Info, "User disconected\r\n");
				return;
			}
			/*}
			catch (Exception e)
			{
				ConsoleUtils.Write(MSG_TYPE.Info, "User disconected. " + e.Message);
			}*/
			Client.ClSocket.BeginReceive(
				Client.Buffer,
				0,
				Config.MaxBuffer,
				0,
				new AsyncCallback(ReadCallback),
				Client
			);
		}

		// Client --> Redirector
		private void PacketReceived(PacketStream data)
		{
			//bgWorker.ReportProgress(0, "Packet received from client");
			data = PacketFunc('C', data);
			SendServer(data);
		}

		// Redirector --> Client
		public void Send(PacketStream data)
		{
			byte[] byteData = data.GetPacket().ToArray();
			//ConsoleUtils.HexDump(byteData, "Sending Packet");

			// Begin sending the data to the remote device.
			Client.ClSocket.BeginSend(
				Client.Encoder.DoCipher(ref byteData),
				0,
				byteData.Length,
				0,
				new AsyncCallback(SendCallback),
				Client
			);
		}

		// Redirector --> Client
		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				int bytesSent = Client.ClSocket.EndSend(ar);
				bgWorker.ReportProgress(0, "Packet sent to client");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		// ===========================================
		//				SERVER
		// ===========================================

		// Server --> Redirector
		private void ServerReadCallback(IAsyncResult ar)
		{
			//try
			//{
			// Read data from the client socket. 
			int bytesRead = Server.ClSocket.EndReceive(ar);
			if (bytesRead > 0)
			{
				byte[] decode = Server.Decoder.DoCipher(ref Server.Buffer, bytesRead);
				int curOffset = 0;
				int bytesToRead = 0;

				do
				{
					if (Server.PacketSize == 0)
					{
						if (Server.Offset + bytesRead > 3)
						{
							bytesToRead = (4 - Server.Offset);
							Server.Data.Write(decode, curOffset, bytesToRead);
							curOffset += bytesToRead;
							Server.Offset = bytesToRead;
							Server.PacketSize = BitConverter.ToInt32(Server.Data.ReadBytes(0, 4, true), 0);
						}
						else
						{
							Server.Data.Write(decode, 0, bytesRead);
							Server.Offset += bytesRead;
							curOffset += bytesRead;
						}
					}
					else
					{
						int needBytes = Server.PacketSize - Server.Offset;

						// If there's enough bytes to complete this packet
						if (needBytes <= (bytesRead - curOffset))
						{
							Server.Data.Write(decode, curOffset, needBytes);
							curOffset += needBytes;
							// Packet is done, send to server to be parsed
							// and continue.
							ServerPacketReceived(Server.Data);
							// Do needed clean up to start a new packet
							Server.Data = new PacketStream();
							Server.PacketSize = 0;
							Server.Offset = 0;
						}
						else
						{
							bytesToRead = (bytesRead - curOffset);
							Server.Data.Write(decode, curOffset, bytesToRead);
							Server.Offset += bytesToRead;
							curOffset += bytesToRead;
						}
					}
				} while (bytesRead - 1 > curOffset);

			}
			else
			{
				//ConsoleUtils.Write(ConsoleMsgType.Info, "User disconected\r\n");
				return;
			}
			/*}
			catch (Exception e)
			{
				ConsoleUtils.Write(MSG_TYPE.Info, "User disconected. " + e.Message);
			}*/
			Server.ClSocket.BeginReceive(
				Server.Buffer,
				0,
				Config.MaxBuffer,
				0,
				new AsyncCallback(ServerReadCallback),
				Server
			);
		}

		// Server --> Redirector
		private void ServerPacketReceived(PacketStream data)
		{
			bgWorker.ReportProgress(0, "Packet received from server");
			data = PacketFunc('S', data);
			Send(data);
		}

		// Redirector --> Server
		public void SendServer(PacketStream data)
		{
			byte[] byteData = data.GetPacket().ToArray();
			//ConsoleUtils.HexDump(byteData, "Sending Packet");

			// Begin sending the data to the remote device.
			Server.ClSocket.BeginSend(
				Server.Encoder.DoCipher(ref byteData),
				0,
				byteData.Length,
				0,
				new AsyncCallback(SendServerCallback),
				Server
			);
		}

		private void SendServerCallback(IAsyncResult ar)
		{
			try
			{
				int bytesSent = Server.ClSocket.EndSend(ar);
				bgWorker.ReportProgress(0, "Packet sent to server");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
