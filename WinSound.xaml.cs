using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinSounds
{
	/// <summary>
	/// Логика взаимодействия для WinSound.xaml
	/// </summary>
	public partial class WinSound : System.Windows.Window
    {
		public WinSound()
		{
			InitializeComponent();
		}

		private int idMood = -1;
		private bool updateValues = false;

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateListUI();
			UpdateSelectedUI(Settings.currentMood);
			Timer();
			timer_Tick(new object(), new EventArgs());
		}

		public void UpdateSelectedUI(Mood mood)
		{
			updateValues = false;
			UpdateGraphicSettings();
			if (mood != null && mood.NAME != null)
			{
				UIVisibility(Visibility.Visible);
				string icon = System.IO.Path.Join(mood.PATH, "icon.png");

				UI_Mood_Icon.Source = new BitmapImage(new Uri(icon));
				UI_Mood_Name.Content = mood.NAME;
				UI_Mood_Discription.Content = mood.NAME;

				if (Settings.currentMood.PATH != mood.PATH)
				{
					UI_ButtonInstall.Visibility = Visibility.Visible;
				}
				else
				{
					UI_ButtonInstall.Visibility = Visibility.Hidden;
				}
			}
			else
			{
				UIVisibility(Visibility.Hidden);
			}
			updateValues = true;
		}

		public void UpdateGraphicSettings()
		{
			UI_Mood_Backspace.IsChecked = Settings.userSettings.TYPING_BACKSPACE;
			UI_Mood_Enter.IsChecked = Settings.userSettings.TYPING_ENTER;
			UI_Mood_Letter.IsChecked = Settings.userSettings.TYPING_LETTER;
			UI_Mood_Space.IsChecked = Settings.userSettings.TYPING_SPACE;
			UI_Mood_WindowClose.IsChecked = Settings.userSettings.WINDOW_CLOSE;
			UI_Mood_WindowOpen.IsChecked = Settings.userSettings.WINDOW_OPEN;
			UI_Mood_Click.IsChecked = Settings.userSettings.CLICK;
			UI_Mood_Volume.Value = Settings.userSettings.VOLUME;
			UI_Mood_ClickWheel.IsChecked = Settings.userSettings.MOUSE_WHEEL;
			UI_SOLO_TRACK.IsChecked = Settings.userSettings.SOLO_TRACK;
			UI_HOLDING.IsChecked = Settings.userSettings.HOLDING;
			UI_SOLO_AUTOLOAD.IsChecked = Settings.userSettings.AUTO_LOAD;
		}

		public void UIVisibility(Visibility visibility)
		{
			UI_ButtonInstall.Visibility = visibility;
			UI_Mood_Backspace.Visibility = visibility;
			UI_Mood_Click.Visibility = visibility;
			UI_Mood_Discription.Visibility = visibility;
			UI_Mood_Enter.Visibility = visibility;
			UI_Mood_Icon.Visibility = visibility;
			UI_Mood_Letter.Visibility = visibility;
			UI_Mood_Name.Visibility = visibility;
			UI_Mood_Space.Visibility = visibility;
			UI_Mood_Volume.Visibility = visibility;
			UI_Mood_WindowOpen.Visibility = visibility;
			UI_Mood_WindowClose.Visibility = visibility;
			UI_Mood_ClickWheel.Visibility = visibility;
		}

		public void Timer()
		{
			DispatcherTimer timer = new DispatcherTimer();
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = new TimeSpan(0, 0, 3);
			timer.Start();
		}
		private int processIdRefresh = 0;
		private void timer_Tick(object sender, EventArgs e)
		{
			int IdRefresh = 0;
			string[] processCollection = Array.ConvertAll(Process.GetProcesses(), x => x.ProcessName);

			IEnumerable<string> processFiltred = processCollection.Except(Settings.ignored_proc);
			processFiltred = processFiltred.Except(Settings.userSettings.IGNORED_PROCESSES);

			ProcessesIgnored.Items.Clear();
			foreach (string proc in Settings.userSettings.IGNORED_PROCESSES)
			{
				ProcessesIgnored.Items.Add(proc);
			}

			IEnumerable<string> bans = processCollection.Intersect(Settings.userSettings.IGNORED_PROCESSES);

			Settings.MUTE = bans.ToArray().Length > 0;

			foreach (string st in processFiltred)
			{
				IdRefresh += st[0];
			}
			if (IdRefresh == processIdRefresh) { return; }
			else { processIdRefresh = IdRefresh; }

			Processes.Items.Clear();
			foreach (string proc in processFiltred)
			{
				Processes.Items.Add(proc);
			}
		}

		private void UI_Mood_Backspace_Checked(object sender, RoutedEventArgs e)
		{
			if(updateValues)
			Settings.userSettings.TYPING_BACKSPACE = (bool)UI_Mood_Backspace.IsChecked;

		}

		private void UI_Mood_Enter_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.TYPING_ENTER = (bool)UI_Mood_Enter.IsChecked;

		}

		private void UI_Mood_Letter_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.TYPING_LETTER = (bool)UI_Mood_Letter.IsChecked;

		}

		private void UI_Mood_Space_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.TYPING_SPACE = (bool)UI_Mood_Space.IsChecked;

		}

		private void UI_Mood_WindowClose_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.WINDOW_CLOSE = (bool)UI_Mood_WindowClose.IsChecked;

		}

		private void UI_Mood_WindowOpen_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.WINDOW_OPEN = (bool)UI_Mood_WindowOpen.IsChecked;

		}

		private void UI_Mood_Click_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.CLICK = (bool)UI_Mood_Click.IsChecked;

		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (updateValues)
				Settings.userSettings.VOLUME = (float)UI_Mood_Volume.Value;

		}

		private void UI_MoodsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			idMood = UI_MoodsList.SelectedIndex;
			if (idMood != -1)
				UpdateSelectedUI(Settings.GetMood(idMood));
			else
				UpdateSelectedUI(Settings.currentMood);
		}

		private void UI_ButtonUpdate_Click(object sender, RoutedEventArgs e)
		{
			UpdateListUI();
		}

		public void UpdateListUI()
		{
			Settings.LoadMoods();
			UI_MoodsList.Items.Clear();
			foreach (Mood mood in Settings.moods)
			{
				UI_MoodsList.Items.Add(mood.NAME);
			}
		}

		private void UI_ButtonInstall_Click(object sender, RoutedEventArgs e)
		{
			Settings.SetCurrentMood(idMood);
			UpdateSelectedUI(Settings.currentMood);
			Settings.SaveSettings();
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Settings.SaveSettings();
		}

		private void UI_SOLO_TRACK_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.SOLO_TRACK = (bool)UI_SOLO_TRACK.IsChecked;

		}

		private void UI_HOLDING_Checked(object sender, RoutedEventArgs e)
		{
			if(updateValues)
				Settings.userSettings.HOLDING = (bool)UI_HOLDING.IsChecked;
		}

		private void UI_ButtonOpenFolder_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("explorer.exe", Settings.MOOD_PATH);
		}

		private void UI_Mood_ClickWheel_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
				Settings.userSettings.MOUSE_WHEEL = (bool)UI_Mood_ClickWheel.IsChecked;
		}

		private void UI_SOLO_AUTOLOAD_Checked(object sender, RoutedEventArgs e)
		{
			if (updateValues)
			{
				Settings.userSettings.AUTO_LOAD = (bool)UI_SOLO_AUTOLOAD.IsChecked;
				AutoLoad.SetAutorunValue(Settings.userSettings.AUTO_LOAD);
			}
		}

		private void Processes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(Processes.SelectedIndex != -1)
			{
				Settings.userSettings.IGNORED_PROCESSES.Add(Processes.Items.GetItemAt(Processes.SelectedIndex).ToString());
				processIdRefresh += 1;
				timer_Tick(new object(), new EventArgs());
			}
		}

		private void ProcessesIgnored_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ProcessesIgnored.SelectedIndex != -1)
			{
				Settings.userSettings.IGNORED_PROCESSES.Remove(ProcessesIgnored.Items.GetItemAt(ProcessesIgnored.SelectedIndex).ToString());
				processIdRefresh += 1;
				timer_Tick(new object(), new EventArgs());
			}
		}
	}
}
