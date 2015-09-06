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
			this.packets = new System.Windows.Forms.DataGridView();
			this.direct = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.len = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.data = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.logBox = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.str = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.richTextBox2 = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.packets)).BeginInit();
			this.SuspendLayout();
			// 
			// packets
			// 
			this.packets.AllowUserToAddRows = false;
			this.packets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.packets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.packets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.direct,
            this.id,
            this.name,
            this.len,
            this.data,
            this.str});
			this.packets.Location = new System.Drawing.Point(12, 25);
			this.packets.Name = "packets";
			this.packets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.packets.Size = new System.Drawing.Size(619, 171);
			this.packets.TabIndex = 0;
			this.packets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.packets_CellClick);
			// 
			// direct
			// 
			this.direct.FillWeight = 60.9137F;
			this.direct.HeaderText = "Direction";
			this.direct.Name = "direct";
			this.direct.ReadOnly = true;
			// 
			// id
			// 
			this.id.FillWeight = 49.14294F;
			this.id.HeaderText = "ID";
			this.id.MaxInputLength = 10;
			this.id.Name = "id";
			this.id.ReadOnly = true;
			// 
			// name
			// 
			this.name.FillWeight = 144.9717F;
			this.name.HeaderText = "Name";
			this.name.Name = "name";
			this.name.ReadOnly = true;
			// 
			// len
			// 
			this.len.FillWeight = 144.9717F;
			this.len.HeaderText = "Length";
			this.len.Name = "len";
			this.len.ReadOnly = true;
			// 
			// data
			// 
			this.data.HeaderText = "Raw Data";
			this.data.Name = "data";
			this.data.Visible = false;
			// 
			// logBox
			// 
			this.logBox.Location = new System.Drawing.Point(12, 482);
			this.logBox.Name = "logBox";
			this.logBox.Size = new System.Drawing.Size(619, 56);
			this.logBox.TabIndex = 1;
			this.logBox.Text = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 544);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Start Proxies";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// str
			// 
			this.str.HeaderText = "Struct";
			this.str.Name = "str";
			this.str.Visible = false;
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(12, 202);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(619, 190);
			this.richTextBox1.TabIndex = 3;
			this.richTextBox1.Text = "";
			// 
			// richTextBox2
			// 
			this.richTextBox2.Location = new System.Drawing.Point(12, 398);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.Size = new System.Drawing.Size(619, 65);
			this.richTextBox2.TabIndex = 4;
			this.richTextBox2.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(643, 579);
			this.Controls.Add(this.richTextBox2);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.logBox);
			this.Controls.Add(this.packets);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Packet Analyzer";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.packets)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView packets;
		private System.Windows.Forms.DataGridViewTextBoxColumn direct;
		private System.Windows.Forms.DataGridViewTextBoxColumn id;
		private System.Windows.Forms.DataGridViewTextBoxColumn name;
		private System.Windows.Forms.DataGridViewTextBoxColumn len;
		private System.Windows.Forms.RichTextBox logBox;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.DataGridViewTextBoxColumn data;
		private System.Windows.Forms.DataGridViewTextBoxColumn str;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.RichTextBox richTextBox2;

	}
}

