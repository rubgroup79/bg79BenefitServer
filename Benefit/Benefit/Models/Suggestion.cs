using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Suggestion
    {
        public int FromCode { get; set; }
        public int ToCode { get; set; }
        public int StatusCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public Suggestion(int _fromCode, int _toCode, int _statusCode, DateTime _date, DateTime _time)
        {
            FromCode = _fromCode;
            ToCode = _toCode;
            StatusCode = _statusCode;
            Date = _date;
            Time = _time;
        }


    }
}