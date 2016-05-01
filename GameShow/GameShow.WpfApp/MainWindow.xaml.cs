using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameShow.Http;

namespace GameShow.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowContext.Current.MainWindow = this;
            Logging.OnLogMessage += OnLogMessage;
            InitHttpServer();
            InitShowWindow();
            this.Closing += OnClosing;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // for testing
            activityOperatorControl.LoadActivity(new Activities.GenericText.GenericTextActivity());
        }

        private List<string> _logMsgs = new List<string>();
        private void OnLogMessage(string source, string message)
        {
            if (System.Threading.Thread.CurrentThread != this.Dispatcher.Thread)
            {
                this.Dispatcher.Invoke(() => { OnLogMessage(source, message); });
                return;
            }

            _logMsgs.Add($"{source}: {message}");
            while (_logMsgs.Count > 100)
            {
                _logMsgs.RemoveAt(0);
            }

            txtLogMessages.Text = string.Join("\n", _logMsgs.ToArray());
            txtLogMessages.ScrollToEnd();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_showWindow != null)
            {
                _showWindow.Close();
            }
        }

        private void InitHttpServer()
        {
            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                try
                {
                    GsHttpServer.Start(1234);
                }
                catch
                {
                }
            });
        }

        private void Log(string msg)
        {
            Logging.LogMessage("App", msg);
        }
        
        private ShowWindow _showWindow = null;
        private void InitShowWindow()
        {
            if (_showWindow == null)
            {
                ShowContext.Current.ScreenCount = Screen.AllScreens.Count();
                Log($"{ShowContext.Current.ScreenCount} screen(s) detected");
                if (ShowContext.Current.ScreenCount <= 1)
                {
                    Log($"Disabling show window due to 1 or less screens present");
                }
                else
                {
                    ShowContext.Current.ShowScreenIndex = Screen.AllScreens.Length - 1;
                }

                _showWindow = new ShowWindow();

                System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
                {
                    while (true)
                    {
                        try
                        {
                            this.Dispatcher.Invoke(RefreshShowWindow);
                        } catch { }
                        System.Threading.Thread.Sleep(1000);
                    }
                });
            }

            RefreshShowWindow();
        }

        private void RefreshShowWindow()
        {
            if (!ShowContext.Current.ShowScreenIndex.HasValue)
            {
                _showWindow.Hide();
                return;
            }

            _showWindow.Show();
            _showWindow.WindowState = WindowState.Normal;
            _showWindow.WindowStyle = WindowStyle.None;
            _showWindow.ShowInTaskbar = false;
            var targetScreen = Screen.AllScreens[ShowContext.Current.ShowScreenIndex.Value];
            var screenBounds = targetScreen.Bounds;
            _showWindow.Top = screenBounds.Top;
            _showWindow.Left = screenBounds.Left;
            _showWindow.Width = screenBounds.Width;
            _showWindow.Height = screenBounds.Height;
            _showWindow.Topmost = true;
        }

        private void OnGameNameTextChanged(object sender, TextChangedEventArgs e)
        {
            ShowContext.Current.GameName = txtGameName.Text;
        }
    }
}
