using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MoonlighterItem
{
    public partial class ArtifactsPage : Page
    {
        private List<Artifact> _allArtifacts;

        public ArtifactsPage()
        {
            InitializeComponent();

            // Загрузка данных из JSON
            LoadArtifacts();

            // Заполнение ComboBox подземельями
            var dungeons = new List<string>
            {
                "Все подземелья",
                "Подземелье големов",
                "Лесное подземелье",
                "Пустынное подземелье",
                "Технологичное подземелье",
                "Межпространственное подземелье"
            };

            DungeonFilterComboBox.ItemsSource = dungeons;

            // Установка начального значения ComboBox
            DungeonFilterComboBox.SelectedIndex = 0;
        }

        private void LoadArtifacts()
        {
            try
            {
                // Путь к JSON-файлу
                string jsonPath = "artifacts.json";

                // Проверка существования файла
                if (File.Exists(jsonPath))
                {
                    // Чтение JSON-файла
                    string json = File.ReadAllText(jsonPath);

                    // Десериализация JSON в список предметов
                    _allArtifacts = JsonSerializer.Deserialize<List<Artifact>>(json);

                    // Отображение всех предметов
                    ArtifactsList.ItemsSource = _allArtifacts;
                }
                else
                {
                    MessageBox.Show("Файл artifacts.json не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                SearchTextBox.Foreground = Brushes.Black; // Устанавливаем цвет текста для ввода
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Если поле пустое, возвращаем подсказку
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Поиск...";
                SearchTextBox.Foreground = Brushes.Gray; // Устанавливаем серый цвет для подсказки
            }
        }

        // Обработчик изменения текста в TextBox
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Обработчик изменения выбора в ComboBox
        private void DungeonFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // Проверяем, что _allArtifacts не равен null
            if (_allArtifacts == null)
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
            var selectedDungeon = DungeonFilterComboBox.SelectedItem as string;
            if (selectedDungeon == null)
            {
                return; // Прерываем выполнение, если подземелье не выбрано
            }

            // Применяем фильтры
            var filteredArtifacts = _allArtifacts
                .Where(artifact => string.IsNullOrEmpty(searchText) || artifact.Name.ToLower().Contains(searchText))
                .Where(artifact => selectedDungeon == "Все подземелья" || artifact.Dungeon == selectedDungeon)
                .ToList();

            // Обновляем список предметов
            ArtifactsList.ItemsSource = filteredArtifacts;
        }

        // Класс предмета
        public class Artifact
        {
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public int BasePrice { get; set; }
            public int HighDemandPrice { get; set; }
            public string Dungeon { get; set; }
        }
    }
}