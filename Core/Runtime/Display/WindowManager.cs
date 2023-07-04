using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace App 
{
    public class WindowManager
    {

#if UNITY_EDITOR_WIN
        public static string GetCurrentWindowTitle()
        {
            var list = WindowManager.GetWindowTitleList(Application.productName + ".* Unity ");
            string windowName = "";
            foreach(var name in list)
            {
                windowName = name;
                break;
            }
            return windowName;
        }
#else 
        public static string GetCurrentWindowTitle()
        {
            return Application.productName;
        }
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

        public static List<string> GetWindowTitleList()
        {
            List<string> windows = new List<string>();
            EnumWindows(delegate (IntPtr hWnd, IntPtr lparam)
                {
                    int textLen = GetWindowTextLength(hWnd);
                    if (0 < textLen)
                    {
                        StringBuilder tsb = new StringBuilder(textLen + 1);
                        GetWindowText(hWnd, tsb, tsb.Capacity);
                        windows.Add(tsb.ToString());
                    }
                    return true;
                }, IntPtr.Zero);
            return windows;
        }

        public static List<string> GetWindowTitleList(string findTitlePattern)
        {
            List<string> windows = new List<string>();
            Regex re = new Regex(findTitlePattern);
            EnumWindows(delegate (IntPtr hWnd, IntPtr lparam)
                {
                    int textLen = GetWindowTextLength(hWnd);
                    if (0 < textLen)
                    {
                        StringBuilder tsb = new StringBuilder(textLen + 1);
                        GetWindowText(hWnd, tsb, tsb.Capacity);
                        string title = tsb.ToString();
                        if (re.IsMatch(title))
                        {
                            windows.Add(title);
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            return windows;
        }

        delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

#else 
        public static List<string> GetWindowTitleList(string findTitlePattern)
        {
            return new List<string>();
        }

        public static List<string> GetWindowTitleList()
        {
            return new List<string>();
        }
#endif
    }
}
