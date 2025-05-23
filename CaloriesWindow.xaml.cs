using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CaloriesCounter
{
    public partial class CaloriesWindow : Window
    {
        private List<FoodItem> foodItems = new List<FoodItem>();
        private List<FoodItem> eatenItems = new List<FoodItem>();

        private int dailyCalorieLimit = 2000;

        public CaloriesWindow()
        {
            InitializeComponent();
        }

        private void AddEaten_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(CaloriesBox.Text))
            {
                MessageBox.Show("Введи назву і кількість калорій", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(CaloriesBox.Text, out int calories))
            {
                MessageBox.Show("Калорії мають бути числом", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FoodItem eatenItem = new FoodItem(NameBox.Text.Trim(), calories);
            eatenItems.Add(eatenItem);

            RefreshEatenList();
            ClearFields();
            UpdateStatus();
        }

        private void EditEaten_Click(object sender, RoutedEventArgs e)
        {
            if (EatenList.SelectedItem is FoodItem selectedItem)
            {
                if (string.IsNullOrWhiteSpace(NameBox.Text))
                {
                    MessageBox.Show("Введи назву", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(CaloriesBox.Text, out int calories))
                {
                    MessageBox.Show("Калорії мають бути числом", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedItem.Name = NameBox.Text.Trim();
                selectedItem.Calories = calories;

                RefreshEatenList();
                ClearFields();
                UpdateStatus();
            }
            else
            {
                MessageBox.Show("Вибери пункт зі списку для редагування", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteEaten_Click(object sender, RoutedEventArgs e)
        {
            if (EatenList.SelectedItem is FoodItem selectedItem)
            {
                eatenItems.Remove(selectedItem);
                RefreshEatenList();
                ClearFields();
                UpdateStatus();
            }
            else
            {
                MessageBox.Show("Вибери пункт зі списку для видалення", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            string goal = GoalBox.Text.Trim().ToLower();

            switch (goal)
            {
                case "зменшити":
                    dailyCalorieLimit = 2000 - 300;
                    break;
                case "збільшити":
                    dailyCalorieLimit = 2000 + 300;
                    break;
                case "зберегти":
                    dailyCalorieLimit = 2000;
                    break;
                default:
                    MessageBox.Show("Введи: зменшити / зберегти / збільшити", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }

            ResultBlock.Text = $"Ваша денна норма калорій: {dailyCalorieLimit} ккал";
            UpdateStatus();
        }



        private void RefreshEatenList()
        {
            EatenList.ItemsSource = null;
            EatenList.ItemsSource = eatenItems;
        }

        private void ClearFields()
        {
            NameBox.Clear();
            CaloriesBox.Clear();
        }

        private void UpdateStatus()
        {
            int totalCalories = 0;
            foreach (var item in eatenItems)
                totalCalories += item.Calories;

            if (dailyCalorieLimit == 0)
            {
                StatusBlock.Text = "Вкажи ціль і натисни 'Розрахувати денну норму'";
                return;
            }

            if (totalCalories > dailyCalorieLimit)
            {
                StatusBlock.Text = $"Перевищення норми на {totalCalories - dailyCalorieLimit} ккал 😱";
                StatusBlock.Foreground = System.Windows.Media.Brushes.Red;
            }
            else if (totalCalories < dailyCalorieLimit)
            {
                StatusBlock.Text = $"Недобір калорій: {dailyCalorieLimit - totalCalories} ккал 🤏";
                StatusBlock.Foreground = System.Windows.Media.Brushes.Yellow;
            }
            else
            {
                StatusBlock.Text = "Ви вписалися у денну норму! 👍";
                StatusBlock.Foreground = System.Windows.Media.Brushes.Lime;
            }
        }
    }

    public class FoodItem
    {
        public string Name { get; set; }
        public int Calories { get; set; }

        public FoodItem(string name, int calories)
        {
            Name = name;
            Calories = calories;
        }

        public override string ToString()
        {
            return $"{Name} — {Calories} ккал";
        }
    }
}
