using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WinSounds.Keyboard.Classes;
using Application = System.Windows.Forms.Application;

namespace WinSounds
{
	static class Program
	{
		public static Manager appManager = new Manager();
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());
			appManager.Load();
		}

	}
}
