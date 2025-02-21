using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoonlighterItem
{
    public partial class MainWindow : Window
    {
        private bool _isDragging = false;
        private Point _startPoint;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new WelcomePage());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WelcomePage());
            Home.IsEnabled = false;
            Artifacts.IsEnabled = true;
            Info.IsEnabled = true;
        }

        private void Artifacts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new ArtifactsPage());
            Home.IsEnabled = true;
            Artifacts.IsEnabled = false;
            Info.IsEnabled = true;
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new InfoPage());
            Home.IsEnabled = true;
            Artifacts.IsEnabled = true;
            Info.IsEnabled = false;
        }

        //private void Armor_Click(object sender, RoutedEventArgs e)
        //{
        //    MainFrame.NavigationService.Navigate(new ArmorPage());
        //    Home.IsEnabled = true;
        //    Artifacts.IsEnabled = true;
        //    //Armor.IsEnabled = false;
        //}

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                // Возвращаем окно в нормальное состояние
                this.WindowState = WindowState.Normal;
                // Восстанавливаем CornerRadius для всех элементов
                SetWindowCornerRadius(23, 20, 20, 23); // Основной Border
                SetSlidePanelCornerRadius(20, 0, 0, 20); // SlidePanel
            }
            else
            {
                // Разворачиваем окно на весь экран
                this.WindowState = WindowState.Maximized;
                // Убираем CornerRadius для всех элементов
                SetWindowCornerRadius(0, 0, 0, 0); // Основной Border
                SetSlidePanelCornerRadius(0, 0, 0, 0); // SlidePanel
            }
        }

        // Метод для изменения CornerRadius основного Border
        private void SetWindowCornerRadius(double topLeft, double topRight, double bottomRight, double bottomLeft)
        {
            if (MainBorder != null)
            {
                MainBorder.CornerRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
            }
        }

        // Метод для изменения CornerRadius SlidePanel
        private void SetSlidePanelCornerRadius(double topLeft, double topRight, double bottomRight, double bottomLeft)
        {
            if (SlidePanel != null)
            {
                SlidePanel.CornerRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
            }
        }

        // Обработчик изменения состояния окна
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (this.WindowState == WindowState.Maximized)
            {
                // Убираем CornerRadius при разворачивании
                SetWindowCornerRadius(0, 0, 0, 0); // Основной Border
                SetSlidePanelCornerRadius(0, 0, 0, 0); // SlidePanel
            }
            else
            {
                // Восстанавливаем CornerRadius при возврате в нормальное состояние
                SetWindowCornerRadius(23, 20, 20, 23); // Основной Border
                SetSlidePanelCornerRadius(20, 0, 0, 20); // SlidePanel
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            // Сворачиваем окно в панель задач
            this.WindowState = WindowState.Minimized;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(this);
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPoint = e.GetPosition(this);
                this.Left += currentPoint.X - _startPoint.X;
                this.Top += currentPoint.Y - _startPoint.Y;
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
        }

        private void ResizeThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;

            if (thumb != null)
            {
                if (thumb.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    // Изменение ширины окна
                    this.Width += e.HorizontalChange;
                }
                else if (thumb.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    // Изменение высоты окна
                    this.Height += e.VerticalChange;
                }
                else if (thumb.HorizontalAlignment == HorizontalAlignment.Right && thumb.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    // Изменение ширины и высоты одновременно
                    this.Width += e.HorizontalChange;
                    this.Height += e.VerticalChange;
                }
            }
        }
    }
}