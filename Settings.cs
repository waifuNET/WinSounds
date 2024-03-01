using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinSounds
{
	public class Mood
	{
		public string PATH { get; set; }
		public string NAME { get; set; }
		public string DESCRIPTION { get; set; }
		public string VERSION { get; set; }
		public List<string> CLICK { get; set; }
		public List<string> MOUSE_WHEEL { get; set; }
		public List<string> WINDOW_CLOSE { get; set; }
		public List<string> WINDOW_OPEN { get; set; }
		public List<string> TYPING_BACKSPACE { get; set; }
		public List<string> TYPING_ENTER { get; set; }
		public List<string> TYPING_LETTER { get; set; }
		public List<string> TYPING_SPACE { get; set; }
	}
	class SoundModel
	{
		public WaveFileReader reader { get; set; }
		public WaveOut waveOut { get; set; }

		public SoundModel(string filePath)
		{
			reader = new WaveFileReader(filePath);
			waveOut = new WaveOut();

			waveOut.Init(reader);
		}
		public void Play()
		{
			reader.Seek(0, SeekOrigin.Begin);
			waveOut.Volume = Settings.userSettings.VOLUME;
			waveOut.Play();
		}
		public void Stop()
		{
			waveOut.Stop();
			reader.Seek(0, SeekOrigin.Begin);
		}
		public void Clear()
		{
			waveOut.Stop();
			waveOut.Dispose();

			reader.Close();
			reader.Dispose();
		}
	}
	public class UserSettings
	{
		public string CURRENT_MOOD { get; set; }
		public bool AUTO_LOAD { get; set; }
		public bool SOLO_TRACK { get; set; }

		public bool CLICK { get; set; }
		public bool MOUSE_WHEEL { get; set; }
		public bool WINDOW_CLOSE { get; set; }
		public bool WINDOW_OPEN { get; set; }
		public bool TYPING_BACKSPACE { get; set; }
		public bool TYPING_ENTER { get; set; }
		public bool TYPING_LETTER { get; set; }
		public bool TYPING_SPACE { get; set; }
		public float VOLUME { get; set; }
		public bool HOLDING { get; set; }
		public List<string> IGNORED_PROCESSES { get; set; }
	}
	static class Settings
	{
		public static bool MUTE = false;

		public static List<string> ignored_proc = new List<string>() { "Idle", "svhost", "WinSounds", "System", "Secure System", "Registry", "smss", "csrss" };
		public static UserSettings userSettings = new UserSettings();

		public static string APP_PATH = AppDomain.CurrentDomain.BaseDirectory;
		public static string MOOD_PATH = Path.Join(APP_PATH, "Moods");

		public static Mood currentMood = new Mood();
		public static List<Mood> moods = new List<Mood>();
		public static Dictionary<string, SoundModel> currentMoodSounds = new Dictionary<string, SoundModel>();

		//public static Mood mood = new Mood()
		//{
		//	NAME = "Shylily: Womp Womp",
		//	DESCRIPTION = "A Shylily themed mod | Modified 4.1a Version",
		//	VERSION = "5.2",

		//	CLICK = new List<string> { "sounds/click3.wav", "sounds/click5.wav", "sounds/click6.wav", "sounds/click4.wav", "sounds/click2.wav", "sounds/click9.wav", "sounds/click7.wav", "sounds/click1.wav", "sounds/click11.wav", "sounds/click10.wav", "sounds/click8.wav" },
		//	MOUSE_WHEEL = new List<string> { "sounds/wheel6.wav", "sounds/wheel1.wav", "sounds/wheel5.wav", "sounds/wheel3.wav", "sounds/wheel4.wav", "sounds/wheel2.wav", "sounds/wheel7.wav" },
		//	WINDOW_CLOSE = new List<string> { "sounds/close1.wav", "sounds/close2.wav", "sounds/close6.wav", "sounds/close7.wav", "sounds/close3.wav", "sounds/close8.wav", "sounds/close9.wav", "sounds/close4.wav", "sounds/close5.wav" },
		//	WINDOW_OPEN = new List<string> { "sounds/insert2.wav", "sounds/insert1.wav", "sounds/insert4.wav", "sounds/insert3.wav" },
		//	TYPING_BACKSPACE = new List<string> { "keyboard/backspace3.wav", "keyboard/backspace2.wav", "keyboard/backspace4.wav", "keyboard/backspace1.wav" },
		//	TYPING_ENTER = new List<string> { "keyboard/enter3.wav", "keyboard/enter2.wav", "keyboard/enter5.wav", "keyboard/enter4.wav", "keyboard/enter1.wav" },
		//	TYPING_LETTER = new List<string> { "keyboard/letter8.wav", "keyboard/letter17.wav", "keyboard/letter15.wav", "keyboard/letter5.wav", "keyboard/letter18.wav", "keyboard/letter4.wav", "keyboard/letter3.wav", "keyboard/letter13.wav", "keyboard/letter19.wav", "keyboard/letter14.wav", "keyboard/letter7.wav", "keyboard/letter16.wav", "keyboard/letter1.wav", "keyboard/letter21.wav", "keyboard/letter22.wav", "keyboard/letter6.wav", "keyboard/letter11.wav", "keyboard/letter20.wav", "keyboard/letter12.wav", "keyboard/letter10.wav", "keyboard/letter2.wav", "keyboard/letter9.wav" },
		//	TYPING_SPACE = new List<string> { "keyboard/space1.wav", "keyboard/space4.wav", "keyboard/space2.wav", "keyboard/space3.wav", "keyboard/space5.wav" },
		//};
		//public static void CreateJson()
		//{
		//	using (StreamWriter file = File.CreateText("./save.json"))
		//	{
		//		JsonSerializer serializer = new JsonSerializer();
		//		serializer.Serialize(file, mood);
		//	}
		//}

		public static void SaveSettings()
		{
			using (StreamWriter file = File.CreateText("./UserSettings.json"))
			{
				userSettings.CURRENT_MOOD = currentMood.PATH;
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, userSettings);
			}
		}

		public static void LoadSettings()
		{
			if (!File.Exists("./UserSettings.json"))
			{
				userSettings = new UserSettings { MOUSE_WHEEL = false, CURRENT_MOOD = currentMood.PATH, CLICK = false, SOLO_TRACK = false, TYPING_BACKSPACE = true, TYPING_ENTER = true, TYPING_LETTER = true, TYPING_SPACE = true, VOLUME = 1, HOLDING = false, WINDOW_CLOSE = true, WINDOW_OPEN = true, AUTO_LOAD = true, IGNORED_PROCESSES = new List<string>() };
				SaveSettings();
			}

			string jsonFile = "";
			using (FileStream fstream = File.OpenRead("./UserSettings.json"))
			{
				byte[] buffer = new byte[fstream.Length];
				fstream.Read(buffer, 0, buffer.Length);
				jsonFile = Encoding.Default.GetString(buffer);
			}

			userSettings = JsonConvert.DeserializeObject<UserSettings>(jsonFile);
			AutoLoad.SetAutorunValue(userSettings.AUTO_LOAD);
			if(userSettings.IGNORED_PROCESSES == null) {
				userSettings.IGNORED_PROCESSES = new List<string>();
			}
		}

		public static void LoadCurrentMood()
		{
			foreach(KeyValuePair<string, SoundModel> mood in currentMoodSounds)
			{
				mood.Value.Clear();
			}
			currentMoodSounds.Clear();
			List<List<string>> sounds = new List<List<string>> {
				currentMood.CLICK,
				currentMood.MOUSE_WHEEL,
				currentMood.WINDOW_CLOSE,
				currentMood.WINDOW_OPEN,
				currentMood.TYPING_BACKSPACE,
				currentMood.TYPING_ENTER,
				currentMood.TYPING_LETTER,
				currentMood.TYPING_SPACE,
			};

			foreach (List<string> s in sounds)
			{
				if (s != null && s.Count > 0)
					foreach (string v in s)
					{
						try
						{
							string file = Path.Join(Settings.currentMood.PATH, v);
							if (!File.Exists(file)) continue;
							currentMoodSounds.Add(v, new SoundModel(file));
						}
						catch(Exception ex)
						{
							MessageBox.Show(ex.Message);
							continue;
						}
					}
			}

		}

		public static void Init()
		{
			//CreateJson();
			LoadSettings();
			LoadMoods();
			SetCurrentMood(userSettings.CURRENT_MOOD);
		}
		private static List<string> loadedMoods = new List<string>();
		public static void LoadMoods()
		{
			List<string> temp_moods = new List<string>();
			string[] pathMoods = Directory.GetDirectories(APP_PATH + @"\Moods");
			foreach (string i in pathMoods)
			{
				if (loadedMoods.IndexOf(i) == -1) temp_moods.Add(i);
				else
				{
					//foreach (Mood m in moods)
					//{
					//	if (m.NAME == i) moods.Remove(m);
					//}
				}
			}
			pathMoods = temp_moods.ToArray();
			for (int i = 0; i < pathMoods.Length; i++)
			{
				pathMoods[i] = pathMoods[i].Replace(@".\", String.Empty);
			}
			foreach (string moodPath in pathMoods)
			{
				string moodFile = Path.Join(moodPath, "Mood.json");
				if (!File.Exists(moodFile))
				{
					continue;
				}

				string jsonFile = "";
				using (FileStream fstream = File.OpenRead(moodFile))
				{
					byte[] buffer = new byte[fstream.Length];
					fstream.Read(buffer, 0, buffer.Length);
					jsonFile = Encoding.Default.GetString(buffer);
				}

				Mood tempMood = JsonConvert.DeserializeObject<Mood>(jsonFile);
				tempMood.PATH = moodPath;

				moods.Add(tempMood);
				loadedMoods.Add(moodPath);
			}
		}
		public static Mood GetMood(int id)
		{
			if (moods.Count > id && id >= 0)
			{
				return moods[id];
			}
			return null;
		}
		public static void SetCurrentMood(int id)
		{
			if (moods.Count > id && id >= 0)
			{
				currentMood = moods[id];
				LoadCurrentMood();
			}
		}
		public static void SetCurrentMood(string PATH)
		{
			foreach (Mood m in moods)
			{
				if (m.PATH == PATH)
				{
					currentMood = m;
					LoadCurrentMood();
				}
			}
		}
		public static void SetCurrentMood(Mood mood)
		{
			currentMood = mood;
			LoadCurrentMood();
		}
	}
}
