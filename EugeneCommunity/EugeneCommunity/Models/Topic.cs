using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Topic
    {
        private List<Message> posts = new List<Message>();
        public virtual int TopicId { get; set; }
        public virtual string Name { get; set; }
        
        public List<Message> Posts
        {
            get { return posts; }
        }
    }
}