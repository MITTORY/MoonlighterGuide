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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoonlighterItem
{
    /// <summary>
    /// Логика взаимодействия для InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void EmailLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Текст для копирования
            string email = "novo1233@mail.ru";

            // Копируем текст в буфер обмена
            Clipboard.SetText(email);

            // Создаем и показываем уведомление
            NotificationWindow notification = new NotificationWindow("Email скопирован в буфер обмена!");
            notification.Show();

            // Закрываем уведомление через 2 секунды
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, ev) =>
            {
                notification.Close();
                timer.Stop();
            };
            timer.Start();
        }

        private void LicenseLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://opensource.org/licenses/MIT");
        }

        private void GitHubLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/MITTORY");
        }
    }
}
