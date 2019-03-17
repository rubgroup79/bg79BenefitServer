using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class CoupleTraining : Training
    {


        public CoupleTraining(string _trainingTime, string _latitude, string _longitude, int _withTrainer, int _statusCode )
            :base(_trainingTime,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
        }


    }
}