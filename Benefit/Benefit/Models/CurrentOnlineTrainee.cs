using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public  class CurrentOnlineTrainee: OnlineHistoryTrainee
    {
        public CurrentOnlineTrainee(int _userCode, string _insertTime, string _latitude, string _longitude, string _startTime, string _endTime, int _withTrainer, int _withPartner, int _groupWithTrainer, int _groupWithPartners)
            :base( _userCode,  _insertTime,  _latitude,  _longitude,  _startTime,  _endTime,  _withTrainer,  _withPartner,  _groupWithTrainer,  _groupWithPartners)
        {

        }


    }
}