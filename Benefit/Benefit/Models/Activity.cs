using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public abstract class Activity
    {
        public int UserCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Activity(int _userCode, DateTime _date, DateTime _time, string _latitude, string _longitude, DateTime _startTime, DateTime _endTime)
        {
            UserCode = _userCode;
            Date = _date;
            Time = _time;
            Latitude = _latitude;
            Longitude = _longitude;
            StartTime = _startTime;
            EndTime = _endTime;
        }


    }
}