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
        

		[HttpPost]
		[Route("api/UpdateSuggestionsStatus")]
		public void UpdateSuggestionsStatus()
		{
			CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
			s.UpdateSuggestionsStatus();
		}

        [HttpPost]
        [Route("api/CancelSuggestion")]
        public void CancelSuggestion(  int SuggestionCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            s.CancelSuggestion(SuggestionCode);
        }

    }
}
