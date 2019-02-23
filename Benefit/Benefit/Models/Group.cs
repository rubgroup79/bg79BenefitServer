using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Group : Training
    {

        public int ManagerCode { get; set; }
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }


        public Group(DateTime _date, DateTime _time, string _latitude, string _longitude, bool _isAGroup, bool _withTrainer, int _managerCode, int _minParticipants, int _maxParticipants )
            :base( _date,  _time,  _latitude,  _longitude,  _isAGroup,  _withTrainer)
        {
            ManagerCode = _managerCode;
            MinParticipants = _minParticipants;
            MaxParticipants = _maxParticipants;
        }


    }
}