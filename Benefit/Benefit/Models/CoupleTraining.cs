using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class CoupleTraining : Training
    {


        public CoupleTraining(DateTime _date, DateTime _time, string _latitude, string _longitude, bool _withTrainer, int _statusCode )
            :base( _date,  _time,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
        }


    }
}