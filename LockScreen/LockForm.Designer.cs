namespace LockScreen
{
    partial class LockForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LockForm));
            this.icoTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.appleUnlockWidget1 = new LockScreen.Ui.AppleUnlockWidget();
            this.statusBar1 = new LockScreen.StatusBar();
            this.wifiWidget1 = new LockScreen.Ui.StatusBar.WifiWidget();
            this.batteryWidget1 = new LockScreen.BatteryWidget();
            this.contextMenuStrip1.SuspendLayout();
            this.statusBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // icoTray
            // 
            this.icoTray.ContextMenuStrip = this.contextMenuStrip1;
            this.icoTray.Icon = ((System.Drawing.Icon)(resources.GetObject("icoTray.Icon")));
            this.icoTray.Text = "LockScreen";
            this.icoTray.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.statusBar1.SetDisplayIndex(this.contextMenuStrip1, 0);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 26);
            // 
            // mnuClose
            // 
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.Size = new System.Drawing.Size(103, 22);
            this.mnuClose.Text = "Close";
            this.mnuClose.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // appleUnlockWidget1
            // 
            this.appleUnlockWidget1.BackColor = System.Drawing.Color.Transparent;
            this.statusBar1.SetDisplayIndex(this.appleUnlockWidget1, 0);
            this.appleUnlockWidget1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.appleUnlockWidget1.Location = new System.Drawing.Point(0, 607);
            this.appleUnlockWidget1.Name = "appleUnlockWidget1";
            this.appleUnlockWidget1.Size = new System.Drawing.Size(998, 90);
            this.appleUnlockWidget1.TabIndex = 5;
            // 
            // statusBar1
            // 
            this.statusBar1.BackColor = System.Drawing.Color.Transparent;
            this.statusBar1.Controls.Add(this.wifiWidget1);
            this.statusBar1.Controls.Add(this.batteryWidget1);
            this.statusBar1.SetDisplayIndex(this.statusBar1, 0);
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusBar1.Location = new System.Drawing.Point(0, 0);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(998, 110);
            this.statusBar1.TabIndex = 4;
            // 
            // wifiWidget1
            // 
            this.wifiWidget1.BackColor = System.Drawing.Color.Transparent;
            this.statusBar1.SetDisplayIndex(this.wifiWidget1, 1);
            this.wifiWidget1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.wifiWidget1.Location = new System.Drawing.Point(3, 41);
            this.wifiWidget1.Name = "wifiWidget1";
            this.wifiWidget1.Size = new System.Drawing.Size(123, 32);
            this.wifiWidget1.TabIndex = 1;
            // 
            // batteryWidget1
            // 
            this.batteryWidget1.BackColor = System.Drawing.Color.Transparent;
            this.statusBar1.SetDisplayIndex(this.batteryWidget1, 0);
            this.batteryWidget1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.batteryWidget1.Location = new System.Drawing.Point(3, 3);
            this.batteryWidget1.Name = "batteryWidget1";
            this.batteryWidget1.Size = new System.Drawing.Size(138, 32);
            this.batteryWidget1.TabIndex = 0;
            this.batteryWidget1.Text = "98,00% (Laden...)";
            // 
            // LockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(998, 697);
            this.ControlBox = false;
            this.Controls.Add(this.appleUnlockWidget1);
            this.Controls.Add(this.statusBar1);
            this.statusBar1.SetDisplayIndex(this, 0);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LockForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LockForm";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnGlobalKeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGlobalMouseHandler);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnGlobalMouseHandler);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusBar1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon icoTray;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private StatusBar statusBar1;
        private BatteryWidget batteryWidget1;
        private Ui.StatusBar.WifiWidget wifiWidget1;
        private Ui.AppleUnlockWidget appleUnlockWidget1;


    }
}

