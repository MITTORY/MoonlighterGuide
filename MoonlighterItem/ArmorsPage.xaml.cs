using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Логика взаимодействия для ArmorsPage.xaml
    /// </summary>
    public partial class ArmorsPage : Page
    {
        private List<Armors> _allArmors;

        public ArmorsPage()
        {
            InitializeComponent();

            // Загрузка данных из JSON
            LoadArmors();

            // Заполнение ComboBox подземельями
            var Type = new List<string>
    {
        "Вся броня",
        "Тканевая броня",
        "Железная броня",
        "Стальная броня",
        "Композитная броня"
    };

            TypeFilterComboBox.ItemsSource = Type;

            // Установка начального значения ComboBox
            TypeFilterComboBox.SelectedIndex = 0;

            // Установка начального значения сортировки
            SortComboBox.SelectedIndex = 0;
        }

        private void LoadArmors()
        {
            try
            {
                // Путь к JSON-файлу
                string jsonPath = "armors.json";

                // Проверка существования файла
                if (File.Exists(jsonPath))
                {
                    // Чтение JSON-файла
                    string json = File.ReadAllText(jsonPath);

                    // Десериализация JSON в список предметов
                    _allArmors = JsonSerializer.Deserialize<List<Armors>>(json);

                    // Отображение всех предметов
                    ArmorsList.ItemsSource = _allArmors;
                }
                else
                {
                    MessageBox.Show("Файл Armors.json не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Если текст равен подсказке, очищаем поле и меняем цвет текста
            if (SearchTextBox.Text == "Поиск...")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Если поле пустое, возвращаем подсказку
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Поиск...";
                SearchTextBox.Foreground = Brushes.Gray;
            }
        }

        // Обработчик изменения текста в TextBox
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Обработчик изменения выбора в ComboBox
        private void TypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // Проверяем, что _allarmors не равен null
            if (_allArmors == null)
            {
                return; // Прерываем выполнение, если данные не загружены
            }

            // Получаем текст поиска
            var searchText = SearchTextBox.Text?.ToLower() ?? string.Empty;

            // Если текст равен "Поиск...", игнорируем его
            if (searchText == "поиск...")
            {
                searchText = string.Empty;
            }

            // Проверяем, что выбранный элемент в ComboBox не равен null
            var selectedType = TypeFilterComboBox.SelectedItem as string;
            if (selectedType == null)
            {
                return; // Прерываем выполнение, если подземелье не выбрано
            }

            // Применяем фильтры
            var filteredArmors = _allArmors
                .Where(armor => string.IsNullOrEmpty(searchText) || armor.Name.ToLower().Contains(searchText))
                .Where(armor => selectedType == "Вся броня" || armor.Type == selectedType)
                .ToList();

            // Применяем сортировку
            var selectedSort = (SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (selectedSort)
            {
                case "Сортировка: По названию (А-Я)":
                    filteredArmors = filteredArmors.OrderBy(armor => armor.Name).ToList();
                    break;
                case "Сортировка: По названию (Я-А)":
                    filteredArmors = filteredArmors.OrderByDescending(armor => armor.Name).ToList();
                    break;
                case "Сортировка: По цене (↑)":
                    filteredArmors = filteredArmors.OrderBy(armor => armor.BasePrice).ToList();
                    break;
                case "Сортировка: По цене (↓)":
                    filteredArmors = filteredArmors.OrderByDescending(armor => armor.BasePrice).ToList();
                    break;
                case "Сортировка: По виду брони (А-Я)":
                    filteredArmors = filteredArmors.OrderBy(armor => armor.Type).ToList();
                    break;
                case "Без сортировки":
                    break;
            }

            // Обновляем список предметов
            ArmorsList.ItemsSource = filteredArmors;
        }

        // Класс предмета
        public class Armors
        {
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public int LowPrice { get; set; }
            public int BasePrice { get; set; }
            public int HighDemandPrice { get; set; }
            public string Type { get; set; }
        }
    }
}
