using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HRUC.Hooks
{
    /// <summary>
    /// Класс, реализующий низкоуровневый хук клавиатуры
    /// </summary>
    public class KeyboardHook
    {
        /// <summary>
        /// Конструктор, инициализирует хук
        /// </summary>
        public KeyboardHook()
        {
            Install();
        }

        /// <summary>
        /// Деструктор, удаляет хук
        /// </summary>
        ~KeyboardHook()
        {
            Uninstall();
        }

        private Native.HookHandler hookHandler;
        
        /// <summary>
        /// Сигнатура обработчика событий клавиатуры
        /// </summary>
        /// <param name="key">Код клавиши</param>
        public delegate void KeyboardHookCallback(ConsoleKey key);
        public event KeyboardHookCallback KeyDown;
        public event KeyboardHookCallback KeyUp;

        private IntPtr hookID = IntPtr.Zero;

        /// <summary>
        /// Устанавливает хук клавиатуры
        /// </summary>
        public void Install()
        {
            hookHandler = HookFunc;
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                hookID = Native.SetWindowsHookEx(
                    idHook: (int)Native.HookType.KEYBOARD_LL,
                    lpfn: HookFunc,
                    hMod: Native.GetModuleHandle(module.ModuleName),
                    dwThreadId: 0);
            }
        }

        /// <summary>
        /// Удаляет установленный хук
        /// </summary>
        public void Uninstall()
        {
            Native.UnhookWindowsHookEx(hookID);
        }

        /// <summary>
        /// Обработчик
        /// </summary>
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {   
            if (nCode >= 0)
            {
                int iwParam = wParam.ToInt32();

                if ((iwParam == (int)Native.KeyboardMessages.WM_KEYDOWN || iwParam == (int)Native.KeyboardMessages.WM_SYSKEYDOWN))
                    if (KeyDown != null)
                        KeyDown((ConsoleKey)Marshal.ReadInt32(lParam));
                if ((iwParam == (int)Native.KeyboardMessages.WM_KEYUP || iwParam == (int)Native.KeyboardMessages.WM_SYSKEYUP))
                    if (KeyUp != null)
                        KeyUp((ConsoleKey)Marshal.ReadInt32(lParam));
            }

            return Native.CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}
