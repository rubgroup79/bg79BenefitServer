using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Request
    {
        public int FromCode { get; set; }
        public int GroupCode { get; set; }
        public int StatusCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public Request(int _fromCode, int _groupCode, int _statusCode, DateTime _date, DateTime _time)
        {
            FromCode = _fromCode;
            GroupCode = _groupCode;
            StatusCode = _statusCode;
            Date = _date;
            Time = _time;

        }


    }
}