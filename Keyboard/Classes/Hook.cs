using Gma.System.MouseKeyHook;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;
using static WinSounds.Keyboard.Classes.Hook;

namespace WinSounds.Keyboard.Classes
{
    class Hook
    {
        #region Libraries
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;

        private static LowLevelKeyboardProc _proc = hookProc;

        private static IntPtr hhook = IntPtr.Zero;
        #endregion

        public delegate void Function(int key);
        static Function function = default;

		public delegate void MouseFunction(MouseButtons e);
		static MouseFunction mouseFunction = default;

		private static IKeyboardMouseEvents m_GlobalHook;
		public static void Start(Function func, MouseFunction mouse)
		{
            function = func;
            mouseFunction = mouse;

			IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);

			m_GlobalHook = Gma.System.MouseKeyHook.Hook.GlobalEvents();
			m_GlobalHook.MouseDownExt += MoseClick;
			m_GlobalHook.MouseWheelExt += MoseClick;
		}
		private static void MoseClick(object sender, MouseEventExtArgs e)
		{
			mouseFunction(e.Button);
		}
		public static void Stop()
        {
            UnhookWindowsHookEx(hhook);
			m_GlobalHook.MouseDownExt -= MoseClick;
			m_GlobalHook.MouseWheelExt -= MoseClick;

			//It is recommened to dispose it
			m_GlobalHook.Dispose();
		}

        private static int lastParam = 0;
        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if ((code >= 0 && wParam == (IntPtr)0x101 || code >= 0 && wParam == (IntPtr)260) && (lastParam != 0x101 || Settings.userSettings.HOLDING))
                {
                    lastParam = 0x101;
                }
                else if ((code >= 0 && wParam == (IntPtr)0x100 || code >= 0 && wParam == (IntPtr)260) && (lastParam != 0x100 || Settings.userSettings.HOLDING))
				{
                    lastParam = 0x100;

					int vkCode = Marshal.ReadInt32(lParam);
					function(vkCode);
				}
                return IntPtr.Zero;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }
    }
    enum WindowHookEnum
    {
        OPEN = 0,
        CLOSE = 1,
    }
    static class WindowHook
    {
		public delegate void Function(WindowHookEnum key);
		static Function function = default;
        public static void Init(Function func)
        {
            function = func;

            Automation.AddAutomationEventHandler(
                eventId: WindowPattern.WindowOpenedEvent,
                element: AutomationElement.RootElement,
                scope: TreeScope.Subtree,
                eventHandler: OnWindowOpenOrClose);
            
            Automation.AddAutomationEventHandler(
                eventId: WindowPattern.WindowClosedEvent,
                element: AutomationElement.RootElement,
                scope: TreeScope.Subtree,
                eventHandler: OnWindowOpenOrClose);
        }
        public static void CloseHook()
        {
			Automation.RemoveAllEventHandlers();
		}
        private static void OnWindowOpenOrClose(object src, AutomationEventArgs e)
        {
            AutomationElement sourceElement;
            try
            {
                sourceElement = src as AutomationElement;
            }
            catch (ElementNotAvailableException)
            {
                return;
            }

            if (e.EventId == WindowPattern.WindowOpenedEvent)
            {
                try
                {
					function(WindowHookEnum.OPEN);

                    if (sourceElement != null)
                    {
                        Process execute = Process.GetProcessById(sourceElement.Current.ProcessId);
                        execute.EnableRaisingEvents = true;

                        execute.Exited += (s, e) => { function(WindowHookEnum.CLOSE); };
                    }
				}
				catch
                {

                }

				return;
            }
            //if (e.EventId == WindowPattern.WindowClosedEvent)
            //{
            //    //function(WindowHookEnum.CLOSE, -1);
            //    return;
            //}
        }
	}
}
