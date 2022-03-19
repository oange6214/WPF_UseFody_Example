using Jed.Common;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPF_Fody.Models;

namespace WPF_Fody.ViewModels
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            Employees.Add(new EmployeeModel
            {
                Id = 1,
                Name = "Jed.Lin",
                Department = "Software"
            });
            Employees.Add(new EmployeeModel
            {
                Id = 2,
                Name = "Frank",
                Department = "Software"
            });
            Employees.Add(new EmployeeModel
            {
                Id = 3,
                Name = "Vito",
                Department = "Software"
            });

            //GetEmployeeCmd = new RelayCommand<string>(GetEmployeeAction);
            GetEmployeeCmd = new RelayCommand<EmployeeModel>(GetEmployeeAction);

            // Binding - string.Format Sample
            DateTimeNow = DateTime.Now;
            Price = 123.456;
            Total = 28768234.9329;

            Person = new PersonModel { FirstName = "Jed", LastName = "Lin" };
        }

        #region Properties
        public ICommand GetEmployeeCmd { get; set; }

        public DateTime DateTimeNow { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public PersonModel Person { get; set; }


        private ObservableCollection<EmployeeModel> _employees;
        public ObservableCollection<EmployeeModel> Employees
        {
            get { return _employees ?? (_employees = new ObservableCollection<EmployeeModel>()); }
        }

        #endregion


        #region Action

        private void GetEmployeeAction(string name)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"姓名：{name}");
            });
        }

        private void GetEmployeeAction(EmployeeModel model)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Id：{model.Id}, 姓名：{model.Name}, 部門：{model.Department}");
            });
        }

        #endregion

    }
}
