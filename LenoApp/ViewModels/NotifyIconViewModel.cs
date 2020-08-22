using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LenoApp.ViewModels
{
    public class NotifyIconViewModel : BaseViewModel
    {
        public Command ShowWindowCommand { get; set; }
        public Command HideWindowCommand { get; set; }
        public Command ExitApplicationCommand { get; set; }

        public NotifyIconViewModel()
        {
            ShowWindowCommand = new Command(() =>
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
            }, () => { return true; });

            HideWindowCommand = new Command(() =>
            {
                Application.Current.MainWindow.Hide();
            }, () => { return true; });

            ExitApplicationCommand = new Command(() =>
            {
                Application.Current.Shutdown();
            }, () => { return true; });

        }
    }
}
