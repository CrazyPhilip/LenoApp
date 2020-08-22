using HookUtil;
using SourceChord.FluentWPF;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Forms.Clipboard;
using Keys = HookUtil.Keys;

namespace LenoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AcrylicWindow
    {
        /*
        WindowState ws;
        WindowState wsl;
        NotifyIcon notifyIcon;

        private UserActivityHook _hook;

        /// <summary>
        /// 导入模拟键盘的方法
        /// </summary>
        /// <param name="bVk" >按键的虚拟键值</param>
        /// <param name= "bScan" >扫描码，一般不用设置，用0代替就行</param>
        /// <param name= "dwFlags" >选项标志：0：表示按下，2：表示松开</param>
        /// <param name= "dwExtraInfo">一般设置为0</param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalSize(IntPtr handle);

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseClipboard();

        const uint CF_TEXT = 1;
        const uint CF_UNICODETEXT = 13;
        const uint CF_OEMTEXT = 7;
        */

        public MainWindow()
        {
            InitializeComponent();

            /*
            //NotifyIcon
            icon();

            //ContextMenuStrip
            contextMenu();

            //保证窗体显示在上方。 
            wsl = WindowState;

            _hook = new UserActivityHook(true, false);
            _hook.OnMouseActivity += _hook_OnMouseActivity;
            _hook.KeyDown += _hook_KeyDown;
            */
        }

        private void AcrylicWindow_Closed(object sender, System.EventArgs e)
        {
            mainViewModel.Dispose();
        }

        private void AcrylicWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
        
        /*
        #region 托盘右键菜单
        private void icon()
        {
            string path = Path.GetFullPath(@"Icons\leno.ico");
            if (File.Exists(path))
            {
                this.notifyIcon = new NotifyIcon();
                this.notifyIcon.BalloonTipText = "Hello, Leno!"; //设置程序启动时显示的文本
                this.notifyIcon.Text = "Leno";//最小化到托盘时，鼠标点击时显示的文本
                Icon icon = new Icon(path);//程序图标 
                this.notifyIcon.Icon = icon;
                this.notifyIcon.Visible = true;
                notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
                this.notifyIcon.ShowBalloonTip(1000);
            }

        }

        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }

        private void Window_StateChanged_1(object sender, EventArgs e)
        {
            ws = this.WindowState;
            if (ws == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void contextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            //关联 NotifyIcon 和 ContextMenuStrip
            notifyIcon.ContextMenuStrip = cms;

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem { Text = "退出" };
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            ToolStripMenuItem hideMenumItem = new ToolStripMenuItem { Text = "隐藏" };
            hideMenumItem.Click += new EventHandler(hideMenumItem_Click);

            ToolStripMenuItem showMenuItem = new ToolStripMenuItem { Text = "显示" };
            showMenuItem.Click += new EventHandler(showMenuItem_Click);

            cms.Items.Add(exitMenuItem);
            cms.Items.Add(hideMenumItem);
            cms.Items.Add(showMenuItem);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            System.Windows.Application.Current.Shutdown();
        }

        private void hideMenumItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }
        #endregion

        #region 钩子
        void _hook_KeyDown(object sender, CustomKeyEventArgs e)
        {
            Console.WriteLine(String.Format(">> {0} - {1}{2}", e.Key, DateTime.Now.ToString("HH:mm:ss"), Environment.NewLine));
          
        }

        void _hook_OnMouseActivity(object sender, CustomMouseEventArgs e)
        {
            Console.WriteLine(String.Format(">> {0}{1} - {2}{3}", e.Button, e.Clicks, DateTime.Now.ToString("HH:mm:ss"), Environment.NewLine));

            if (e.Button == MouseButton.Left)
            {
                keybd_event((byte)Keys.ControlKey, 0, 0, 0);//模拟按下ctrl
                keybd_event((byte)Keys.C, 0, 0, 0);//模拟按下c
                keybd_event((byte)Keys.ControlKey, 0, 2, 0);//模拟松开ctrl
                keybd_event((byte)Keys.C, 0, 2, 0);//模拟松开c

                //Console.WriteLine(Clipboard.GetText());
                textBox.Text = Clipboard.GetText();
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _hook.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _hook.Stop();
        }
        #endregion
        */
    }
}
        