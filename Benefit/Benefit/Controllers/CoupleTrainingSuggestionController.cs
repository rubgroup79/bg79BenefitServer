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

        //sender=true כדי לקבל את ההצעות ששלח היוזר 
        // sender= false כדי לקבל את ההצעות שנשלחו לו 
        [HttpGet]
        [Route("api/GetPendingSuggestions")]
        public IEnumerable<CoupleTrainingSuggestion> GetPendingSuggestions(int UserCode, bool Sender)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.GetPendingSuggestions(UserCode, Sender);
        }

        [HttpGet]
        [Route("api/GetSuggestionDetails")]
        public IEnumerable<Result> GetSuggestionDetails(int SuggestionCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.GetSuggestionDetails(SuggestionCode);
        }

    }
}
