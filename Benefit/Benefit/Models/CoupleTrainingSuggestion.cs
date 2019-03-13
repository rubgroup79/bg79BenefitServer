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
        public DateTime SendingTime { get; set; }

        public CoupleTrainingSuggestion(int _senderCode, int _receiverCode, int _statusCode, DateTime _sendingTime)
        {
            SenderCode = _senderCode;
            ReceiverCode = _receiverCode;
            StatusCode = _statusCode;
            SendingTime = _sendingTime;
        }


    }
}