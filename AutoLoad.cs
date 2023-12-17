using Microsoft.Win32;
using System.Windows.Forms;

namespace WinSounds
{
	class AutoLoad
	{
        public static bool SetAutorunValue(bool autorun)
        {
            string executablePath = Application.ExecutablePath;
            var nameApp = "WinSounds";
            RegistryKey reg;
            reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (autorun)
            {
                reg.SetValue(nameApp, executablePath);
                reg.Close();
                return true;
            }
            else
            {
                if (reg.GetValue(nameApp) != null)
                {
                    reg.DeleteValue(nameApp);
                    reg.Close();
                    return true;
                }
                else
                    return false;
            }
        }
    }
}
