using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public abstract class OnlineHistory
    {
        public int OnlineCode { get; set; }
        public int UserCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public OnlineHistory( int _userCode,  string _latitude, string _longitude, string _startTime, string _endTime)
        {
            
            UserCode = _userCode;
            Latitude = _latitude;
            Longitude = _longitude;
            StartTime = _startTime;
            EndTime = _endTime;

        }


    }
}