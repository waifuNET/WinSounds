using Microsoft.Win32;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;

namespace WinSounds
{
	class AutoLoad
	{
		public const int WTS_CURRENT_SERVER_HANDLE = 0;
		public const int WTS_CURRENT_SESSION = -1;

		[DllImport("WTSApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool WTSSendMessage(IntPtr hServer, int SessionId, string pTitle, int TitleLength, string pMessage, int MessageLength, int Style, int Timeout, out int pResponse, Boolean bWait);

		[DllImport("WTSApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool WTSEnumerateSessions(IntPtr hServer, int Reserved, int Version, out IntPtr ppSessionInfo, out int pCount);

		[DllImport("WTSApi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void WTSFreeMemory(IntPtr pMemory);

		[DllImport("WTSApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool WTSQuerySessionInformation(IntPtr hServer, int SessionId, WTS_INFO_CLASS WTSInfoClass, out IntPtr ppBuffer, out uint BytesReturned);

		public enum WTS_INFO_CLASS
		{
			WTSInitialProgram,
			WTSApplicationName,
			WTSWorkingDirectory,
			WTSOEMId,
			WTSSessionId,
			WTSUserName,
			WTSWinStationName,
			WTSDomainName,
			WTSConnectState,
			WTSClientBuildNumber,
			WTSClientName,
			WTSClientDirectory,
			WTSClientProductId,
			WTSClientHardwareId,
			WTSClientAddress,
			WTSClientDisplay,
			WTSClientProtocolType,
			WTSIdleTime,
			WTSLogonTime,
			WTSIncomingBytes,
			WTSOutgoingBytes,
			WTSIncomingFrames,
			WTSOutgoingFrames,
			WTSClientInfo,
			WTSSessionInfo,
			WTSSessionInfoEx,
			WTSConfigInfo,
			WTSValidationInfo,
			WTSSessionAddressV4,
			WTSIsRemoteSession
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WTS_SESSION_INFO
		{
			public int SessionId;             // session id                                                                                                                                                   
			public string pWinStationName;      // name of WinStation this session is connected to                                                                                                            
			public WTS_CONNECTSTATE_CLASS State; // connection state (see enum)                                                                                                                               
		}

		public enum WTS_CONNECTSTATE_CLASS
		{
			WTSActive,              // User logged on to WinStation                                                                                                                                           
			WTSConnected,           // WinStation connected to client                                                                                                                                         
			WTSConnectQuery,        // In the process of connecting to client                                                                                                                                 
			WTSShadow,              // Shadowing another WinStation                                                                                                                                           
			WTSDisconnected,        // WinStation logged on without client                                                                                                                                    
			WTSIdle,                // Waiting for client to connect                                                                                                                                          
			WTSListen,              // WinStation is listening for connection                                                                                                                                 
			WTSReset,               // WinStation is being reset                                                                                                                                              
			WTSDown,                // WinStation is down due to error                                                                                                                                        
			WTSInit,                // WinStation in initialization                                                                                                                                           
		}

		public static string SessionCheck()
		{
			IntPtr pSessions = IntPtr.Zero;
			int nSessions;
			if (WTSEnumerateSessions((IntPtr)WTS_CURRENT_SERVER_HANDLE, 0, 1, out pSessions, out nSessions))
			{
				int nDataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
				IntPtr pCurrentSession = pSessions;
				for (int Index = 0; Index < nSessions; Index++)
				{
					WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure(pCurrentSession, typeof(WTS_SESSION_INFO));
					if (si.State == WTS_CONNECTSTATE_CLASS.WTSActive || si.State == WTS_CONNECTSTATE_CLASS.WTSConnected)
					{
						uint nBytesReturned = 0;
						IntPtr pUserName = IntPtr.Zero;
						bool bRet = WTSQuerySessionInformation((IntPtr)WTS_CURRENT_SERVER_HANDLE, si.SessionId, WTS_INFO_CLASS.WTSUserName, out pUserName, out nBytesReturned);
						string sUserName = Marshal.PtrToStringUni(pUserName);
						return sUserName;
					}
					pCurrentSession += nDataSize;
				}
				WTSFreeMemory(pSessions);
			}
			return null;
		}

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
