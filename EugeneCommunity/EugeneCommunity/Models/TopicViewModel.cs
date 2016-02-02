using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class TopicViewModel
    {
        List<Message> posts = new List<Message>();
        public virtual int TopicId { get; set; }
        public virtual string Name { get; set; }

        public virtual List<Message> Posts
        {
            get { return posts; }
            set { posts = value; }
        }
    }
}