using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Windows.ApplicationModel.VoiceCommands;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinSounds
{
	public partial class WinSoundsManager : Form
	{
		WinSound winSound = new WinSound();
		bool winSoundActive = false;

		public WinSoundsManager()
		{
			InitializeComponent();
			WinSoundsNotify.MouseClick += notifyIcon_Click;
		}
		public void ChangeViStatus(MouseButtons cl)
		{
			if (cl == MouseButtons.Left)
			{
				if (!winSoundActive)
				{
					winSound = new WinSound();
					winSound.Show();
					winSoundActive = true;
				}
				else
				{
					winSound.Close();
					winSoundActive = false;
				}
			}
		}

		void notifyIcon_Click(object sender, MouseEventArgs e)
		{
			ChangeViStatus(e.Button);
		}

		private ContextMenuStrip menu = new ContextMenuStrip();

		private void WinSoundsManager_Load(object sender, EventArgs e)
		{
			UpdateNotify();

			WinSoundsNotify.ContextMenuStrip = menu;

			WinSoundsNotify.BalloonTipText = "Was launched in tray.";
			WinSoundsNotify.BalloonTipTitle = "WinSounds";
			WinSoundsNotify.ShowBalloonTip(1);
		}

		private void UpdateNotify()
		{
			menu.Items.Clear();

			ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Open");
			ToolStripMenuItem muteMenuItem = new ToolStripMenuItem("Mute");
			ToolStripMenuItem closeMenuItem = new ToolStripMenuItem("Close");

			openMenuItem.Click += (object sender, EventArgs e) => ChangeViStatus(MouseButtons.Left);
			closeMenuItem.Click += (object sender, EventArgs e) => Program.appManager.Exit();

			muteMenuItem.Checked = Settings.MUTE;
			muteMenuItem.Click += (object sender, EventArgs e) => { Settings.MUTE = !Settings.MUTE; UpdateNotify(); };

			menu.Items.Add(openMenuItem);
			menu.Items.Add(muteMenuItem);
			menu.Items.Add(closeMenuItem);
		}
	}
}
