using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class EugeneCommunityInitializer : System.Data.Entity.DropCreateDatabaseAlways<EugeneCommunityContext>
    {
        protected override void Seed(EugeneCommunityContext context)
        {
            Topic t1 = new Topic { TopicId = 1, Name = "IPA Crave" };
            Topic t2 = new Topic { TopicId = 2, Name = "Beer Tasting" };
            context.Topics.Add(t1);
            context.Topics.Add(t2);

            Member m1 = new Member { MemberId = 1, Name = "Brody", Email = "brodyjcase@gmail.com", Password = "Passw0rd", IsAdmin = true};
            Member m2 = new Member { MemberId = 2, Name = "Zach", Email = "beerlove@gmail.com", Password = "Passw0rd", IsAdmin = false };
            context.Members.Add(m1);
            context.Members.Add(m2);

            context.Messages.Add(new Message { 
                MessageId = 1,
                TopicId = 1,
                Body = "HUB (Hopworks Urban Brewery) makes my favorite IPA as of now. They source a single hop called the Simcoe, it's grown in Washington. Not to bitter, but a smooth, crisp flavor jumps out at you. I love all the organic beer that HUB crafts. Give it a try!",
                Date = DateTime.Now.AddHours(2),
                MemberId = 1
            });

            context.Messages.Add(new Message
            {
                MessageId = 2,
                TopicId = 2,
                Body = "With so many pourhouses here in Eugene, where do you even start?! Well, I'll give you a shortened list to tackle: Falling Sky Delicatessen, The Bier Stein, The Tap and Growler. Pick one and have fun tasting!",
                Date = DateTime.Now.AddHours(4),
                MemberId = 2
            });

            context.Messages.Add(new Message
            {
                MessageId = 3,
                TopicId = 1,
                Body = "While typically a Summer seasonal beer, Pelican Brewery currently is putting out their 'Umbrella' IPA. It's supreme. Although, pelicans really are nasty birds.",
                Date = DateTime.Now.AddMinutes(30),
                MemberId = 2
            });

            context.Messages.Add(new Message
            {
                MessageId = 3,
                TopicId = 2,
                Body = "Another great taphouse is 16 Tons. They're location off Willamette St (next to Market of Chores) is also a cafe. What goes better than anything else with beer? In my opinion, crepes! And 16 Tons has those! So, go pair a porter with a sweet crepe for something new.",
                Date = DateTime.Now.AddMinutes(50),
                MemberId = 1
            });

            base.Seed(context);
        }
    }
}