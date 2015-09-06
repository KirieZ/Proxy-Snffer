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

		public Form1()
		{
			InitializeComponent();
			Instance = this;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Redirector s = new Redirector(Config.Auth_ServerIp, Config.Auth_ServerPort, Config.Auth_ClientIp, Config.Auth_ClientPort, AuthPackets.PacketReceived);
			BackgroundWorker bgServer = new BackgroundWorker();
			bgServer.DoWork += s.Start;
			bgServer.ProgressChanged += bgServer_ProgressChanged;
			bgServer.WorkerReportsProgress = true;
			bgServer.RunWorkerAsync(bgServer);

			s = new Redirector(Config.Game_ServerIp, Config.Game_ServerPort, Config.Game_ClientIp, Config.Game_ClientPort, GamePackets.PacketReceived);
			bgServer = new BackgroundWorker();
			bgServer.DoWork += s.Start;
			bgServer.ProgressChanged += bgServer_ProgressChanged;
			bgServer.WorkerReportsProgress = true;
			bgServer.RunWorkerAsync(bgServer);
		}

		void bgServer_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			Instance.richTextBox1.AppendText((string)e.UserState + "\r\n");
		}
	}
}
