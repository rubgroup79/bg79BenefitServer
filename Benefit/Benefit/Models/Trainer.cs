using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Trainer : User
    {
        public int PersonalTrainingPrice { get; set; }
        public int GroupTrainingPrice { get; set; }


        public Trainer(string _email, string _firstName, string _lastName, string _password, string _gender, DateTime _dateOfBirth, string _picture, float _rate, int _searchRadius, bool _isTrainer, int _personalTrainingPrice, int _groupTrainingPrice)
            : base(_email, _firstName, _lastName, _password, _gender, _dateOfBirth, _picture, _rate, _searchRadius, _isTrainer)
        {
            PersonalTrainingPrice = _personalTrainingPrice;
            GroupTrainingPrice = _groupTrainingPrice;
        }


    }
}