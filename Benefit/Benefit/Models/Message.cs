using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Message
    {
        public int ChatCode { get; set; }
        public int SenderCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public Message(int _chatCode , int _senderCode, DateTime _date, DateTime _time, string _content)
        {
            ChatCode = _chatCode;
            SenderCode = _senderCode;
            Date = _date;
            Time = _time;
            Content = _content;
            
        }


    }
}