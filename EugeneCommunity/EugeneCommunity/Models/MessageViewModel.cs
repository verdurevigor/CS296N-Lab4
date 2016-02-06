using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public Topic Subject { get; set; }
        public Member User { get; set; }
    }
}