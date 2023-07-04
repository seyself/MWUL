using System;
using System.Runtime.InteropServices;
using UnityEngine;

// ref https://gist.github.com/mattatz/ca84b487c5697e7d43f8216c57a2b975
// ref http://answers.unity3d.com/questions/13523/is-there-a-way-to-set-the-position-of-a-standalone.html
// ref http://stackoverflow.com/questions/2825528/removing-the-title-bar-of-external-application-using-c-sharp
namespace App 
{
    public class WindowControl
    {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(System.String className, System.String windowName);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // assorted constants needed
        public static int GWL_STYLE = -16;
        public static int WS_CHILD = 0x40000000; //child window
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar

        public static void SetPosition(int x, int y) 
        {
            SetPosition(x, y, Screen.width, Screen.height, true);
        }

        public static void SetPosition(int x, int y, int width, int height, bool hideTitleBar) 
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
            string procName = proc.ProcessName;
            SetPosition(procName, x, y, width, height, hideTitleBar);
        }

        public static void SetPosition(string windowName, int x, int y, int width, int height, bool hideTitleBar) 
        {
            var window = FindWindow(null, windowName);
            if(hideTitleBar) 
            {
                int style = GetWindowLong(window, GWL_STYLE);
                SetWindowLong(window, GWL_STYLE, (style & ~WS_CAPTION));
            }
            SetWindowPos(window, 0, x, y, width, height, width * height == 0 ? 1 : 0);
        }
#else 
        public static void SetPosition(string windowName, int x, int y, int width, int height, bool hideTitleBar) 
        {

        }
#endif

    }
}
