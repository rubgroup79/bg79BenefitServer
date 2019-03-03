using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public  class CurrentOnlineTrainee: OnlineHistoryTrainee
    {
        public CurrentOnlineTrainee(int _userCode, DateTime _date, DateTime _time, string _latitude, string _longitude, DateTime _startTime, DateTime _endTime, bool _withTrainer, bool _withPartner, bool _groupWithTrainer, bool _groupWithPartners)
            :base( _userCode,  _date,  _time,  _latitude,  _longitude,  _startTime,  _endTime,  _withTrainer,  _withPartner,  _groupWithTrainer,  _groupWithPartners)
        {

        }


    }
}