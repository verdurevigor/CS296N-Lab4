using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        [Required(ErrorMessage = "The {0} of the message is required.")]
        public string Body { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime Date { get; set; }

        public Topic Subject { get; set; }
        public Member User { get; set; }
    }
}