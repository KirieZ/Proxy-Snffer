namespace RappelzSniffer
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.packets = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.TskbarLogging = new System.Windows.Forms.NotifyIcon(this.components);
            this.combo_Filter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkedBox1 = new HexEdit.LinkedBox();
            this.hexEditBox1 = new HexEdit.HexEditBox();
            ((System.ComponentModel.ISupportInitialize)(this.packets)).BeginInit();
            this.SuspendLayout();
            // 
            // packets
            // 
            this.packets.AllowUserToAddRows = false;
            this.packets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.packets.BackgroundColor = System.Drawing.Color.Gray;
            this.packets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.packets.GridColor = System.Drawing.Color.White;
            this.packets.Location = new System.Drawing.Point(0, 29);
            this.packets.Name = "packets";
            this.packets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.packets.Size = new System.Drawing.Size(619, 171);
            this.packets.TabIndex = 0;
            this.packets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.packets_CellClick);
            this.packets.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.packets_RowPostPaint);
            this.packets.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.packets_RowsAdded);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(634, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start Packet Inspection";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 206);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(619, 197);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(634, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Pause Inspection";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TskbarLogging
            // 
            this.TskbarLogging.Text = "PacketInspector";
            this.TskbarLogging.Visible = true;
            // 
            // combo_Filter
            // 
            this.combo_Filter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.combo_Filter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combo_Filter.FormattingEnabled = true;
            this.combo_Filter.Items.AddRange(new object[] {
            "None",
            "TS_SC_RESULT",
            "TS_CA_RSA_PUBLIC_KEY",
            "TS_AC_AES_KEY_IV",
            "TS_DUMMY",
            "TS_AC_RESULT",
            "TS_CA_VERSION",
            "TS_AC_RESULT_WITH_STRING",
            "TS_CA_OTP_ACCOUNT",
            "TS_CA_IMBC_ACCOUNT",
            "TS_CA_SERVER_LIST",
            "TS_AC_SERVER_LIST",
            "TS_CA_SELECT_SERVER",
            "TS_AC_SELECT_SERVER",
            "TS_CA_DISTRIBUTION_INFO"});
            this.combo_Filter.Location = new System.Drawing.Point(87, 2);
            this.combo_Filter.Name = "combo_Filter";
            this.combo_Filter.Size = new System.Drawing.Size(255, 21);
            this.combo_Filter.TabIndex = 8;
            this.combo_Filter.SelectedIndexChanged += new System.EventHandler(this.combo_Filter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Filter:";
            // 
            // linkedBox1
            // 
            this.linkedBox1.Location = new System.Drawing.Point(358, 409);
            this.linkedBox1.Name = "linkedBox1";
            this.linkedBox1.Size = new System.Drawing.Size(262, 156);
            this.linkedBox1.TabIndex = 7;
            this.linkedBox1.Text = "";
            // 
            // hexEditBox1
            // 
            this.hexEditBox1.Location = new System.Drawing.Point(0, 409);
            this.hexEditBox1.Name = "hexEditBox1";
            this.hexEditBox1.Size = new System.Drawing.Size(359, 156);
            this.hexEditBox1.TabIndex = 6;
            this.hexEditBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 566);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.combo_Filter);
            this.Controls.Add(this.linkedBox1);
            this.Controls.Add(this.hexEditBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.packets);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet Inspector";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.packets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView packets;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NotifyIcon TskbarLogging;
        private HexEdit.HexEditBox hexEditBox1;
        private HexEdit.LinkedBox linkedBox1;
        private System.Windows.Forms.ComboBox combo_Filter;
        private System.Windows.Forms.Label label1;
    }
}

