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
        public int SportCategoryCode1 { get; set; }
        public int SportCategoryCode2 { get; set; }
        public int SportCategoryCode3 { get; set; }

        public GroupTraining(DateTime _date, DateTime _time, string _latitude, string _longitude, bool _withTrainer, int _statusCode, int _creatorCode, int _minParticipants, int _maxParticipants, int _sportCategoryCode1, int _sportCategoryCode2, int _sportCategoryCode3, int _currentParticipants=0)
            :base( _date,  _time,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
            CreatorCode = _creatorCode;
            MinParticipants = _minParticipants;
            MaxParticipants = _maxParticipants;
            CurrentParticipants = _currentParticipants;
            SportCategoryCode1 = _sportCategoryCode1;
            SportCategoryCode2 = _sportCategoryCode2;
            SportCategoryCode3 = _sportCategoryCode3;



        }


    }
}