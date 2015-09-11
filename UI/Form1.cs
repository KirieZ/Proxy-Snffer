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

namespace RappelzSniffer
{
	public partial class Form1 : Form
	{
		private static Form1 Instance;
		private static bool IsPaused = false;

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
		}

		public static void Log(string message)
		{
			Form1.Instance.Invoke(new MethodInvoker(delegate
			{
				Form1.Instance.logBox.AppendText(message+"\r\n");
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
			PacketStream data = (PacketStream) packets.Rows[e.RowIndex].Cells[4].Value;
			byte[] d = data.GetPacket().ToArray();
			richTextBox2.Text = "";
			for (int i = 0; i < d.Length; i++)
				richTextBox2.AppendText(d[i].ToString("X2") + " ");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (IsPaused)
			{
				IsPaused = false;
				button2.Text = "Pause";
			}
			else
			{
				IsPaused = true;
				button2.Text = "Resume";
			}
		}
	}
}
