using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RappelzSniffer.Network;
using HexEdit;
using System.IO;

namespace RappelzSniffer
{
	public partial class Form1 : Form
	{
		private static Form1 Instance;
		private static bool IsPaused = false;
        private static bool hasstarted = false;

        private const string logFile = "packet_dump.log";

        public Form1()
		{
			InitializeComponent();
			Instance = this;
		}

        private void Form1_Load(object sender, EventArgs e)
		{
        }

        private void button1_Click(object sender, EventArgs e)
		{
            if (hasstarted == false)
            {
                hasstarted = true;
                button1.Text = "Start Packet Inspection";
                Redirector s = new Redirector(Config.Auth_ServerIp, Config.Auth_ServerPort, Config.Auth_ClientIp, Config.Auth_ClientPort, AuthPackets.PacketReceived);
                BackgroundWorker bgServer = new BackgroundWorker();
                bgServer.DoWork += s.Start;
                bgServer.WorkerReportsProgress = true;
                bgServer.RunWorkerAsync(bgServer);

                s = new Redirector(Config.Game_ServerIp, Config.Game_ServerPort, Config.Game_ClientIp, Config.Game_ClientPort, GamePackets.PacketReceived);
                bgServer = new BackgroundWorker();
                bgServer.DoWork += s.Start;
                bgServer.WorkerReportsProgress = true;
                bgServer.RunWorkerAsync(bgServer);
                button1.Text = "Stop Packet Inspection";
            }
            else
            {   //Initialise application.restart due to sockets unbind etc, too much coding it is easyer to restart doing so.
                Application.Restart();
            }
        }

		public static void Log(string message)
		{
            Form1.Instance.Invoke(new MethodInvoker(delegate
            {
                Form1.Instance.TskbarLogging.Visible = true;
                Form1.Instance.TskbarLogging.Icon = SystemIcons.Exclamation;
                Form1.Instance.TskbarLogging.BalloonTipTitle = "Packet Inspector";
                Form1.Instance.TskbarLogging.BalloonTipText = message + "\r\n";
                Form1.Instance.TskbarLogging.BalloonTipIcon = ToolTipIcon.Error;
                Form1.Instance.TskbarLogging.ShowBalloonTip(1000);
            }));
        }

		public static void PacketRecv(char src, string name, PacketStream data, string str = "")
		{
			if (IsPaused) return;
			Form1.Instance.Invoke(new MethodInvoker(delegate
			{
				int i = Form1.Instance.packets.Rows.Add();
				Form1.Instance.packets.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
				Form1.Instance.packets.Rows[i].Cells[0].Value = String.Format("({0}) recv", src);
				Form1.Instance.packets.Rows[i].Cells[1].Value = "0x" + data.GetId().ToString("X4");
				Form1.Instance.packets.Rows[i].Cells[2].Value = name;
				Form1.Instance.packets.Rows[i].Cells[3].Value = data.ToArray().Length;
				Form1.Instance.packets.Rows[i].Cells[4].Value = data;
				Form1.Instance.packets.Rows[i].Cells[5].Value = str;
                Form1.Instance.packets_CellClick(null, new DataGridViewCellEventArgs(0, i));
            }));
            LogToFile("received from", src, name, data, str);
		}

        public static void PacketSend(char src, string name, PacketStream data, string str = "")
		{
			if (IsPaused) return;
			Form1.Instance.Invoke(new MethodInvoker(delegate
			{
				int i = Form1.Instance.packets.Rows.Add();
				Form1.Instance.packets.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
				Form1.Instance.packets.Rows[i].Cells[0].Value = String.Format("({0}) send", src);
				Form1.Instance.packets.Rows[i].Cells[1].Value = "0x" + data.GetId().ToString("X4");
				Form1.Instance.packets.Rows[i].Cells[2].Value = name;
				Form1.Instance.packets.Rows[i].Cells[3].Value = data.ToArray().Length;
				Form1.Instance.packets.Rows[i].Cells[4].Value = data;
				Form1.Instance.packets.Rows[i].Cells[5].Value = str;
                Form1.Instance.packets_CellClick(null, new DataGridViewCellEventArgs(0, i));
            }));
            LogToFile("sent to", src, name, data, str);
        }

        private static void LogToFile(string dir, char src, string name, PacketStream data, string str = "")
        {
            int id = data.GetId();
            byte[] dataArray = data.ToArray();

            StringBuilder hexDump = new StringBuilder();
            StringBuilder asciiDump = new StringBuilder();
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (i % 16 == 0 && i > 0)
                {
                    byte b = dataArray[i];
                    hexDump.AppendFormat(Environment.NewLine + "{0:x2} ", b);
                    asciiDump.Append(Environment.NewLine + (b < 32 ? '·' : (char)b));
                }
                else
                {
                    byte b = dataArray[i];
                    hexDump.AppendFormat("{0:x2} ", b);
                    asciiDump.Append((b < 32 ? '·' : (char)b));
                }
            }

            string msg = String.Format(
                "Packet [{0}] - {1}[{2}] {3} {4} ({5} bytes)\n\n" +
                "{6}\n\n" +
                "{7}\n\n" +
                "{8}\n" +
                "-----------------------------------------------------------\n\n",
                name,
                id,
                "0x" + data.GetId().ToString("X4"),
                dir,
                (src == 'A' ? "Auth" : "Game"),
                dataArray.Length,
                hexDump.ToString(),
                asciiDump.ToString(),
                str
            );
            File.AppendAllText(logFile, msg);
        }

        private void packets_CellClick(object sender, DataGridViewCellEventArgs e)
		{
            if (e.RowIndex < 0) return;
            richTextBox1.Text = packets.Rows[e.RowIndex].Cells[5].Value.ToString().Replace("\t", "    ");
            PacketStream data = (PacketStream)packets.Rows[e.RowIndex].Cells[4].Value;
            byte[] d = data.GetPacket().ToArray();
            linkedBox1.Text = "";
            for (int i = 0; i < d.Length; i++)
                hexEditBox1.LoadData(d);
            hexEditBox1.LinkDisplay(linkedBox1);
        }

		private void button2_Click(object sender, EventArgs e)
		{
			if (IsPaused)
			{
				IsPaused = false;
				button2.Text = "Pause Inspection";
			}
			else
			{
				IsPaused = true;
				button2.Text = "Resume Inspection";
			}
		}

        private void packets_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex < 0) return;
            packets.CurrentCell = packets.Rows[e.RowIndex].Cells[0];
        }
    }
}
