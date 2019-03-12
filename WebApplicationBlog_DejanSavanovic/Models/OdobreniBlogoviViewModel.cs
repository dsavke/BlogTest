using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class OdobreniBlogoviViewModel
    {
        public int BlogId { get; set; }
        public string NaslovSlikaLink { get; set; }
        public string Naslov { get; set; }
        public string KorisnickoIme { get; set; }
        public DateTime DatumKreiranja { get; set; }
    }
}