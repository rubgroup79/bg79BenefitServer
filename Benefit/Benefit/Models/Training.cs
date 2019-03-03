using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public abstract class Training
    {
        public int TrainingCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool WithTrainer { get; set; }
        public int StatusCode { get; set; }

        public Training(DateTime _date, DateTime _time, string _latitude, string _longitude, bool _withTrainer, int _statusCode)
        {
            Date = _date;
            Time = _time;
            Latitude = _latitude;
            Longitude = _longitude;
            WithTrainer = _withTrainer;
            StatusCode = _statusCode;
        }


    }
}