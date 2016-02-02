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
    public class TopicsController : Controller
    {
        private EugeneCommunityContext db = new EugeneCommunityContext();

        // GET: Topics
        public ActionResult Index()
        {
            return View(db.Topics.ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Use linq query to retrieve the topic of given id. Retrieve list of posts on that topic (posts with that topic's fk).
            // Set the topics posts with the retrieved list of messages.
            // Then pass this topic with list of messages to the View.

            TopicViewModel topic = new TopicViewModel();
            /*topic = (from t in db.Topics
                     where t.TopicId == id
                     select new TopicViewModel { 
                        TopicId = t.TopicId,
                        Name = t.Name,
                        Posts = new List<Message> {
                            from m in db.Messages
                            where m.TopicId == id
                            select m
                        }.FindAll()
                     }).FirstOrDefault();*/
            topic = (from t in db.Topics
                     where t.TopicId == id
                     select new TopicViewModel
                     {
                         TopicId = t.TopicId,
                         Name = t.Name,
                         // attempting to add list of messages here
                         Posts = (from p in db.Messages
                                  where p.TopicId == id
                                  select p).ToList()
                     }).FirstOrDefault();
            /*List<Message> p = new List<Message>();
            var messagesQuery = from m in db.Messages
                  where m.TopicId == id
                  select m;

            foreach (var message in messagesQuery)
            {
                p.Add(message);
            }*/

            // Attempting to query without an execution foreach statement -- Works!
            /* Now attempting to get list of books in single linq query above.
            List<Message> me = (from m in db.Messages
                                where m.TopicId == id
                                select m).ToList();
            
            topic.Posts = me;
*/         
            //Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicId,Name")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,Name")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Index");
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
