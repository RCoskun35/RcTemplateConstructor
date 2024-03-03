using Application.StaticServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Identity
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = LanguageService.lang["birHataOlustuLutfenDahaSonraTekrarDeneyiniz"]
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = LanguageService.lang["islemAyniAndaBaskaBirKullaniciTarafindanYapildiLutfenDahaSonraTekrarDeneyiniz"]
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = LanguageService.lang["girilenSifreHatali"]
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = LanguageService.lang["gecersizDogrulamaKodu"]
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = LanguageService.lang["buHesapBaskaBirKullaniciyaAittir"]
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"{LanguageService.lang["kullaniciAdiGecersiz"]} : '{userName}' . {LanguageService.lang["kullaniciAdiEnAz3KarakterUzunlugundaOlmalidir"]}"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = $"{LanguageService.lang["epostaAdresiGecersiz"]} : '{email}'"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"{LanguageService.lang["kullaniciAdiZatenKullanilmaktadir"]} : '{userName}'"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"{LanguageService.lang["epostaAdresiZatenKullanilmaktadir"]} : '{email}'"
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = $"{LanguageService.lang["rolAdiGecersiz"]} : '{role}'. {LanguageService.lang["rolAdiEnAz3KarakterUzunlugundaOlmalidir"]}"
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = $"{LanguageService.lang["rolAdiZatenKullanilmaktadir"]} : '{role}'"
            };
        }

    }
}
