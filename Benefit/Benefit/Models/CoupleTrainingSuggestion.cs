using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class CoupleTrainingSuggestion
    {
        public int SuggestionCode { get; set; }
        public int SenderCode { get; set; }
        public int ReceiverCode { get; set; }
        public int StatusCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public CoupleTrainingSuggestion(int _senderCode, int _receiverCode, int _statusCode, DateTime _date, DateTime _time)
        {
            SenderCode = _senderCode;
            ReceiverCode = _receiverCode;
            StatusCode = _statusCode;
            Date = _date;
            Time = _time;
        }


    }
}