using Newtonsoft.Json;
using RoadsOfRussiaDLL.Desktop;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoadsOfRussiaWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RoadsOfRussiaDLL.Desktop.Model.DivisionModel selectedDivision;
        private RoadsOfRussiaDLL.Desktop.DesktopController desktopController;

        public MainWindow()
        {
            InitializeComponent();
            desktopController = new RoadsOfRussiaDLL.Desktop.DesktopController();

            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            try
            {
                var divisions = await desktopController.GetDivisionsAsync();

                PopulateTreeView(divisions, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void PopulateTreeView(RoadsOfRussiaDLL.Desktop.Model.DivisionModel division, TreeViewItem parentItem)
        {
            try
            {
                TreeViewItem newItem = new TreeViewItem
                {
                    Header = division.Title,
                    Tag = division
                };

                if (parentItem == null)
                {
                    DivisionTreeView.Items.Add(newItem);
                }
                else
                {
                    parentItem.Items.Add(newItem);
                }

                if (division.Division1 != null && division.Division1.Any())
                {
                    foreach (var subDivision in division.Division1)
                    {
                        PopulateTreeView(subDivision, newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void DivisionTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (DivisionTreeView.SelectedItem is TreeViewItem division)
                {
                    var selectedDivision = (RoadsOfRussiaDLL.Desktop.Model.DivisionModel)division.Tag;
                    this.selectedDivision = (RoadsOfRussiaDLL.Desktop.Model.DivisionModel)division.Tag;

                    if (selectedDivision != null)
                    {
                        List<RoadsOfRussiaDLL.Desktop.Model.EmployeeSelectedModel> employeeSelecteds = await desktopController.GetEmployeesAsync(selectedDivision.IDDivision);

                        EmployeeListView.ItemsSource = null;
                        EmployeeListView.ItemsSource = employeeSelecteds;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async Task UpdateEmployeeList(int divisionId)
        {
            try
            {
                List<RoadsOfRussiaDLL.Desktop.Model.EmployeeSelectedModel> employeeSelecteds = await desktopController.GetEmployeesAsync(divisionId);

                EmployeeListView.ItemsSource = null;
                EmployeeListView.ItemsSource = employeeSelecteds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void EmployeeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedEmployee = EmployeeListView.SelectedItem as RoadsOfRussiaDLL.Desktop.Model.EmployeeSelectedModel;

                if (selectedEmployee != null)
                {
                    AddOrRemoveEmployee addOrRemoveEmployee = new AddOrRemoveEmployee(1, selectedDivision, selectedEmployee);
                    addOrRemoveEmployee.ShowDialog();

                    await UpdateEmployeeList(selectedDivision.IDDivision);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddOrRemoveEmployee addOrRemoveEmployee = new AddOrRemoveEmployee(0, selectedDivision, null);
                addOrRemoveEmployee.ShowDialog();

                await UpdateEmployeeList(selectedDivision.IDDivision);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
