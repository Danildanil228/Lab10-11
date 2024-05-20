using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab10_11
{
    public partial class Lab11 : Page
    {
        public Lab11()
        {
            InitializeComponent();
        }
        [Serializable]
        public class Employee
        {
            public string LastName { get; set; }
            public string Position { get; set; }
            public double Salary { get; set; }
            public DateTime BirthDate { get; set; }
            public override string ToString()
            {
                return $"{LastName}, {Position}, {Salary}, {BirthDate.ToShortDateString()}";
            }
        }

        private List<Employee> employees = new List<Employee>();



        private void SaveToFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON файл (*.json)|*.json";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(employees));
                    MessageBox.Show("Данные успешно сохранены.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON файл (*.json)|*.json";

                if (openFileDialog.ShowDialog() == true)
                {
                    employees = JsonSerializer.Deserialize<List<Employee>>(File.ReadAllText(openFileDialog.FileName));
                    UpdateEmployeeList();
                    MessageBox.Show("Данные успешно загружены.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = new Employee
                {
                    LastName = LastNameTextBox.Text,
                    Position = PositionTextBox.Text,
                    Salary = double.Parse(SalaryTextBox.Text),
                    BirthDate = BirthDatePicker.SelectedDate ?? DateTime.MinValue
                };

                employees.Add(employee);
                UpdateEmployeeList();
                MessageBox.Show("Сотрудник добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении сотрудника: " + ex.Message);
            }
        }

        private void UpdateEmployeeList()
        {
            EmployeesListBox.Items.Clear();
            foreach (var emp in employees)
            {
                EmployeesListBox.Items.Add(emp);
            }
        }
        private void ShowEmployeeInfoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filteredEmployees = employees.Where(emp => emp.Salary > employees.Average(emp => emp.Salary) &&
                                                               (DateTime.Now.Year - emp.BirthDate.Year) < 30);

                EmployeeInfoTextBlock.Text = "Сотрудники со зарплатой\nвыше средней и возрастом\nменьше 30 лет:\n\n";

                foreach (var emp in filteredEmployees)
                {
                    EmployeeInfoTextBlock.Text += $"Фамилия: {emp.LastName}\nДолжность: {emp.Position}\n" +
                                                  $"Зарплата: {emp.Salary}\nДата рождения: {emp.BirthDate.ToShortDateString()}\n\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выводе информации: " + ex.Message);
            }
        }
    }
}
