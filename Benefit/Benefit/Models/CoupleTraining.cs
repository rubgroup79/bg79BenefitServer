using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{


    public class CoupleTraining : Training
    {
        public int PartnerUserCode { get; set; }
        public string PartnerFirstName { get; set; }
        public string PartnerLastName { get; set; }
        public int PartnerAge { get; set; }
        public string PartnerPicture { get; set; }


        public CoupleTraining(string _trainingTime, float _latitude, float _longitude, int _withTrainer, int _statusCode
            , int _partnerUserCode, string _partnerFirstName, string _partnerLastName, int _partnerAge, string _partnerPicture)
            :base(_trainingTime,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
            PartnerUserCode = _partnerUserCode;
            PartnerFirstName = _partnerFirstName;
            PartnerLastName = _partnerLastName;
            PartnerAge = _partnerAge;
            PartnerPicture = _partnerPicture;
    }

        public CoupleTraining() { }
        public List<CoupleTraining> GetFutureTrainings(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetFutureTrainings(UserCode);
        }

    }
}