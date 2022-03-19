using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Fody.Common;
using WPF_Fody.Models;

namespace WPF_Fody.ViewModels
{
    public class EmployeeViewModel
    {
        public ICommand GetEmployeeCmd { get; set; }

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

            GetEmployeeCmd = new RelayCommand<string>(GetEmployeeAction);
        }

        private void GetEmployeeAction(string name)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"姓名：{name}");
            });
        }


        private ObservableCollection<EmployeeModel> _employees;
        public ObservableCollection<EmployeeModel> Employees    
        {
            get { return _employees ?? (_employees = new ObservableCollection<EmployeeModel>()); }
        }
    }
}
