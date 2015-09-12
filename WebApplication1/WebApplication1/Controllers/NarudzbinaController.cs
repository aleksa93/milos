using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System.IO;
using System.Net.Mail;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WebApplication1.Controllers
{
    public class NarudzbinaController : Controller
    {

        private IEPBazaEntities12 baza = new IEPBazaEntities12();
        //
        // GET: /Narudzbina/
        public ActionResult Index(string sortOrder, int? page)
        {

            var narudzbine = from n in baza.Narudzbina
                             select n;
            var id = User.Identity.GetUserId();
            narudzbine = narudzbine.Where(n => n.IDKorisnik.Equals(id));
            narudzbine = narudzbine.OrderBy(n => n.Status);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(narudzbine.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detalji(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = baza.Narudzbina.Find(id);
            ViewBag.test = "Cekanje na obradu";
            if (narudzbina == null)
            {
                return HttpNotFound();
            }
            return View(narudzbina);
        }

        public ActionResult izveziUPdf(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = baza.Narudzbina.Find(id);
            if (narudzbina == null)
            {
                return HttpNotFound();
            }

            MemoryStream fs = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);

            document.Open();
            document.Add(new Paragraph());

            PdfPTable table = new PdfPTable(2);
            table.SpacingBefore = 30;

            PdfPCell cell01 = new PdfPCell(new Phrase("Identifikator narudzbine"));
            table.AddCell(cell01);
            PdfPCell cell02 = new PdfPCell(new Phrase(narudzbina.IDNarudzbina.ToString()));
            table.AddCell(cell02);
            PdfPCell cell03 = new PdfPCell(new Phrase("Trenutni status"));
            table.AddCell(cell03);
            PdfPCell cell04 = new PdfPCell(new Phrase(narudzbina.Status));
            table.AddCell(cell04);
            PdfPCell cell05 = new PdfPCell(new Phrase("Vreme promene statusa"));
            table.AddCell(cell05);
            PdfPCell cell06 = new PdfPCell(new Phrase(narudzbina.Vreme.ToString()));
            table.AddCell(cell06);
            PdfPCell cell1 = new PdfPCell(new Phrase("Cena"));
            table.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase(narudzbina.Oglas.Cena.ToString()));
            table.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Trenutna kolicina"));
            table.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase(narudzbina.Oglas.Kolicina.ToString()));
            table.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Ime"));
            table.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase(narudzbina.Korisnik.Ime));
            table.AddCell(cell6);
            PdfPCell cell15 = new PdfPCell(new Phrase("Prezime"));
            table.AddCell(cell15);
            PdfPCell cell16 = new PdfPCell(new Phrase(narudzbina.Korisnik.Prezime));
            table.AddCell(cell16);
            PdfPCell cell7 = new PdfPCell(new Phrase("Mesto"));
            table.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase(narudzbina.Korisnik.Mesto));
            table.AddCell(cell8);
            PdfPCell cell07 = new PdfPCell(new Phrase("Telefon"));
            table.AddCell(cell07);
            PdfPCell cell08 = new PdfPCell(new Phrase(narudzbina.Korisnik.Telefon));
            table.AddCell(cell08);
            PdfPCell cell9 = new PdfPCell(new Phrase("Link ka slici"));
            table.AddCell(cell9);
            PdfPCell cell10 = new PdfPCell(new Phrase(narudzbina.Oglas.LinkSlika));
            table.AddCell(cell10);


            document.Add(table);
            document.Close();

            return new FileContentResult(fs.ToArray(), "application/pdf");
        }

        public void posaljiEMail(Narudzbina narudzbina)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(narudzbina.Oglas.Korisnik.eMail);
            message.Subject = "Obavestenje";
            message.From = new System.Net.Mail.MailAddress("iepprojekatma120245@yahoo.com");
            message.Body = "Obavestavamo vas da je vas proizvod uspesno prodat";
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.yahoo.com");
            smtp.Credentials = new System.Net.NetworkCredential("iepprojekatma120245", "Aleksa0608");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
        public HttpStatusCodeResult Placanje(long clientid, long transactionid, string status)
        {
            if (clientid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = baza.Narudzbina.Find(Convert.ToInt32(clientid));
            ViewBag.Realizovana = narudzbina.IDNarudzbina;

            if (status == "success")
            {
                narudzbina.Status = "Realizovana";
                narudzbina.Vreme = System.DateTime.Now;
                narudzbina.Oglas.Kolicina = narudzbina.Oglas.Kolicina - 1;
                if (narudzbina.Oglas.Kolicina == 0)
                {
                    narudzbina.Oglas.Status = 0;

                    var nar = from n in baza.Narudzbina
                                 select n;

                    if (nar.Where(n => n.IDOglas == narudzbina.IDOglas).Where(n => n.Status.Contains("Cekanje")).Any())
                    {

                        nar = nar.Where(n => n.IDOglas == narudzbina.IDOglas).Where(n => n.Status.Contains("Cekanje"));

                        foreach (Narudzbina k in nar)
                        {
                            k.Status = "Ponistena";
                        }

                    }


                }
                if (narudzbina.Oglas.Korisnik.MailStize==1)
                {
                    posaljiEMail(narudzbina);
                }
                baza.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                narudzbina.Status = "Ponistena";
                narudzbina.Vreme = System.DateTime.Now;
                baza.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }


        public ActionResult UspesnoPlacanje(long clientid)
        {
            if (clientid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = baza.Narudzbina.Find(Convert.ToInt32(clientid));
            ViewBag.Realizovana = narudzbina.IDNarudzbina;
            narudzbina.Status = "Realizovana";
                narudzbina.Vreme = System.DateTime.Now;
                narudzbina.Oglas.Kolicina = narudzbina.Oglas.Kolicina - 1;
                baza.SaveChanges();
                if (narudzbina.Oglas.Kolicina == 0)
                {
                    narudzbina.Oglas.Status = 0;

                    var nar = from n in baza.Narudzbina
                              select n;

                    if (nar.Where(n => n.IDOglas == narudzbina.IDOglas).Where(n => n.Status.Contains("Cekanje")).Any())
                    {

                        nar = nar.Where(n => n.IDOglas == narudzbina.IDOglas).Where(n => n.Status.Contains("Cekanje"));

                        foreach (Narudzbina k in nar)
                        {
                            k.Status = "Ponistena";
                        }

                    }


                }
                if (narudzbina.Oglas.Korisnik.MailStize == 1)
                {
                    posaljiEMail(narudzbina);
                }
                baza.SaveChanges();

            if (narudzbina == null)
            {
                return HttpNotFound();
            }
            return View(narudzbina);
        }

        public ActionResult NeuspesnoPlacanje(long clientid)
        {
            Narudzbina narudzbina = baza.Narudzbina.Find(Convert.ToInt32(clientid));
            narudzbina.Status = "Ponistena";
            narudzbina.Vreme = System.DateTime.Now;
            baza.SaveChanges();
            return View();
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