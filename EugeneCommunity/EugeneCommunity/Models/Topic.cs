using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Topic
    {
        private List<Message> posts = new List<Message>();
        [Key]
        public virtual int TopicId { get; set; }
        [Display(Name="Topic Title")]
        public virtual string Title { get; set; } 
        public List<Message> Posts
        {
            get { return posts; }
        }
    }
}