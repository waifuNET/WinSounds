using System;
using System.Collections.Generic;
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
			InitFolders();
			sounds.Init();
			Settings.Init();

			Application.Run(winSoundsManager);
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
