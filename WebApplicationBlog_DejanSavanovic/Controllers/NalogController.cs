using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplicationBlog_DejanSavanovic.DBModels;
using WebApplicationBlog_DejanSavanovic.Models;

namespace WebApplicationBlog_DejanSavanovic.Controllers
{
    public class NalogController : Controller
    {

        [AllowAnonymous]
        public ActionResult Prijavi()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Prijavi(NalogViewModel viewModel, string returnUrl)
        {
            using (var context = new BlogContext())
            {
                var korisnik = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == viewModel.KorisnickoIme && k.Lozinka == viewModel.Lozinka);

                if (korisnik != null)
                {
                    var authTicket = new FormsAuthenticationTicket(
                                                         1,
                                                         viewModel.KorisnickoIme,
                                                         DateTime.Now,
                                                         DateTime.Now.AddMinutes(30),
                                                         false,
                                                         korisnik.Uloga.Naziv
                                                         //"User",     ovaj ne moze jer ne mogu 2 user-a u 2 stringa
                                                         //string.Join(",", korisnik.KorisnikUlogas.Select(u => u.Uloga.Naziv).Distinct())  //user uloga

                                                         );
                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Pocetna");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Pogresno korisnicko ime ili lozinka");
                    return View();
                }
            }
        }

        [Authorize(Roles = "Administrator, Korisnik")]
        public ActionResult Odjavi()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Prijavi", "Nalog");
        }

    }
}