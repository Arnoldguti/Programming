using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Common;

namespace Project.Models
{
    public class Postals_C : BindableBase
    {

        private string _pathCode;

        public string PathCode
        {
            get { return _pathCode; }
            set { SetProperty(ref _pathCode, value); }
        }


        private string _pathName;

        public string PathName
        {
            get { return _pathName; }
            set { SetProperty(ref _pathName, value); }
        }

     
        
        private string _pathDate;

        public string PathDate
        {
            get { return _pathDate; }
            set { SetProperty(ref _pathDate, value); }
        }
        
    }
}
