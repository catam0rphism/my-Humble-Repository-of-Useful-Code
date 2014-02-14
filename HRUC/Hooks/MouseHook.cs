using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HRUC.Hooks
{   
    /// <summary>
    /// Класс, реализующий низкоуровневый хук мыши
    /// </summary>
    public class MouseHook
    {
        public MouseHook()
        {
            Install();
        }
        ~MouseHook()
        {
            Uninstall();
        }

        private Native.HookHandler hookHandler;

        /// <summary>
        /// Сигнатура обработчика событий мыши
        /// </summary>
        public delegate void MouseHookCallback(Native.MSLLHOOKSTRUCT mouseStruct);
        public event MouseHookCallback LeftButtonDown;
        public event MouseHookCallback LeftButtonUp;
        public event MouseHookCallback RightButtonDown;
        public event MouseHookCallback RightButtonUp;
        public event MouseHookCallback MouseMove;
        public event MouseHookCallback MouseWheel;
        public event MouseHookCallback DoubleClick;
        public event MouseHookCallback MiddleButtonDown;
        public event MouseHookCallback MiddleButtonUp;

        private IntPtr hookID = IntPtr.Zero;

        public void Install()
        {
            hookHandler = HookFunc;
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                hookID = Native.SetWindowsHookEx(
                    idHook: (int)Native.HookType.MOUSE_LL,
                    lpfn: HookFunc,
                    hMod: Native.GetModuleHandle(module.ModuleName),
                    dwThreadId: 0);
            }
        }

        public void Uninstall()
        {
            if (hookID == IntPtr.Zero)
                return;

            Native.UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
       
        /// <summary>
        /// обработчик
        /// </summary>
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // parse system messages
            if (nCode >= 0)
            {
                if (Native.MouseMessages.WM_LBUTTONDOWN == (Native.MouseMessages)wParam)
                    if (LeftButtonDown != null)
                        LeftButtonDown((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_LBUTTONUP == (Native.MouseMessages)wParam)
                    if (LeftButtonUp != null)
                        LeftButtonUp((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_RBUTTONDOWN == (Native.MouseMessages)wParam)
                    if (RightButtonDown != null)
                        RightButtonDown((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_RBUTTONUP == (Native.MouseMessages)wParam)
                    if (RightButtonUp != null)
                        RightButtonUp((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_MOUSEMOVE == (Native.MouseMessages)wParam)
                    if (MouseMove != null)
                        MouseMove((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_MOUSEWHEEL == (Native.MouseMessages)wParam)
                    if (MouseWheel != null)
                        MouseWheel((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_LBUTTONDBLCLK == (Native.MouseMessages)wParam)
                    if (DoubleClick != null)
                        DoubleClick((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_MBUTTONDOWN == (Native.MouseMessages)wParam)
                    if (MiddleButtonDown != null)
                        MiddleButtonDown((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
                if (Native.MouseMessages.WM_MBUTTONUP == (Native.MouseMessages)wParam)
                    if (MiddleButtonUp != null)
                        MiddleButtonUp((Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.MSLLHOOKSTRUCT)));
            }
            return Native.CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}
