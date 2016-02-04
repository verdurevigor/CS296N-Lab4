using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class TopicViewModel
    {
        List<Message> posts = new List<Message>();
        // Using virtual keyword is NOT necessary with ViewModels! Ok in domain model though.
        // Adding virtual means the propety isn't instantiated until it is set.
        public int TopicId { get; set; }
        public string Name { get; set; }

        public List<Message> Posts
        {
            get { return posts; }
            set { posts = value; }
        }
    }
}