using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using PerkPopUp.Models;

namespace PerkPopUp.Controllers
{
    public class PerkRedeemsController : Controller
    {
        private PerkPopUpContext db = new PerkPopUpContext();

        // GET: PerkRedeems
        public ActionResult Index()
        {
            return View(db.PerkRedeems.ToList());
        }

        // GET: PerkRedeems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerkRedeem perkRedeem = db.PerkRedeems.Find(id);
            if (perkRedeem == null)
            {
                return HttpNotFound();
            }
            return View(perkRedeem);
        }

        // GET: PerkRedeems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PerkRedeems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PerkName")] PerkRedeem perkRedeem)
        {
            if (ModelState.IsValid)
            {
                db.PerkRedeems.Add(perkRedeem);
                db.SaveChanges();

                var cache = MemoryCache.Default;
                //How many users bought this in last 5 minutes?
                //first, do i have this perk already redeemed in last 5 minutes
                int existingCount = 0;
                if (cache.Contains(perkRedeem.PerkName))
                {
                    var existingValue = cache[perkRedeem.PerkName].ToString();
                    existingCount = Convert.ToInt32(existingValue);
                    cache.Remove(perkRedeem.PerkName);
                }
                
                //second, add the new count
                var key = perkRedeem.PerkName;
                var perksRedeemed = existingCount + 1;
                var policy = new CacheItemPolicy { SlidingExpiration = new TimeSpan(5, 0, 0) };
                cache.Add(key, perksRedeemed, policy);
                
                
                //third, update the stock 
                string remainingMsg = null;
                var current = db.PerkDatas.FirstOrDefault(x => x.PerkName == perkRedeem.PerkName);
                if (current!=null)
                {
                    var stockQuantity = current.Quantity;
                    stockQuantity--;
                    remainingMsg = $"There are only {stockQuantity} {perkRedeem.PerkName} perks remaining!";
                    db.PerkDatas.Remove(current);
                    db.PerkDatas.Add(new PerkData
                        {Id = current.Id, PerkName = current.PerkName, Quantity = stockQuantity});
                    db.SaveChanges();
                }
                
                //notify all users
                var msg = $"{perkRedeem.PerkName} has been redeemed {perksRedeemed} times in the last 5 minutes!";
                //if (!string.IsNullOrEmpty(remainingMsg))
                //{
                //    msg = msg + Environment.NewLine + remainingMsg;
                //}
                var context = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                context.Clients.All.broadcastMessage(msg, remainingMsg);



                return RedirectToAction("Index");
            }

            return View(perkRedeem);
        }

        // GET: PerkRedeems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerkRedeem perkRedeem = db.PerkRedeems.Find(id);
            if (perkRedeem == null)
            {
                return HttpNotFound();
            }
            return View(perkRedeem);
        }

        // POST: PerkRedeems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PerkName")] PerkRedeem perkRedeem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perkRedeem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(perkRedeem);
        }

        // GET: PerkRedeems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerkRedeem perkRedeem = db.PerkRedeems.Find(id);
            if (perkRedeem == null)
            {
                return HttpNotFound();
            }
            return View(perkRedeem);
        }

        // POST: PerkRedeems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PerkRedeem perkRedeem = db.PerkRedeems.Find(id);
            db.PerkRedeems.Remove(perkRedeem);
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
