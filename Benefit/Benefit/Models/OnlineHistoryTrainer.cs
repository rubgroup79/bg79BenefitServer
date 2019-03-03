﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class OnlineHistoryTrainer : OnlineHistory
    {

        public OnlineHistoryTrainer( int _userCode, DateTime _date, DateTime _time, string _latitude, string _longitude, DateTime _startTime, DateTime _endTime)
            :base(_userCode, _date, _time, _latitude, _longitude, _startTime, _endTime)
        {
            
        }


    }
}