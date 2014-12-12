namespace TestApplication
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.txtAddress = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.txtPort = new System.Windows.Forms.ToolStripTextBox();
			this.btnConnect = new System.Windows.Forms.ToolStripButton();
			this.btnListen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.txtSend = new System.Windows.Forms.ToolStripTextBox();
			this.btnSend = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnFork = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.lstReceived = new System.Windows.Forms.ListBox();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtAddress,
            this.toolStripLabel1,
            this.txtPort,
            this.btnConnect,
            this.btnListen,
            this.toolStripSeparator1,
            this.txtSend,
            this.btnSend,
            this.toolStripSeparator2,
            this.btnFork});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1120, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// txtAddress
			// 
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(100, 25);
			this.txtAddress.Text = "127.0.0.1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(10, 22);
			this.toolStripLabel1.Text = ":";
			// 
			// txtPort
			// 
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(100, 25);
			this.txtPort.Text = "12345";
			// 
			// btnConnect
			// 
			this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
			this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(56, 22);
			this.btnConnect.Text = "Connect";
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnListen
			// 
			this.btnListen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnListen.Image = ((System.Drawing.Image)(resources.GetObject("btnListen.Image")));
			this.btnListen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnListen.Name = "btnListen";
			this.btnListen.Size = new System.Drawing.Size(42, 22);
			this.btnListen.Text = "Listen";
			this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// txtSend
			// 
			this.txtSend.Enabled = false;
			this.txtSend.Name = "txtSend";
			this.txtSend.Size = new System.Drawing.Size(300, 25);
			this.txtSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSend_KeyDown);
			// 
			// btnSend
			// 
			this.btnSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSend.Enabled = false;
			this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
			this.btnSend.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(37, 22);
			this.btnSend.Text = "Send";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnFork
			// 
			this.btnFork.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnFork.Image = ((System.Drawing.Image)(resources.GetObject("btnFork.Image")));
			this.btnFork.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFork.Name = "btnFork";
			this.btnFork.Size = new System.Drawing.Size(34, 22);
			this.btnFork.Text = "Fork";
			this.btnFork.Click += new System.EventHandler(this.btnFork_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 721);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1120, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
			this.toolStripStatusLabel1.Text = "Status:";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(102, 17);
			this.lblStatus.Text = "WAITING USER.. :)";
			// 
			// lstReceived
			// 
			this.lstReceived.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstReceived.FormattingEnabled = true;
			this.lstReceived.IntegralHeight = false;
			this.lstReceived.Location = new System.Drawing.Point(0, 25);
			this.lstReceived.Name = "lstReceived";
			this.lstReceived.Size = new System.Drawing.Size(1120, 696);
			this.lstReceived.TabIndex = 3;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1120, 743);
			this.Controls.Add(this.lstReceived);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "Utf8NetworkLib Test Application";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripTextBox txtAddress;
		private System.Windows.Forms.ToolStripTextBox txtPort;
		private System.Windows.Forms.ToolStripButton btnConnect;
		private System.Windows.Forms.ToolStripButton btnListen;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripTextBox txtSend;
		private System.Windows.Forms.ToolStripButton btnSend;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnFork;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ListBox lstReceived;
	}
}

