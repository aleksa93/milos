using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Controllers
{
    public class KorisnikController : Controller
    {
        private IEPBazaEntities12 baza = new IEPBazaEntities12();

        // GET: /Kontroler/
        public ActionResult Index()
        {
            return View(baza.Korisnik.ToList());
        }
        
        public ActionResult IzmeniPodatke()
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik korisnik = baza.Korisnik.Find(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }
            return View(korisnik);
        }

        // POST: /Kontroler/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzmeniPodatke([Bind(Include = "IDKorisnik,Ime,Prezime,eMail,Mesto,Adresa,Telefon,MailStize")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                baza.Entry(korisnik).State = EntityState.Modified;
                baza.SaveChanges();
                return RedirectToAction("Profil", "Account");
            }
            return View(korisnik);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                baza.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
