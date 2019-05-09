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

    public class CoupleTrainingSuggestionController : ApiController
    {
        public CoupleTrainingSuggestionController()
        {

        }
        [HttpGet]
        [Route("api/SendSuggestion")]
        public string SendSuggestion(int SenderCode, int ReceiverCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.SendSuggestion(SenderCode, ReceiverCode);
        }

        [HttpPost]
        [Route("api/ReplySuggestion")]
        public void ReplySuggestion(int SuggestionCode, bool reply)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            s.ReplySuggestion(SuggestionCode, reply);
        }

        [HttpGet]
        [Route("api/GetSuggestions")]
        public IEnumerable<SuggestionResult> GetSuggestions(int UserCode,  bool IsApproved)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.GetSuggestions(UserCode,  IsApproved);
        }


        [HttpGet]
        [Route("api/CheckActiveSuggestions")]
        public string  CheckActiveSuggestions(int SenderCode, int ReceiverCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.CheckActiveSuggestions(SenderCode,ReceiverCode);
        }

        [HttpPost]
        [Route("api/CancelSuggestion")]
        public void CancelSuggestion(  int SuggestionCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            s.CancelSuggestion(SuggestionCode);
        }

        // move to another controller ?!! 
        [HttpGet]
        [Route("api/GetFutureTrainings")]
        public IEnumerable<CoupleTraining> GetFutureTrainings(int UserCode)
        {
            CoupleTraining c = new CoupleTraining();
            return c.GetFutureTrainings(UserCode);
        }


    }
}
