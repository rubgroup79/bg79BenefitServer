using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Person
    {

        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string Gender { get; set; }
        public double Age { get; set; }
        public double Height { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int[] Hobbies { get; set; }
        public string Phone { get; set; }
        public int IsActive { get; set; }
        public int IsPremium { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Person()
        {

        }
        public Person(string _name, string _familyName, string _gender, double _age, double _height, string _image, string _address, int[] _hobbies, string _phone, int _isPremium, string _password, string _email, int _isActive = 0) {

            Name = _name;
            FamilyName = _familyName;
            Gender = _gender;
            Age = _age;
            Height = _height;
            Address = _address;
            Image = " ";
            Hobbies = _hobbies;
            IsActive = _isActive;
            IsPremium = _isPremium;
            Phone = _phone;
            Email = _email;
            Password = _password;
            
        }
        public int insert()
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

        public List<Person> get_person ()
        {
            DBservices dbs = new DBservices();
            List<Person> pl=dbs.get_person("tinder_ConnectionStringName", "PersonTbl");
            return pl;
        }

        public int Active(int Active, int PersonId)
        {
            DBservices dbs = new DBservices();
            int numAffected= dbs.Active(Active , PersonId);
            return numAffected;
          
        }

        public bool login(string Email , string Password)
        {
            DBservices dbs = new DBservices();
            return dbs.login(Email , Password);

        }
        public bool checkEmail(string email)
        {
            DBservices dbs = new DBservices();
            return dbs.checkEmail("tinder_ConnectionStringName", "PersonTbl", email);
        }
        

        public Person GetDetails(string email)
        {
            DBservices dbs = new DBservices();
            return dbs.GetDetails("tinder_ConnectionStringName", "PersonTbl", email);
        }
        
            public int editDetails()
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.editDetails(this);
            return numAffected;
        }

    }
}