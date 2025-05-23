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
    }
}