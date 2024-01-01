using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinSounds
{
	class Manager
	{
		public Sounds sounds = new Sounds();
		public WinSoundsManager winSoundsManager = new WinSoundsManager();
		public void Load()
		{
			AUTOloadFix();

			InitFolders();
			sounds.Init();
			Settings.Init();

			Application.Run(winSoundsManager);
		}

		public static void AUTOloadFix()
		{
			while (true)
			{
				if (AutoLoad.SessionCheck() != null)
				{
					return;
				}
				else
				{
					Thread.Sleep(1000);
				}
				return;
			}
		}

		public void InitFolders()
		{
			if (!Directory.Exists(".\\Moods")) Directory.CreateDirectory(".\\Moods");
		}

		public void Exit()
		{
			Settings.SaveSettings();
			Application.Exit();
		}
	}
}
