using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Fody.ViewModels
{
    public class EmployeeViewModel
    {
        public ICommand GetEmployeeCmd { get; set; }

        public EmployeeViewModel()
        {

        }
    }
}
