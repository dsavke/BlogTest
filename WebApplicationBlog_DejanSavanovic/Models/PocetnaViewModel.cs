using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class PocetnaViewModel
    {
        public List<SelectListItem> Drzave { get; set; }
        public List<TopPetBlogovaViewModel> TopPetBlogova { get; set; }
        public List<TopPetAutoraViewModel> TopPetAutora { get; set; }
    }
}