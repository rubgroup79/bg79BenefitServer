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

    public class OnlineHistoryTraineeController : ApiController
    {
        public OnlineHistoryTraineeController()
        {

        }

        [HttpPost]
        [Route("api/InsertOnlineTrainee")]
        public IEnumerable<Trainee> InsertOnlineTrainee([FromBody]OnlineHistoryTrainee o)
        {
           return o.InsertOnlineTrainee(o);
        }

        

    }
}
