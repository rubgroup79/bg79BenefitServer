using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Training
    {
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsAGroup { get; set; }
        public bool WithTrainer { get; set; }

        public Training(DateTime _date, DateTime _time, string _latitude, string _longitude, bool _isAGroup, bool _withTrainer)
        {
            Date = _date;
            Time = _time;
            Latitude = _latitude;
            Longitude = _longitude;
            IsAGroup = _isAGroup;
            WithTrainer = _withTrainer;
        }


    }
}