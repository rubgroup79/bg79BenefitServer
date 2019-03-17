using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class HistoryGroupTraining : Training
    {
        public int CreatorCode { get; set; }
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public int SportCategoryCode { get; set; }
        public int Price { get; set; }


        public HistoryGroupTraining(string _trainingTime, string _latitude, string _longitude, int _withTrainer, int _statusCode, int _creatorCode, int _minParticipants, int _maxParticipants, int _price, int _sportCategoryCode,  int _currentParticipants=0)
            :base(_trainingTime,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
            CreatorCode = _creatorCode;
            MinParticipants = _minParticipants;
            MaxParticipants = _maxParticipants;
            CurrentParticipants = _currentParticipants;
            SportCategoryCode = _sportCategoryCode;
            Price = _price;



        }

        public HistoryGroupTraining()
        {

        }

        public void InsertGroupTraining(HistoryGroupTraining h)
        {
            DBservices dbs = new DBservices();
            dbs.InsertGroupTraining(this);
        }




    }
}