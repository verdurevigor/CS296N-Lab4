using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EugeneCommunity.Models;

namespace EugeneCommunity.Controllers
{
    public class MessagesController : Controller
    {
        private EugeneCommunityContext db = new EugeneCommunityContext();

        // GET: Messages
        public ActionResult Index()
        {
            // Query db for list of messages, attaching user and subject to the message
            var messages = (from m in db.Messages
                            orderby m.Date
                            select new MessageViewModel
                            {
                                MessageId = m.MessageId,
                                Body = m.Body,
                                Date = m.Date,
                                Subject = (from t in db.Topics
                                           where m.TopicId == t.TopicId
                                           select t).FirstOrDefault(),
                                User = (from u in db.Members
                                        where m.MemberId == u.MemberId
                                        select u).FirstOrDefault()
                            }).ToList();
            // Order messages by most recent            Not sure if this is working...
            messages.OrderBy(m => m.Date);
            return View(messages);
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Query db for message matching id parameter and include Member and Topic
            var message = (from m in db.Messages
                            where id == m.MessageId
                            select new MessageViewModel
                            {
                                MessageId = m.MessageId,
                                Body = m.Body,
                                Date = m.Date,
                                Subject = (from t in db.Topics
                                           where m.TopicId == t.TopicId
                                           select t).FirstOrDefault(),
                                User = (from u in db.Members
                                        where m.MemberId == u.MemberId
                                        select u).FirstOrDefault()
                            }).FirstOrDefault();
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create(int? topicId)
        {
            // Preselect the topic in the SelectList if client came from a Topic Details view.
            if (topicId != null)
               ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", topicId);
            else
                ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title");

            // For now, send a SelectList of users for client to use as an identity
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Title,Body,Date,TopicId,MemberId")] MessageViewModel messageVm, int? CurrentTopics, int? CurrentUsers)
        {           
            if (ModelState.IsValid)
            {
                // Check if the topic is new, if so create and save a new topic to the database before adding the TopicId to the new message
                Topic topic = (from t in db.Topics
                               where t.TopicId == CurrentTopics
                               select t).FirstOrDefault();
                
                if (topic == null)
                {
                    topic = new Topic() { Title = messageVm.Subject.Title };
                    db.Topics.Add(topic);
                    db.SaveChanges();
                }

                // Using the MessageViewModel input, create a message object
                Message message = new Message()
                {
                    Body = messageVm.Body,
                    Date = DateTime.Now,
                    MemberId = (int)CurrentUsers,       // Cast possibly null int, app will still crash if it is not filled when form is submitted. TODO: modify when Member is stored in session
                    TopicId = topic.TopicId
                };

                // Add and save Message to db
                db.Messages.Add(message);
                db.SaveChanges();

                // Redirect user to the forum topic which they just added a message to
                return RedirectToAction("Details", "Topics", new { id = topic.TopicId });
            }

            return View(messageVm);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create MessageViewModel from the MessageId to pass to the view
            MessageViewModel message = (from m in db.Messages
                                        where m.MessageId == id
                                        select new MessageViewModel(){
                                            Body = m.Body,
                                            Date = m.Date,
                                            Subject = (from t in db.Topics
                                                       where m.TopicId == t.TopicId
                                                       select t).FirstOrDefault(),
                                            User = (from u in db.Members
                                                    where m.MemberId == u.MemberId
                                                    select u).FirstOrDefault()
                                        }).FirstOrDefault();
            if (message == null)
            {
                return HttpNotFound();
            }
            // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", message.Subject.TopicId);

            // For now, send a SelectList of users for client to use as an identity
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,Body,Date,Subject,User")] MessageViewModel messageVm, int? CurrentUsers)
        {
            if (ModelState.IsValid)
            {
                if (messageVm.User.MemberId == CurrentUsers)
                {
                    Message message = new Message()
                    {
                        MemberId = messageVm.User.MemberId,
                        TopicId = messageVm.Subject.TopicId,
                        MessageId = messageVm.MessageId,
                        Date = DateTime.Now,
                        Body = messageVm.Body
                    };

                    db.Entry(message).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else//Not same user who posted
                {
                    // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
                    ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", messageVm.Subject.TopicId);

                    // For now, send a SelectList of users for client to use as an identity
                    ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
                    return View(messageVm);
                }
            }            
            // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", messageVm.Subject.TopicId);

            // For now, send a SelectList of users for client to use as an identity
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View(messageVm);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Query db for message matching id parameter and include Member and Topic to create a full MessageViewModel
            var message = (from m in db.Messages
                           where id == m.MessageId
                           select new MessageViewModel
                           {
                               MessageId = m.MessageId,
                               Body = m.Body,
                               Date = m.Date,
                               Subject = (from t in db.Topics
                                          where m.TopicId == t.TopicId
                                          select t).FirstOrDefault(),
                               User = (from u in db.Members
                                       where m.MemberId == u.MemberId
                                       select u).FirstOrDefault()
                           }).FirstOrDefault();
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Books/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: Books/Search/searchString
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            // Get the books that matches the searchTerm
            List<MessageViewModel> messageVms = (from m in db.Messages
                                                 where m.Body.Contains(searchTerm)
                                                 select new MessageViewModel()
                                                 {
                                                     MessageId = m.MessageId,
                                                     Body = m.Body,
                                                     Date = m.Date,
                                                     Subject = (from t in db.Topics
                                                                where m.TopicId == t.TopicId
                                                                select new Topic() {
                                                                    TopicId = t.TopicId,
                                                                    Title = t.Title
                                                                }).FirstOrDefault(),
                                                    User = (from u in db.Members
                                                            where u.MemberId == m.MessageId
                                                            select new Member()
                                                            {
                                                                MemberId = u.MemberId,
                                                                UserName = u.UserName
                                                            }).FirstOrDefault()
                                                 }).ToList();

            if (messageVms.Count() > 0)
            {
                return View("Details", messageVms);  // Return to the view, lsit of MessageViewModels
            }
            // Otherwise, nothing was found so return................................
            else
            {
                return View("Details", messageVms);
            }
            // Partial Views are the perfect thing to do when trying to show the search result page based on the index. But for now just make a new View for the return.
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
