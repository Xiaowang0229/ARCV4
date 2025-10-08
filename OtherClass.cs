using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARCV4
{
    using ARCV4.Views;
    using ARCV4.Views.MainPages;
    using Hardcodet.Wpf.TaskbarNotification;
    using NAudio.Wave;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Threading;
    using Wpf.Ui.Controls;

    namespace YourNamespace
    {
        public class TrayManager
        {
            private TaskbarIcon _trayIcon;
            private MainWindow _mainWindow;

            public TrayManager()
            {
                _mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(); ;
                InitializeTray();
            }

            private void InitializeTray()
            {
                // 创建托盘图标
                _trayIcon = new TaskbarIcon
                {
                    Icon = App.ByteArrayToIcon(RootResources.Icon), // 替换成你的图标路径
                    ToolTipText = $"自动点名器 {App.ProgramVersion}"
                };

                _trayIcon.TrayLeftMouseUp += (s, e) => ShowMainWindow();
                // 创建菜单
                var menu = new System.Windows.Controls.ContextMenu();

                // 显示主窗口
                var showItem = new System.Windows.Controls.MenuItem
                {
                    Header = "显示主窗口"
                };
                showItem.Click += (s, e) => ShowMainWindow();
                menu.Items.Add(showItem);

                // 打开设置
                var settingsItem = new System.Windows.Controls.MenuItem
                {
                    Header = "打开设置"
                };
                settingsItem.Click += (s, e) => OpenSettings();
                menu.Items.Add(settingsItem);

                // 分割线
                menu.Items.Add(new System.Windows.Controls.Separator());

                // 退出程序
                var exitItem = new System.Windows.Controls.MenuItem
                {
                    Header = "退出程序"
                };
                exitItem.Click += (s, e) => ExitApplication();
                menu.Items.Add(exitItem);

                var aboutItem = new System.Windows.Controls.MenuItem
                {
                    Header = $"自动点名器 {App.ProgramVersion}"
                };
                aboutItem.Click += (s, e) => AboutClick();

                _trayIcon.ContextMenu = menu;
            }

            public void AboutClick()
            {
                _mainWindow.Show();
                _mainWindow.WindowState = WindowState.Normal;
                _mainWindow.Activate();
                _mainWindow.RootNavi.Navigate(typeof(About));
            }

            public void ShowMainWindow()
            {
                
                
                    _mainWindow.Show();
                    _mainWindow.WindowState = WindowState.Normal;
                    _mainWindow.Activate();
                
            }

            public void HideMainWindow()
            {
                _mainWindow?.Hide();
            }

            private void OpenSettings()
            {
                // 这里可以弹出设置窗口
                _mainWindow.Show();
                _mainWindow.WindowState = WindowState.Normal;
                _mainWindow.Activate();
                _mainWindow.RootNavi.Navigate(typeof(Settings));
            }

            private void ExitApplication()
            {
                _trayIcon.Dispose();
                Application.Current.Shutdown();
            }

            public void Dispose()
            {
                _trayIcon.Dispose();
            }
        }
    }

    public class AudioPlayer
    {
        private static IWavePlayer? outputDevice;
        private static AudioFileReader? audioFile;
        private static bool isLooping = false;

        public static async Task PlayLoopAsync(string mp3Path, string playStatus)
        {
            if (playStatus.ToLower() == "start")
            {
                // 先停止现有播放器
                Stop();

                isLooping = true;

                await Task.Run(() =>
                {
                    audioFile = new AudioFileReader(mp3Path);
                    outputDevice = new WaveOutEvent();

                    // 必须先 Init
                    outputDevice.Init(audioFile);

                    // 播放结束时重新播放实现循环
                    outputDevice.PlaybackStopped += (s, e) =>
                    {
                        if (isLooping && audioFile != null && outputDevice != null)
                        {
                            audioFile.Position = 0;
                            outputDevice.Play();
                        }
                    };

                    outputDevice.Play();
                });
            }
            else if (playStatus.ToLower() == "stop")
            {
                isLooping = false;
                Stop();
            }
        }

        private static void Stop()
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }
    }

    public static class GlobalKeyboardHook
    {
        public static event Action<Keys> KeyDown;
        public static event Action<Keys> KeyUp;

        private static IntPtr _hookId = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;

        private static IntPtr _mainWindowHandle = IntPtr.Zero;
        private static DispatcherTimer _checkTimer;
        private static bool _isActive = false; // 当前钩子状态

        public static void Start(IntPtr mainWindowHandle)
        {
            _mainWindowHandle = mainWindowHandle;

            _checkTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _checkTimer.Tick += (s, e) => CheckWindowState();
            _checkTimer.Start();
        }

        public static void Stop()
        {
            _checkTimer?.Stop();
            _checkTimer = null;
            RemoveHook();
        }

        private static void CheckWindowState()
        {
            IntPtr foreground = GetForegroundWindow();
            bool isForeground = (foreground == _mainWindowHandle);
            bool isMinimized = IsIconic(_mainWindowHandle);

            bool shouldBeActive = isForeground && !isMinimized;

            if (shouldBeActive && !_isActive)
            {
                _hookId = SetHook(_proc);
                _isActive = true;
            }
            else if (!shouldBeActive && _isActive)
            {
                RemoveHook();
                _isActive = false;
            }
        }

        private static void RemoveHook()
        {
            if (_hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;

            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                var key = (Keys)vkCode;

                if (wParam == (IntPtr)WM_KEYDOWN)
                    KeyDown?.Invoke(key);
                else if (wParam == (IntPtr)WM_KEYUP)
                    KeyUp?.Invoke(key);
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        #region Win32 API
        private const int WH_KEYBOARD_LL = 13;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);
        #endregion
    }
}
