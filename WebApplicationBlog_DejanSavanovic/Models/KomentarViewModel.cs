using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class KomentarViewModel
    {
        public string Autor { get; set; }
        public DateTime DatumKreiranje { get; set; }
        public string Komentar { get; set; }
    }
}