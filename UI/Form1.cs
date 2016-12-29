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

namespace RappelzSniffer
{
	public partial class Form1 : Form
	{
		private static Form1 Instance;
		private static bool IsPaused = false;
        private static bool hasstarted = false;

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
			}));
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
			}));
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
            packets.CurrentCell = packets.Rows[e.RowIndex].Cells[0];
        }
    }
}
