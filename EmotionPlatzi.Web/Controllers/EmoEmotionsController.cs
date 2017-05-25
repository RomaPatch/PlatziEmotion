﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmotionPlatzi.Web.Models;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoEmotionsController : Controller
    {
        private EmotionPlatziWebContext db = new EmotionPlatziWebContext();

        // GET: EmoEmotions
        public ActionResult Index()
        {
            var emoEmotion = db.EmoEmotion.Include(e => e.Face);
            return View(emoEmotion.ToList());
        }

        // GET: EmoEmotions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmoEmotion emoEmotion = db.EmoEmotion.Find(id);
            if (emoEmotion == null)
            {
                return HttpNotFound();
            }
            return View(emoEmotion);
        }

        // GET: EmoEmotions/Create
        public ActionResult Create()
        {
            ViewBag.EmoFaceId = new SelectList(db.EmoFaces, "Id", "Id");
            return View();
        }

        // POST: EmoEmotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Score,EmoFaceId,EmotionType")] EmoEmotion emoEmotion)
        {
            if (ModelState.IsValid)
            {
                db.EmoEmotion.Add(emoEmotion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmoFaceId = new SelectList(db.EmoFaces, "Id", "Id", emoEmotion.EmoFaceId);
            return View(emoEmotion);
        }

        // GET: EmoEmotions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmoEmotion emoEmotion = db.EmoEmotion.Find(id);
            if (emoEmotion == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmoFaceId = new SelectList(db.EmoFaces, "Id", "Id", emoEmotion.EmoFaceId);
            return View(emoEmotion);
        }

        // POST: EmoEmotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Score,EmoFaceId,EmotionType")] EmoEmotion emoEmotion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emoEmotion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmoFaceId = new SelectList(db.EmoFaces, "Id", "Id", emoEmotion.EmoFaceId);
            return View(emoEmotion);
        }

        // GET: EmoEmotions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmoEmotion emoEmotion = db.EmoEmotion.Find(id);
            if (emoEmotion == null)
            {
                return HttpNotFound();
            }
            return View(emoEmotion);
        }

        // POST: EmoEmotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmoEmotion emoEmotion = db.EmoEmotion.Find(id);
            db.EmoEmotion.Remove(emoEmotion);
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
