using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StaticServices
{
    public static class ErrorService
    {
        public static string GetErrors(ModelStateDictionary modelState)
        {
            try
            {
                if (modelState == null)
                    return LanguageService.lang["belirlenemeyenBirHataOlustu"] + " !";

                var errorList = "";
                foreach (var entry in modelState)
                {
                    if (string.IsNullOrEmpty(entry.Key))
                        return LanguageService.lang["belirlenemeyenBirHataOlustu"] + " !";

                    if (entry.Value.Errors.Any())
                    {
                        errorList = errorList + LanguageService.lang[entry.Key].ToUpper() + "-";
                        var errorMessages = entry.Value.Errors.Select(error => error.ErrorMessage == "The value '' is invalid." ? LanguageService.lang["alanBosBirakilamaz"] : error.ErrorMessage).ToList();

                        if (errorMessages.Count > 1)
                        {
                            errorList += "* " + errorMessages[1] + "-";
                        }
                        else
                            errorList += "* " + errorMessages[0] + "-";

                    }
                }

                return errorList;
            }
            catch (Exception ex)
            {
                return LanguageService.lang["belirlenemeyenBirHataOlustu"] + " !";
            }

        }

        public static string GetErrors(IdentityResult result)
        {

            try
            {
                if (result == null)
                    return LanguageService.lang["belirlenemeyenBirHataOlustu"] + " !";

                var errorList = "";
                foreach (var error in result.Errors)
                {
                    errorList = errorList + LanguageService.lang[error.Code].ToUpper() + "-";
                    errorList += "* " + error.Description + "-";
                }

                return errorList;
            }
            catch (Exception ex)
            {
                return LanguageService.lang["belirlenemeyenBirHataOlustu"] + " !";
            }
        }
    }
}
