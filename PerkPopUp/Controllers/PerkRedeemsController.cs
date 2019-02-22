using System.Data.Entity;
using System.Linq;
using System.Net;
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

                //notify all users
                var context = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                context.Clients.All.broadcastMessage( "Random user", "Redeemed: " + perkRedeem.PerkName);
                
                //TODO: How many users bought this in last 5 minutes?

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
