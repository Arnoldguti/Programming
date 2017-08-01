using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.Models
{
    public class Schedule: BindableBase
    {

        private string _campusCode;

        public string CampusCode
        {
            get { return _campusCode; }
            set { SetProperty(ref _campusCode, value); }
        }


        private string _campus;

        public string Campus
        {
            get { return _campus; }
            set { SetProperty(ref _campus, value); }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        private string _days;

        public string Days
        {
            get { return _days; }
            set { SetProperty(ref _days, value); }
        }


        private string _classroom;

        public string Classroom
        {
            get { return _classroom; }
            set { SetProperty(ref _classroom, value); }
        }


        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }


        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }


        private string _code;

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }


        private string _group;

        public string Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }


        private string  _courseInitial;

        public string  CourseInitial
        {
            get { return _courseInitial; }
            set { _courseInitial = value; }
        }


        private string _colorListItem;

        public string ColorListItem
        {
            get { return _colorListItem; }
            set { _colorListItem = value; }
        }
        
        

        


    }
}
