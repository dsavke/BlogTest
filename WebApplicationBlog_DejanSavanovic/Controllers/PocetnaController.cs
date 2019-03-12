using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationBlog_DejanSavanovic.DBModels;
using WebApplicationBlog_DejanSavanovic.Models;

namespace WebApplicationBlog_DejanSavanovic.Controllers
{
    public class PocetnaController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new BlogContext())
            {
                var pocetnaViewModel = new PocetnaViewModel()
                {
                    Drzave = context.Drzavas
                    .Select(d => new SelectListItem()
                    {
                        Text = d.Naziv,
                        Value = "" + d.DrzavaId
                    }).ToList(),
                    TopPetBlogova = context.Blogs.Where(b => b.Odobren == true)
                    .Select(b => new TopPetBlogovaViewModel()
                    {
                        Naslov = b.Naslov,
                        BrojSvidjanja = b.Korisniks.Count()
                    }).OrderByDescending(p => p.BrojSvidjanja).Take(5).ToList(),
                    TopPetAutora = context.Korisniks.Where(k => k.Blogs.Count != 0)
                    .Select(k => new TopPetAutoraViewModel()
                    {
                        KorisnickoIme = k.KorisnickoIme,
                        BrojSvidjanja = k.Blogs.ToList().Where(b => b.Korisniks.Count != 0).Sum(b => b.Korisniks.Count)
                    }).OrderByDescending(a => a.BrojSvidjanja).Take(5).ToList()
                };

                pocetnaViewModel.Drzave.Insert(0, new SelectListItem() { Text = "Drzava", Value = "-1" });

                return View(pocetnaViewModel);
            }
        }

        public PartialViewResult Pretraga(string tekstPretraga, string drzavaID)
        {
            using(var context = new BlogContext())
            {
                var drzavaId = Convert.ToInt32(drzavaID);
                var odobreniBlogovi = context.Blogs
                    .Where(b => b.Odobren == true && (tekstPretraga.Trim() == "" || b.Naslov.Contains(tekstPretraga)) && 
                            (drzavaId == -1 || b.DrzavaId == drzavaId))
                    .Select(b => new OdobreniBlogoviViewModel()
                    {
                        BlogId = b.BlogId,
                        DatumKreiranja = b.DatumKreiranja,
                        Naslov = b.Naslov,
                        NaslovSlikaLink = b.NaslovnaSlikaLink,
                        KorisnickoIme = b.Korisnik.KorisnickoIme
                    }).ToList();
                return PartialView("_OdobreniBlogovi", odobreniBlogovi);
            }
        }

        [AllowAnonymous]
        public JsonResult PromijeniJezik(string lang)
        {
            HttpCookie myCookie = new HttpCookie("Jezik");
            DateTime now = DateTime.Now;

            myCookie.Value = lang;

            myCookie.Expires = now.AddMonths(10);

            Response.Cookies.Add(myCookie);

            return Json(new { Success = true });

        }

    }
}