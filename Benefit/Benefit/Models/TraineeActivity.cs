using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class TraineeActivity : Activity
    {

        public bool WithTrainer { get; set; }
        public bool WithPartner { get; set; }
        public bool GroupWithTrainer { get; set; }
        public bool GroupWithPartners { get; set; }

        public TraineeActivity(int _userCode, DateTime _date, DateTime _time, string _latitude, string _longitude, DateTime _startTime, DateTime _endTime, bool _withTrainer , bool _withPartner, bool _groupWithTrainer, bool _groupWithPartners)
            :base ( _userCode,  _date,  _time,  _latitude,  _longitude,  _startTime,  _endTime)
        {
            WithTrainer = _withTrainer;
            WithPartner = _withPartner;
            GroupWithTrainer = _groupWithTrainer;
            GroupWithPartners = _groupWithPartners;

        }


    }
}