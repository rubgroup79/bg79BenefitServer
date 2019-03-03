using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class GroupTraining : Training
    {
        public int CreatorCode { get; set; }
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }


        public GroupTraining(DateTime _date, DateTime _time, string _latitude, string _longitude, bool _withTrainer, int _statusCode, int _creatorCode, int _minParticipants, int _maxParticipants, int _currentParticipants=0 )
            :base( _date,  _time,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
            CreatorCode = _creatorCode;
            MinParticipants = _minParticipants;
            MaxParticipants = _maxParticipants;
            CurrentParticipants = _currentParticipants;
        }


    }
}