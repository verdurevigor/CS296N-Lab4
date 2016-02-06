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
        public ActionResult Create()
        {
            // Use the ViewBag to pass a collection of Topic objects for the dropdownlist
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Subject,Body,Date")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,Subject,Body,Date")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
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
