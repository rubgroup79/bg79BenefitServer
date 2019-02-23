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

    public class UserController : ApiController
    {
        public UserController()
        {

        }

        [HttpPost]
        [Route("api/User/InsertTrainer")]
        public int SignInTrainer(Trainer t)
        {
            return t.SignInTrainer(t);
        }
        [HttpPost]
        [Route("api/User/InsertTrainee")]
        public int  SignInTrainee(Trainee t)
        {
           return t.SignInTrainee(t);
        }


    }
}
