using System.Windows;

namespace MoonlighterItem
{
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string message)
        {
            InitializeComponent();
            NotificationText.Text = message;
        }
    }
}