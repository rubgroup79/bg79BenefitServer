using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public abstract class User
    {
		public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Picture { get; set; }
        public float Rate { get; set; }
        public int SearchRadius { get; set; }
        public bool IsTrainer { get; set; }
        public int[] SportCategories { get; set; }

        public User(string _email, string _firstName, string _lastName, string _password, string _gender, DateTime _dateOfBirth, string _picture, float _rate, int _searchRadius, bool _isTrainer)
        {
            Email = _email;
            FirstName = _firstName;
            LastName = _lastName;
            Password = _password;
            Gender = _gender;
            DateOfBirth = _dateOfBirth;
            Picture = _picture;
            Rate = _rate;
            SearchRadius = _searchRadius;
            IsTrainer = _isTrainer;
        }

        public User()
        {
            
        }

        public int SignInTrainer(Trainer t)
        {
            DBservices dbs = new DBservices();
            int UserCode = dbs.SignInTrainer(t);
            return UserCode;
        }

        public int SignInTrainee(Trainee t)
        {
            DBservices dbs = new DBservices();
            int UserCode = dbs.SignInTrainee(t);
            return UserCode;
        }
    }
}