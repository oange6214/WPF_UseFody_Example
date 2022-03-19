using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Fody.Models
{
    public class DataModel
    {
        private string _processSomeData = "done";

        public string ProcessSomeData
        {
            get 
            {
                Task.Delay(2000).Wait();
                return _processSomeData;
            }
        }

        private string _myData = "download data.";

        public string MyData
        {
            get
            {
                Task.Delay(1000).Wait();
                return _myData;
            }
        }
    }
}
