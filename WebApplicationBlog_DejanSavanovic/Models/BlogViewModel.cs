using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class BlogViewModel
    {
        public int BlogId { get; set; }
        public string Naslov { get; set; }
        public int DrzavaId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DatumPutovanja { get; set; }
        public string NaslovnaSlikaLink { get; set; }
        [AllowHtml]
        public string Sadrzaj { get; set; }
    }
}