﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Result
    {
        public int UserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Picture { get; set; }
        public int Price { get; set; }
        public int IsTrainer { get; set; }
        public Result( int _userCode, string _firstName, string _lastName, string _gender, int _age, string _latitude, string _longitude, string _startTime, string _endTime, string _picture, int _price, int _isTrainer)
            
        {
            UserCode = _userCode;
            FirstName = _firstName;
            LastName = _lastName;
            Gender = _gender;
            Age = _age;
            Latitude = _latitude;
            Longitude = _longitude;
            StartTime = _startTime;
            EndTime = _endTime;
            Picture = _picture;
            Price = _price;
            IsTrainer = _isTrainer;
    }

        public Result()
        {

        }



    }
}