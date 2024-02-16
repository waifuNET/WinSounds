using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using NAudio.Wave;
using System.Windows.Forms;
using WinSounds.Keyboard.Classes;
using System.Threading;
using System.Diagnostics;
using System.Windows.Automation;
using Gma.System.MouseKeyHook;
using System.Timers;
using System.Windows.Threading;

namespace WinSounds
{
	class Sounds
	{
		private Random random = new Random();
		private string lastSoud = null;
		public bool mute = false;
		public void Init()
		{
            Keyboard.Classes.Hook.Start(KeyBoardSound, MouseSoud);
			WindowHook.Init(WindowSound);
		}
		public void MouseSoud(MouseButtons mouse)
		{
			if (mute) return;
			switch(mouse)
			{
				case MouseButtons.Right:
				case MouseButtons.Left:
					if (Settings.userSettings.CLICK)
						PlaySound(RandomSound(Settings.currentMood.CLICK));
					break;
				case MouseButtons.None:
					if (Settings.userSettings.MOUSE_WHEEL)
						PlaySound(RandomSound(Settings.currentMood.MOUSE_WHEEL));
					break;
			}
		}
		public void KeyBoardSound(int key)
		{
			if (mute) return;
			switch (key)
			{
				case Keyboard.Classes.Keys.VK_BACK:
					if (Settings.userSettings.TYPING_BACKSPACE)
						PlaySound(RandomSound(Settings.currentMood.TYPING_BACKSPACE));
					break;
				case Keyboard.Classes.Keys.VK_SPACE:
					if (Settings.userSettings.TYPING_SPACE)
						PlaySound(RandomSound(Settings.currentMood.TYPING_SPACE));
					break;
				case Keyboard.Classes.Keys.VK_RETURN:
					if (Settings.userSettings.TYPING_ENTER)
						PlaySound(RandomSound(Settings.currentMood.TYPING_ENTER));
					break;

				default:
					if (Settings.userSettings.TYPING_LETTER)
						PlaySound(RandomSound(Settings.currentMood.TYPING_LETTER));
					break;
			}
		}

		public void WindowSound(WindowHookEnum status)
		{
			if (mute) return;
			switch (status)
			{
				case WindowHookEnum.OPEN:
					if (Settings.userSettings.WINDOW_OPEN)
						PlaySound(RandomSound(Settings.currentMood.WINDOW_OPEN));
					break;
				case WindowHookEnum.CLOSE:
					if (Settings.userSettings.WINDOW_CLOSE)
						PlaySound(RandomSound(Settings.currentMood.WINDOW_CLOSE));
					break;
			}
		}

		public string RandomSound(List<string> sounds)
		{
			if (sounds == null || sounds.Count <= 0) return null;
			return sounds[random.Next(0, sounds.Count)];
		}

		public void PlaySound(string path)
		{
			SoundModel waveOut;
			if (!Settings.currentMoodSounds.TryGetValue(path, out waveOut))
			{
				return;
			}

			if (Settings.userSettings.SOLO_TRACK)
				StopSound(lastSoud);

			lastSoud = path;
			waveOut.Play();
		}

		public void StopSound(string path)
		{

			if (path == null) return;

			SoundModel waveOut;
			if (!Settings.currentMoodSounds.TryGetValue(path, out waveOut))
			{
				return;
			}
			waveOut.Stop();
		}
	}
}
