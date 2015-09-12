using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using System;
using PagedList;
using System.Web;
using System.Net.Mail;

namespace WebApplication1.Controllers
{
    

    public class OglasController : Controller
    {
        public IEPBazaEntities12 baza = new IEPBazaEntities12();

        

        public ActionResult Index(string currentFilter1, string currentFilter2, string currentFilter3, string searchString1, string searchString2, string searchString3, int? page)
        {

            if (searchString1 != null)
            {
                page = 1;
            }
            else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.CurrentFilter1 = searchString1;

            if (searchString2 != null)
            {
                page = 1;
            }
            else
            {
                searchString2 = currentFilter2;
            }

            ViewBag.CurrentFilter2 = searchString2;

            if (searchString3 != null)
            {
                page = 1;
            }
            else
            {
                searchString3 = currentFilter3;
            }

            ViewBag.CurrentFilter3 = searchString3;

            var oglasi = from o in baza.Oglas
                         select o;

            oglasi = oglasi.Where(o => o.Status == 1);

            if (!String.IsNullOrEmpty(searchString1))
            {
                oglasi = oglasi.Where(o => o.Naslov.ToUpper().Contains(searchString1.ToUpper())
                                       || o.Opis.ToUpper().Contains(searchString1.ToUpper()));
            }
            if (!String.IsNullOrEmpty(searchString2))
            {
                var min = double.Parse(searchString2);
                oglasi = oglasi.Where(o => (o.Cena >= min));
            }
            if (!String.IsNullOrEmpty(searchString3))
            {
                var max = double.Parse(searchString3);
                oglasi = oglasi.Where(o => (o.Cena <= max));
            }
            
            oglasi = oglasi.OrderBy(o => o.Kategorija);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(oglasi.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult MojiOglasi()
        {
            var oglasi = from o in baza.Oglas
                         select o;
            var id = User.Identity.GetUserId();
            oglasi = oglasi.Where(o => o.IDKorisnik.Equals(id));
            return View(oglasi.ToList());
        }

        // GET: /Oglas/Create
        public ActionResult NoviOglas()
        {
            
            List<SelectListItem> kategorije = new List<SelectListItem>();
            kategorije.Add(new SelectListItem { Text = "Ves masine", Value = "Ves masine" });
            kategorije.Add(new SelectListItem { Text = "Racunari", Value = "Racunari" });
            kategorije.Add(new SelectListItem { Text = "Televizori", Value = "Televizori" });
            kategorije.Add(new SelectListItem { Text = "Frizideri", Value = "Frizideri" });
            kategorije.Add(new SelectListItem { Text = "Mikrotalasne rerne", Value = "Mikrotalasne rerne" });
            kategorije.Add(new SelectListItem { Text = "Ostali uredjaji", Value = "Ostali uredjaji" });
            ViewBag.Kategorija = kategorije;

            ViewBag.IDKorisnik = new SelectList(baza.Korisnik, "IDKorisnik", "Ime");
            return View();
        }

        // POST: /Oglas/NoviOglas
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NoviOglas([Bind(Include = "IDOglas,IDKorisnik,Kategorija,Naslov,Opis,Cena,Kolicina,Status,Datum,LinkSlika")] Oglas oglas)
        {
            if (ModelState.IsValid)
            {
                oglas.IDKorisnik = User.Identity.GetUserId();
                oglas.Status = 1;
                oglas.Datum = System.DateTime.Now;
                baza.Oglas.Add(oglas);
                
                baza.SaveChanges();
                int id = oglas.IDOglas;
                string dat = oglas.Datum.ToString();
                Korisnik kor = baza.Korisnik.Find(User.Identity.GetUserId());
                if (kor.MailStize == 1) 
                {
                    posaljiEMail(id, dat, kor.eMail);
                }
                
                return RedirectToAction("MojiOglasi");
            }
            

            ViewBag.IDKorisnik = new SelectList(baza.Korisnik, "IDKorisnik", "Ime", oglas.IDKorisnik);
            return View(oglas);
        }

        public ActionResult MojiOglasiDetaljno(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oglas oglas = baza.Oglas.Find(id);
            if (oglas == null)
            {
                return HttpNotFound();
            }
            return View(oglas);
        }

        public ActionResult OglasDetalji(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oglas oglas = baza.Oglas.Find(id);
            if (oglas == null)
            {
                return HttpNotFound();
            }
            return View(oglas);
        }

        public ActionResult Naruci(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = new Narudzbina();
            narudzbina.IDOglas = Convert.ToInt32(id);
            narudzbina.IDKorisnik = User.Identity.GetUserId();
            narudzbina.Status = "Cekanje na obradu";
            narudzbina.Vreme = System.DateTime.Now;
            baza.Narudzbina.Add(narudzbina);
            baza.SaveChanges();
            return View();
        }

        public void posaljiEMail(int IDOglas, string Datum, string eMail)
        {

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(eMail);
            message.Subject = "Obavestenje";
            message.From = new System.Net.Mail.MailAddress("iepprojekatma120245@yahoo.com");
            message.Body = "Obavestavamo Vas da je Vas oglas (id="+IDOglas.ToString()+") uspesno postavljen. /n Vreme postavljanja: "+Datum+"/n URL: "+string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "Oglas/Detalji/" + IDOglas.ToString() + "\n";
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.yahoo.com");
            smtp.Credentials = new System.Net.NetworkCredential("iepprojekatma120245", "Aleksa0608");
            smtp.EnableSsl = true;
            smtp.Send(message);

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