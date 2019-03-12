using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class BlogDetailsViewModel
    {
        public int BlogId { get; set; }
        public string Autor { get; set; }
        public string Drzava { get; set; }
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public Nullable<System.DateTime> DatumPutovanja { get; set; }
        public string NaslovnaSlikaLink { get; set; }
        public System.DateTime DatumKreiranja { get; set; }
        public bool DozvolaDodavanjaSlika { get; set; }
        public bool DaLiMiSeSvidjaBlog { get; set; }
    }
}