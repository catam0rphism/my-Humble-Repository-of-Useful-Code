using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HRUC.Hooks
{
    public static class Native
    {
        public enum KeyboardMessages
        {
            WM_KEYDOWN = 0x100,
            WM_SYSKEYDOWN = 0x104,
            WM_KEYUP = 0x101,
            WM_SYSKEYUP = 0x105
        }
        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public delegate IntPtr HookHandler(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookHandler lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public enum HookType : int
        {
            CALLWNDPROC = 4,
            CALLWNDPROCRET = 12,
            CBT = 5,
            DEBUG = 9,
            FOREGROUNDIDLE = 11,
            GETMESSAGE = 3,
            JOURNALPLAYBACK = 1,
            JOURNALRECORD = 0,
            KEYBOARD = 2,
            KEYBOARD_LL = 13,
            MOUSE = 7,
            MOUSE_LL = 14,
            MSGFILTER = -1,
            SHELL = 10,
            SYSMSGFILTER = 6
        }
    }
}
