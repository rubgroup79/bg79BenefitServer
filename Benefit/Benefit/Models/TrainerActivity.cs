using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class TrainerActivity : Activity
    {
       
        public bool PersonalTraining { get; set; }
        public bool GroupTraining { get; set; }
 

        public TrainerActivity(int _userCode, DateTime _date, DateTime _time, string _latitude, string _longitude, DateTime _startTime, DateTime _endTime , bool _personalTraining, bool _groupTraining)
            :base( _userCode,  _date,  _time,  _latitude,  _longitude,  _startTime,  _endTime)
        {
            PersonalTraining = _personalTraining;
            GroupTraining = _groupTraining;
        }


    }
}