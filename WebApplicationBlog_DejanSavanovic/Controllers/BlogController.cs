using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationBlog_DejanSavanovic.DBModels;
using WebApplicationBlog_DejanSavanovic.Models;

namespace WebApplicationBlog_DejanSavanovic.Controllers
{
    public class BlogController : Controller
    {
        [Authorize(Roles = "Administrator, Korisnik")]
        public ActionResult Create()
        {

            using (var context = new BlogContext())
            {
                ViewBag.Drzave = context.Drzavas
                   .Select(d => new SelectListItem()
                   {
                       Text = d.Naziv,
                       Value = "" + d.DrzavaId
                   }).ToList();

                return View();

            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Korisnik")]
        public ActionResult Create(BlogViewModel blogViewModel)
        {
            using (var context = new BlogContext())
            {

                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == User.Identity.Name);

                Blog blog = new Blog()
                {
                    DatumKreiranja = DateTime.Now,
                    Odobren = false,
                    KorisnikId = korisnik.KorisnikId,
                    Sadrzaj = blogViewModel.Sadrzaj,
                    DatumPutovanja = blogViewModel.DatumPutovanja,
                    DrzavaId = blogViewModel.DrzavaId,
                    Naslov = blogViewModel.Naslov,
                    NaslovnaSlikaLink = blogViewModel.NaslovnaSlikaLink
                };

                context.Blogs.Add(blog);
                context.SaveChanges();

                return RedirectToAction("Index", "Pocetna");
            }
        }

        public ActionResult Details(string id)
        {
            using (var context = new BlogContext())
            {
                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == User.Identity.Name);

                var blogID = Convert.ToInt32(id);
                var blog = context.Blogs.Find(blogID);
                if (blog.Odobren)
                {

                    var blogViewModel = new BlogDetailsViewModel()
                    {
                        BlogId = blog.BlogId,
                        DatumPutovanja = blog.DatumPutovanja,
                        Drzava = blog.Drzava.Naziv,
                        Autor = blog.Korisnik.Ime + " " + blog.Korisnik.Prezime,
                        DatumKreiranja = blog.DatumKreiranja,
                        Naslov = blog.Naslov,
                        NaslovnaSlikaLink = blog.NaslovnaSlikaLink,
                        Sadrzaj = blog.Sadrzaj,
                        DozvolaDodavanjaSlika = (korisnik == null ? false : (korisnik.KorisnikId == blog.KorisnikId) ? true : false),
                        DaLiMiSeSvidjaBlog = (korisnik == null ? false : korisnik.Blogs1.FirstOrDefault(b => b.BlogId == blog.BlogId) == null ? false : true)//(korisnik.Blogs1.FirstOrDefault(b => b.BlogId == blog.BlogId) != null?true:false)
                    };

                    ViewBag.Slike = blog.Slikas
                        .Select(s => new SlikaViewModel
                        {
                            Link = s.Link
                        }).ToList();

                    ViewBag.Komentari = blog.Komentars
                        .Select(k => new KomentarViewModel()
                        {
                            Komentar = k.Sadrzaj,
                            Autor = k.Korisnik.Ime + " " + k.Korisnik.Prezime,
                            DatumKreiranje = k.DatumKreiranja
                        }).ToList();

                    return View(blogViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Pocetna");
                }
            }
        }

        [Authorize(Roles = "Administrator, Korisnik")]
        public JsonResult Svidja(string blogID)
        {
            var blogId = Convert.ToInt32(blogID);
            using (var context = new BlogContext())
            {
                var data = false;
                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == User.Identity.Name);
                var blogGledani = context.Blogs.Find(blogId);

                var blog = korisnik.Blogs1.FirstOrDefault(b => b.BlogId == blogId);

                if (blog == null)//dodavanje svidja na blog
                {
                    korisnik.Blogs1.Add(blogGledani);
                    data = true;
                }
                else//brisanje iz baze svidja
                {
                    korisnik.Blogs1.Remove(blog);
                }

                context.SaveChanges();

                return Json(new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Administrator, Korisnik")]
        public JsonResult SlikaDodaj(string slikaLink, string blogID)
        {
            using (var context = new BlogContext())
            {
                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == User.Identity.Name);
                if (korisnik.Blogs.Any(b => b.BlogId == Convert.ToInt32(blogID)))
                {
                    Slika slika = new Slika() { BlogId = Convert.ToInt32(blogID), Link = slikaLink };
                    context.Slikas.Add(slika);
                    context.SaveChanges();
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Authorize(Roles = "Administrator, Korisnik")]
        public JsonResult PosaljiKomentar(string blogID, string komentar)
        {
            var blogId = Convert.ToInt32(blogID);
            using (var context = new BlogContext())
            {
                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == User.Identity.Name);

                Komentar komentarModel = new Komentar()
                {
                    BlogId = blogId,
                    DatumKreiranja = DateTime.Now,
                    KorisnikId = korisnik.KorisnikId,
                    Sadrzaj = komentar
                };

                context.Komentars.Add(komentarModel);
                context.SaveChanges();

                return Json(new { Success = true, Autor = korisnik.Ime + " " + korisnik.Prezime, Datum = komentarModel.DatumKreiranja.ToShortDateString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Administrator, Korisnik")]
        public ActionResult Omiljeni()
        {
            using (var context = new BlogContext())
            {
                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == User.Identity.Name);

                var omiljeni = korisnik.Blogs1.Where(b => b.Odobren == true)
                    .Select(b => new OdobreniBlogoviViewModel()
                    {
                        BlogId = b.BlogId,
                        DatumKreiranja = b.DatumKreiranja,
                        KorisnickoIme = b.Korisnik.KorisnickoIme,
                        Naslov = b.Naslov,
                        NaslovSlikaLink = b.NaslovnaSlikaLink
                    }).ToList();

                return View(omiljeni);//PartialView("_OdobreniBlogovi", omiljeni);
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult OdobravanjeBlogova()
        {
            using (var context = new BlogContext())
            {
                var blogovi = context.Blogs.Where(b => b.Odobren == false)
                    .Select(b => new OdobriBlogViewModel()
                    {
                        BlogId = b.BlogId,
                        Odobren = b.Odobren,
                        KorisnickoIme = b.Korisnik.KorisnickoIme,
                        NaslovnaSlikaLink = b.NaslovnaSlikaLink,
                        Naslov = b.Naslov
                    }).ToList();
                return View(blogovi);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult OdobravanjeBlogova(List<OdobriBlogViewModel> Model)
        {
            using(var context = new BlogContext())
            {
                
                foreach(var item in Model)
                {
                    context.Blogs.Find(item.BlogId).Odobren = item.Odobren;
                }

                context.SaveChanges();

                return RedirectToAction("Index", "Pocetna");
            }
        }
    }
}