using LenoApp.Models;
using LenoApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using HookUtil;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Forms.Clipboard;
using Keys = HookUtil.Keys;
using System.Threading;

namespace LenoApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string word;   //Comment
        public string _Word
        {
            get { return word; }
            set { SetProperty(ref word, value); }
        }

        private string temp { get; set; }

        private ObservableCollection<Word> wordList;   //Comment
        public ObservableCollection<Word> WordList
        {
            get { return wordList; }
            set { SetProperty(ref wordList, value); }
        }

        public Command SearchCommand { get; set; }

        public MainViewModel()
        {
            _hook = new UserActivityHook(true, true);
            _hook.OnMouseActivity += _hook_OnMouseActivity;
            _hook.KeyDown += _hook_KeyDown;

            WordList = new ObservableCollection<Word>();

            SearchCommand = new Command(() =>
            {
                SearchWord(_Word);
            }, () => { return true; });

        }

        private void SearchWord(string searchWord)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchWord)) { return; }

                searchWord = searchWord.TrimStart().TrimEnd().ToLower();

                if (temp == searchWord) { return; }

                _Word = temp = searchWord;
                WordList?.Clear();

                if (!Regex.IsMatch(searchWord, @"[a-z]+[\-\']?[a-z]*"))
                {
                    WordList?.Add(new Word() { word = searchWord, translation = "无释义" });
                    return;
                }

                string result = RestSharpService.Search(searchWord);
                Console.WriteLine(result);

                if (result == "[]")
                {
                    WordList?.Add(new Word() { word = searchWord, translation = "无释义" });
                }
                else
                {
                    List<Word> list = JsonConvert.DeserializeObject<List<Word>>(result);

                    if (list?.Count > 0)
                    {
                        list.ForEach(item => WordList.Add(item));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

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

        #region 钩子
        void _hook_KeyDown(object sender, CustomKeyEventArgs e)
        {
            //Console.WriteLine(String.Format(">> {0} - {1}{2}", e.Key, DateTime.Now.ToString("HH:mm:ss"), Environment.NewLine));

            if (e.Key == Keys.Enter)
            {
                SearchWord(_Word);
            }
        }

        void _hook_OnMouseActivity(object sender, CustomMouseEventArgs e)
        {
            //Console.WriteLine(String.Format(">> {0}{1} - {2}{3}", e.Button, e.Clicks, DateTime.Now.ToString("HH:mm:ss"), Environment.NewLine));

            if (e.Button == MouseButton.Left)
            {
                Thread.Sleep(100);

                keybd_event((byte)Keys.ControlKey, 0, 0, 0);//模拟按下ctrl
                keybd_event((byte)Keys.C, 0, 0, 0);//模拟按下c
                keybd_event((byte)Keys.ControlKey, 0, 2, 0);//模拟松开ctrl
                keybd_event((byte)Keys.C, 0, 2, 0);//模拟松开c

                Thread.Sleep(100);

                SearchWord(Clipboard.GetText());
                //Clipboard.Clear();
            }
        }

        public void Dispose()
        {
            _hook.Stop();
        }

        const uint CF_TEXT = 1;
        const uint CF_UNICODETEXT = 13;
        const uint CF_OEMTEXT = 7;

        /// <summary>
        /// Gets the data on the clipboard in the format specified by
        /// </summary>
        public static string GetClipboardData()
        {
            OpenClipboard(IntPtr.Zero);
            //Get pointer to clipboard data in the selected format
            IntPtr ClipboardDataPointer = GetClipboardData(CF_TEXT);

            //Do a bunch of crap necessary to copy the data from the memory
            //the above pointer points at to a place we can access it.
            IntPtr Length = GlobalSize(ClipboardDataPointer);
            IntPtr gLock = GlobalLock(ClipboardDataPointer);

            //Init a buffer which will contain the clipboard data
            byte[] Buffer = new byte[(int)Length];

            //Copy clipboard data to buffer
            Marshal.Copy(gLock, Buffer, 0, (int)Length);
            CloseClipboard();

            return System.Text.Encoding.Default.GetString(Buffer);

        }
        #endregion
    }

}
