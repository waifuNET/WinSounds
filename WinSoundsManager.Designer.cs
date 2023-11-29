namespace WinSounds
{
	partial class WinSoundsManager
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinSoundsManager));
			WinSoundsNotify = new System.Windows.Forms.NotifyIcon(components);
			SuspendLayout();
			// 
			// WinSoundsNotify
			// 
			WinSoundsNotify.Icon = (System.Drawing.Icon)resources.GetObject("WinSoundsNotify.Icon");
			WinSoundsNotify.Text = "WinSounds";
			WinSoundsNotify.Visible = true;
			// 
			// WinSoundsManager
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(443, 273);
			ControlBox = false;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			Name = "WinSoundsManager";
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "WinSoundsManager";
			WindowState = System.Windows.Forms.FormWindowState.Minimized;
			Load += WinSoundsManager_Load;
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.NotifyIcon WinSoundsNotify;
	}
}