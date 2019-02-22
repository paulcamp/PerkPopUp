using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PerkPopUp.Models;

namespace PerkPopUp.Controllers
{
    public class PerkDatasController : Controller
    {
        private PerkPopUpContext db = new PerkPopUpContext();

        // GET: PerkDatas
        public ActionResult Index()
        {
            return View(db.PerkDatas.ToList());
        }

        // GET: PerkDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerkData perkData = db.PerkDatas.Find(id);
            if (perkData == null)
            {
                return HttpNotFound();
            }
            return View(perkData);
        }

        // GET: PerkDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PerkDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PerkName,Quantity")] PerkData perkData)
        {
            if (ModelState.IsValid)
            {
                db.PerkDatas.Add(perkData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(perkData);
        }

        // GET: PerkDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerkData perkData = db.PerkDatas.Find(id);
            if (perkData == null)
            {
                return HttpNotFound();
            }
            return View(perkData);
        }

        // POST: PerkDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PerkName,Quantity")] PerkData perkData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perkData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(perkData);
        }

        // GET: PerkDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerkData perkData = db.PerkDatas.Find(id);
            if (perkData == null)
            {
                return HttpNotFound();
            }
            return View(perkData);
        }

        // POST: PerkDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PerkData perkData = db.PerkDatas.Find(id);
            db.PerkDatas.Remove(perkData);
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
