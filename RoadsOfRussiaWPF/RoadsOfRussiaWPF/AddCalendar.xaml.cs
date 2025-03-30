using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace RoadsOfRussiaWPF
{
    /// <summary>
    /// Логика взаимодействия для AddCalendar.xaml
    /// </summary>
    public partial class AddCalendar : Window
    {
        private readonly int employeeId;

        private RoadsOfRussiaDLL.Desktop.DesktopController desktopController;

        public AddCalendar(int employeeId)
        {
            InitializeComponent();
            desktopController = new RoadsOfRussiaDLL.Desktop.DesktopController();

            this.employeeId = employeeId;
            LoadingData();
        }

        private void LoadingData()
        {
            CalendarType.ItemsSource = new List<string>() { "Обучение", "Временное отсутствие", "Отпуск" };
            CalendarType.SelectedIndex = 0;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate == null || EndDate.SelectedDate == null || Title.Text == "")
            {
                MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            switch (CalendarType.SelectedItem.ToString())
            {
                case "Обучение":
                    if (await desktopController.PostTraningCalendar(employeeId, Title.Text, Description.Text, StartDate.SelectedDate.Value, EndDate.SelectedDate.Value))
                    {
                        MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                        break;

                case "Временное отсутствие":
                    if (await desktopController.PostTemporaryAbsenceCalendar(employeeId, Title.Text, Description.Text, StartDate.SelectedDate.Value, EndDate.SelectedDate.Value))
                    {
                        MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                case "Отпуск":
                    if (await desktopController.PostVacationCalendar(employeeId, Title.Text, Description.Text, StartDate.SelectedDate.Value, EndDate.SelectedDate.Value))
                    {
                        MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }
    }
}
