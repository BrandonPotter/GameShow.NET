using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameShow.WpfApp
{
    /// <summary>
    /// Interaction logic for ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : Window
    {
        public ShowWindow()
        {
            InitializeComponent();
            System.Threading.ThreadPool.QueueUserWorkItem((obj) => { UpdateScreen(); });
            ShowContext.Current.ShowWindow = this;
        }

        private void UpdateScreen()
        {
            while (true)
            { 
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        UpdateScreenControlsMainThread();
                    }
                    catch
                    {
                    }
                });
                System.Threading.Thread.Sleep(250);
            }
        }

        private void UpdateScreenControlsMainThread()
        {
            txtGameTitle.Text = ShowContext.Current.GameName;
        }
        
        private bool _animationInProgress = false;
        private Viewbox _activeContentViewbox = null;
        private Viewbox _standbyContentViewbox = null;
        public void SlideToContent(UserControl toUserControl, TimeSpan? animationDuration)
        {
            _animationInProgress = true;

            if (_activeContentViewbox == null && _standbyContentViewbox == null)
            {
                _activeContentViewbox = vbContent1;
                _standbyContentViewbox = vbContent2;
            }

            this.Focus();
            _standbyContentViewbox.Width = this.ActualWidth;
            _standbyContentViewbox.Margin = new Thickness(this.ActualWidth, 0, 0, 0);
            _standbyContentViewbox.Visibility = System.Windows.Visibility.Visible;
            var previousChild = _standbyContentViewbox.Child;

            _standbyContentViewbox.Child = toUserControl;

            var activeControl = _activeContentViewbox.Child as UserControl;

            if (activeControl != null)
            {
                activeControl.IsEnabled = false;
            }

            previousChild = null;            

            Duration duration = new Duration(animationDuration.GetValueOrDefault(TimeSpan.FromSeconds(0.5)));

            ThicknessAnimation marginAnimationStandby = new ThicknessAnimation();
            marginAnimationStandby.Duration = duration;
            marginAnimationStandby.From = new Thickness(this.ActualWidth, 0, 0, 0);
            marginAnimationStandby.To = new Thickness(0, 0, 0, 0);
            marginAnimationStandby.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };

            ThicknessAnimation marginAnimationActive = new ThicknessAnimation();
            marginAnimationActive.Duration = duration;
            marginAnimationActive.From = new Thickness(0, 0, 0, 0);
            marginAnimationActive.To = new Thickness(0 - this.ActualWidth, 0, this.ActualWidth, 0);
            marginAnimationActive.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };

            Storyboard sb = new Storyboard();
            sb.Duration = duration;

            sb.Children.Add(marginAnimationStandby);
            sb.Children.Add(marginAnimationActive);

            Storyboard.SetTarget(marginAnimationStandby, _standbyContentViewbox);
            Storyboard.SetTargetProperty(marginAnimationStandby, new PropertyPath(Viewbox.MarginProperty));
            Storyboard.SetTarget(marginAnimationActive, _activeContentViewbox);
            Storyboard.SetTargetProperty(marginAnimationActive, new PropertyPath(Viewbox.MarginProperty));

            // Begin the animation.
            sb.Begin();

            var newActive = _standbyContentViewbox;
            var newStandby = _activeContentViewbox;

            _activeContentViewbox = newActive;
            _standbyContentViewbox = newStandby;

            sb.Completed += OnStoryboardCompleted;
        }

        void OnStoryboardCompleted(object sender, EventArgs e)
        {
            _animationInProgress = false;
        }
    }
}
