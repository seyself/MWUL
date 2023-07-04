using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

// ref http://pinvoke.net/default.aspx/user32.EnumDisplayMonitors
// ref https://tutorialmore.com/questions-174756.htm
namespace App 
{
    // ディスプレイの情報を取得する
    public class DisplayInfo 
    {
        public string DeviceName { get; private set; }
        public string Availability { get; private set; }
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }
        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }
        public Rect MonitorArea { get; private set; }
        public Rect WorkArea { get; private set; }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public class DisplayInfoCollection : List<DisplayInfo> {}

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);
        delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct MONITORINFOEX
        {
            public uint Size;
            public Rect Monitor;
            public Rect WorkArea;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
        }

        public static List<DisplayInfo> GetDisplayList()
        {
            List<DisplayInfo> col = new List<DisplayInfo>();
            EnumDisplayMonitors( IntPtr.Zero, IntPtr.Zero,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor,  IntPtr dwData)
                    {
                        MONITORINFOEX mi = new MONITORINFOEX();
                        mi.Size = (uint)Marshal.SizeOf(mi);
                        bool success = GetMonitorInfo(hMonitor, ref mi);
                        if (success)
                        {
                            DisplayInfo di = new DisplayInfo();
                            di.ScreenWidth = mi.Monitor.right - mi.Monitor.left;
                            di.ScreenHeight = mi.Monitor.bottom - mi.Monitor.top;
                            di.ScreenX = mi.Monitor.left;
                            di.ScreenY = mi.Monitor.top;
                            di.DeviceName = mi.DeviceName;
                            di.MonitorArea = mi.Monitor;
                            di.WorkArea = mi.WorkArea;
                            di.Availability = mi.Flags.ToString();
                            col.Add(di);
                        }
                        return true;
                    }, IntPtr.Zero );
            return col;
        }
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        public static List<DisplayInfo> GetDisplayList()
        {
            return new List<DisplayInfo>();
        }
#else
        public static List<DisplayInfo> GetDisplayList()
        {
            return new List<DisplayInfo>();
        }
#endif

    }
}