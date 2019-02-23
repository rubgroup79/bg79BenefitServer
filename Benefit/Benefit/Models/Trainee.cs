using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Trainee: User
    {
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }
        public string PartnerGender { get; set; }
        public string TrainerGender { get; set; }

        public Trainee(string _email, string _firstName, string _lastName, string _password, string _gender, DateTime _dateOfBirth, string _picture, float _rate, int _searchRadius, bool _isTrainer, int _minBudget, int _maxBudget, string _partnerGender , string _trainerGender )
            :base( _email,  _firstName,  _lastName,  _password,  _gender,  _dateOfBirth,  _picture,  _rate,  _searchRadius,  _isTrainer)
        {
            MinBudget = _minBudget;
            MaxBudget = _maxBudget;
            PartnerGender = _partnerGender;
            TrainerGender = _trainerGender;
        }



    }
}