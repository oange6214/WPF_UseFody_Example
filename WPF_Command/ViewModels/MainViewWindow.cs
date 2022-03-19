using Jed.Common;
using System.Windows;
using System.Windows.Input;

namespace WPF_Command.ViewModels
{
    public class MainViewWindow
    {
        public MainViewWindow()
        {
            TestCommand = new RelayCommand(GetEmployeeAction);
        }

        public ICommand TestCommand { get; set; }

        private void GetEmployeeAction()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Hello World");
            });
        }
    }
}
