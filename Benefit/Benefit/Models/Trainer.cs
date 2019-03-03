﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Trainer : User
    {
        public int PersonalTrainingPrice { get; set; }
        public int GroupTrainingPrice { get; set; }


        public Trainer(string _email, string _firstName, string _lastName, string _password, string _gender, string _dateOfBirth, string _picture, int _searchRadius,int _isTrainer, int[] _sportCategories ,int _personalTrainingPrice, int _groupTrainingPrice, float _rate=0)
            : base(_email, _firstName, _lastName, _password, _gender, _dateOfBirth, _picture, _searchRadius,_isTrainer , _sportCategories, _rate)
        {
            PersonalTrainingPrice = _personalTrainingPrice;
            GroupTrainingPrice = _groupTrainingPrice;
        }

        public Trainer()
        {
                
        }

        public int SignInTrainer()
        {
            DBservices dbs = new DBservices();
            int UserCode = dbs.SignInTrainer(this);
            return UserCode;
        }


    }
}