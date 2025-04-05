using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoadsOfRussiaWPF
{
    /// <summary>
    /// Логика взаимодействия для AddOrRemoveEmployee.xaml
    /// </summary>
    public partial class AddOrRemoveEmployee : Window
    {
        private readonly int loadingMethod;
        private readonly RoadsOfRussiaDLL.Desktop.Model.DivisionModel selectedDivision;
        private readonly RoadsOfRussiaDLL.Desktop.Model.EmployeeSelectedModel selectedEmployee;

        private List<RoadsOfRussiaDLL.Desktop.Model.CalandarModel> traningCalendar;
        private List<RoadsOfRussiaDLL.Desktop.Model.CalandarModel> vacationCalendar;
        private List<RoadsOfRussiaDLL.Desktop.Model.CalandarModel> temporaryAbsenceCalendar;

        private RoadsOfRussiaDLL.Desktop.DesktopController desktopController;

        public AddOrRemoveEmployee(int loadingMethod, RoadsOfRussiaDLL.Desktop.Model.DivisionModel selectedDivision, RoadsOfRussiaDLL.Desktop.Model.EmployeeSelectedModel selectedEmployee)
        {
            InitializeComponent();
            desktopController = new RoadsOfRussiaDLL.Desktop.DesktopController();

            this.loadingMethod = loadingMethod;
            this.selectedDivision = selectedDivision;
            this.selectedEmployee = selectedEmployee ?? null;
            LoadingData();
        }

        private async void LoadingData()
        {
            try
            {
                var postData = await desktopController.GetPostAsync();
                Post.ItemsSource = postData.Select(x => x.IDPost + "|" + x.Title).ToList();
                Post.SelectedIndex = 0;

                switch (loadingMethod)
                {
                    case 0:
                        var divisionEmployeeDefaultData = await desktopController.GetDirecorAndAssistent(selectedDivision.IDDivision);
                        Director.ItemsSource = divisionEmployeeDefaultData.Select(x => x.IDEmployee + "|" + x.Surname + " " + x.Name + " " + x.Patronymic).ToList();
                        Assistent.ItemsSource = divisionEmployeeDefaultData.Select(x => x.IDEmployee + "|" + x.Surname + " " + x.Name + " " + x.Patronymic).ToList();

                        Fired.IsEnabled = false;
                        AddCalendar.IsEnabled = false;
                        RemoveTraning.IsEnabled = false;
                        RemoveTemporaryAbsence.IsEnabled = false;
                        RemoveVacation.IsEnabled = false;
                        Division.Text = selectedDivision.Title;
                        break;

                    case 1:

                        var divisionEmployeeData = await desktopController.GetDirecorAndAssistent(selectedEmployee.IDDivision);
                        Director.ItemsSource = divisionEmployeeData.Where(x => x.IDEmployee != selectedEmployee.IDEmployee).Select(x => x.IDEmployee + "|" + x.Surname + " " + x.Name + " " + x.Patronymic).ToList();
                        Assistent.ItemsSource = divisionEmployeeData.Where(x => x.IDEmployee != selectedEmployee.IDEmployee).Select(x => x.IDEmployee + "|" + x.Surname + " " + x.Name + " " + x.Patronymic).ToList();

                        Surname.Text = selectedEmployee.Surname;
                        Name.Text = selectedEmployee.Name;
                        Patronimyc.Text = selectedEmployee.Patronymic;
                        Phone.Text = selectedEmployee.PersonalPhone;
                        DateOfBirth.SelectedDate = selectedEmployee.DateOfBirth;
                        Division.Text = selectedEmployee.Division;
                        Post.SelectedItem = selectedEmployee.IDPost + "|" + selectedEmployee.Post;
                        Director.SelectedItem = selectedEmployee.Director;
                        Assistent.SelectedItem = selectedEmployee.Assistent;
                        CorpPhone.Text = selectedEmployee.CorpPhone;
                        Email.Text = selectedEmployee.Email;
                        Cabinet.Text = selectedEmployee.Cabinet;
                        OtherInformation.Text = selectedEmployee.OtherInformation;

                        traningCalendar = await desktopController.GetTraningCalendar(selectedEmployee.IDEmployee);
                        vacationCalendar = await desktopController.GetVacationCalendar(selectedEmployee.IDEmployee);
                        temporaryAbsenceCalendar = await desktopController.GetTemporaryAbsenceCalendar(selectedEmployee.IDEmployee);

                        TraningCalendarListView.ItemsSource = traningCalendar;
                        VacationCalendarListView.ItemsSource = vacationCalendar;
                        TemporaryAbsenceListView.ItemsSource = temporaryAbsenceCalendar;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void PastTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(traningCalendar != null)
                    TraningCalendarListView.ItemsSource = traningCalendar.Where(x => x.EndDate < DateTime.Now.Date).ToList();
                if(vacationCalendar != null)
                    VacationCalendarListView.ItemsSource = vacationCalendar.Where(x => x.EndDate < DateTime.Now.Date).ToList();
                if(temporaryAbsenceCalendar != null)
                    TemporaryAbsenceListView.ItemsSource = temporaryAbsenceCalendar.Where(x => x.EndDate < DateTime.Now.Date).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NowTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(traningCalendar != null)
                    TraningCalendarListView.ItemsSource = traningCalendar.Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now).ToList();
                if(vacationCalendar != null)
                    VacationCalendarListView.ItemsSource = vacationCalendar.Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now).ToList();
                if(temporaryAbsenceCalendar != null)
                    TemporaryAbsenceListView.ItemsSource = temporaryAbsenceCalendar.Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void FutureTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(traningCalendar != null)
                    TraningCalendarListView.ItemsSource = traningCalendar.Where(x => x.StartDate > DateTime.Now.Date).ToList();
                if(vacationCalendar != null)
                    VacationCalendarListView.ItemsSource = vacationCalendar.Where(x => x.StartDate > DateTime.Now.Date).ToList();
                if(temporaryAbsenceCalendar != null)
                    TemporaryAbsenceListView.ItemsSource = temporaryAbsenceCalendar.Where(x => x.StartDate > DateTime.Now.Date).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void RemoveVacation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDate = VacationCalendarListView.SelectedItem as RoadsOfRussiaDLL.Desktop.Model.CalandarModel;

                if (selectedDate != null)
                {
                    if (await desktopController.RemoveVacationCalendar(selectedDate.IDCalendar))
                    {
                        MessageBox.Show("Событие удалено", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadingData();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления события", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Событие не выбранно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void RemoveTemporaryAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDate = TemporaryAbsenceListView.SelectedItem as RoadsOfRussiaDLL.Desktop.Model.CalandarModel;

                if (selectedDate != null)
                {
                    if (await desktopController.RemoveTemporaryAbsenceCalendar(selectedDate.IDCalendar))
                    {
                        MessageBox.Show("Событие удалено", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadingData();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления события", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Событие не выбранно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void RemoveTraning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDate = TraningCalendarListView.SelectedItem as RoadsOfRussiaDLL.Desktop.Model.CalandarModel;

                if (selectedDate != null)
                {
                    if (await desktopController.RemoveTraningCalendar(selectedDate.IDCalendar))
                    {
                        MessageBox.Show("Событие удалено", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadingData();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления события", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Событие не выбранно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void AddCalendar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddCalendar addCalendar = new AddCalendar(selectedEmployee.IDEmployee);
                addCalendar.ShowDialog();

                LoadingData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Surname.Text == "" || Name.Text == "" || Patronimyc.Text == "" || CorpPhone.Text == "" || Email.Text == "" || Cabinet.Text == "" || DateOfBirth.SelectedDate == null)
                {
                    MessageBox.Show("Не все поля заполненый", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int postId = 1;
                int? directorId = null;
                int? assistentId = null;

                if (Post.SelectedIndex != -1)
                {
                    string[] postTemp = Post.SelectedItem.ToString().Split('|');
                    postId = int.Parse(postTemp[0]);
                }
                if (Director.SelectedIndex != -1)
                {
                    string[] directorTemp = Director.SelectedItem.ToString().Split('|');
                    directorId = int.Parse(directorTemp[0]);
                }
                if (Assistent.SelectedIndex != -1)
                {
                    string[] assistentTemp = Assistent.SelectedItem.ToString().Split('|');
                    assistentId = int.Parse(assistentTemp[0]);
                }

                switch (loadingMethod)
                {
                    case 0:
                        if (await desktopController.SaveEmployee(null, Surname.Text, Name.Text, Patronimyc.Text, Phone.Text, DateOfBirth.SelectedDate.Value, selectedDivision.IDDivision, postId, directorId, assistentId, CorpPhone.Text, Email.Text, Cabinet.Text, OtherInformation.Text))
                        {
                            MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case 1:
                        if (await desktopController.SaveEmployee(selectedEmployee.IDEmployee, Surname.Text, Name.Text, Patronimyc.Text, Phone.Text, DateOfBirth.SelectedDate.Value, selectedEmployee.IDDivision, postId, directorId, assistentId, CorpPhone.Text, Email.Text, Cabinet.Text, OtherInformation.Text))
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void Fired_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedEmployee.DateOfFired != null)
                {
                    MessageBox.Show("Сотрудник уже уволен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if ((traningCalendar.Where(x => x.StartDate > DateTime.Now).FirstOrDefault()) == null)
                {
                    if (MessageBox.Show("Вы действительно хотите уволить сотрудника?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (await desktopController.FiredEmployee(selectedEmployee.IDEmployee))
                        {
                            MessageBox.Show("Сотрудник уволен", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при увольнени сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("У данного сотрудника заплпнированно обучение", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
