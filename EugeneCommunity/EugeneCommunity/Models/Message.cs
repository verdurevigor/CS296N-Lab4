using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Message
    {
        [Key]
        public virtual int MessageId { get; set; }
        public virtual string Body { get; set; }
        public virtual DateTime Date { get; set; }
        // FK to Topic
        public virtual int TopicId { get; set; }
        // FK to Member
        public virtual int MemberId { get; set; }
    }
}