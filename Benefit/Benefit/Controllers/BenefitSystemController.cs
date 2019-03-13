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

    public class BenefitSystemController : ApiController
    {
        public BenefitSystemController()
        {

        }

        [HttpPost]
        [Route("api/CheckIfEmailExists")]
        public bool CheckIfEmailExists(string Email)
        {
            BenefitSystem s = new BenefitSystem();
            return s.CheckIfEmailExists(Email);


        }
        

        [HttpPost]
        [Route("api/CheckIfPasswordMatches")]
        public Trainee CheckIfPasswordMatches([FromBody]Trainee t)
        {
            BenefitSystem s = new BenefitSystem();
            return s.CheckIfPasswordMatches(t.Email, t.Password);


        }
        

        //[HttpGet]
        //[Route("api/SearchPartners")]
        //public IEnumerable<User> SearchPartners([FromBody]CurrentOnlineTrainee o)
        //{
        //    BenefitSystem s = new BenefitSystem();
        //    return s.SearchPartners(o);
        //}
    }
}
