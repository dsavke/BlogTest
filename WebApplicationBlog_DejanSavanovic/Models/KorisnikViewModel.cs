using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebApplicationBlog_DejanSavanovic.DBModels;

namespace WebApplicationBlog_DejanSavanovic.Models
{
    public class KorisnikViewModel: IValidatableObject
    {
        public int KorisnikId { get; set; }
        public int UlogaId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")]
        public string Email { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public bool Aktivan { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var context = new BlogContext())
            {

                bool postojiUsernameUBazi = context.Korisniks.Any(k => k.KorisnickoIme == KorisnickoIme && k.KorisnikId != KorisnikId);

                if (postojiUsernameUBazi)
                {
                    yield return new ValidationResult("Greska", new[] { nameof(KorisnickoIme) });
                }

                var hasNumber = new Regex(@"[0-9]+");
                var hasLetter = new Regex(@"[a-zA-Z]+");
                var hasMinimum6Chars = new Regex(@".{6,}");

                var isValidated = hasNumber.IsMatch(Lozinka) && hasLetter.IsMatch(Lozinka) && hasMinimum6Chars.IsMatch(Lozinka);

                if (!isValidated)
                {
                    yield return new ValidationResult("Greska", new[] { nameof(Lozinka) });
                }

            }
        }

    }
}