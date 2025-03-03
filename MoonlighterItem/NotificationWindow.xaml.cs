using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace MoonlighterItem
{
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string message)
        {
            InitializeComponent();
            NotificationText.Text = message;

            // Анимация плавного появления
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            this.BeginAnimation(Window.OpacityProperty, fadeIn);

            // Автоматическое закрытие через 3 секунды с анимацией
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, args) =>
            {
                CloseNotification();
                timer.Stop();
            };
            timer.Start();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseNotification();
        }

        private void CloseNotification()
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOut.Completed += (s, e) => this.Close();
            this.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}
