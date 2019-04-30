using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Benefit.Models;

namespace Benefit.Controllers
{

    public class HistoryGroupTrainingController : ApiController
    {
        public HistoryGroupTrainingController()
        {

        }

        [HttpPost]
        [Route("api/InsertGroupTraining")]
        public void InsertGroupTraining([FromBody]HistoryGroupTraining h)
        {
            h.InsertGroupTraining(h);
        }


        [HttpGet]
        [Route("api/GetFutureTrainings")]
        public IEnumerable<CoupleTraining> GetFutureTrainings(int UserCode)
        {
            CoupleTraining c = new CoupleTraining();
            return c.GetFutureTrainings(UserCode);
        }

    }
}
