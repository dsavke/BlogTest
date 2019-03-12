using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class OdobriBlogViewModel
    {
        public int BlogId { get; set; }
        public string Naslov { get; set; }
        public string NaslovnaSlikaLink { get; set; }
        public bool Odobren { get; set; }
        public string KorisnickoIme { get; set; }
    }
}