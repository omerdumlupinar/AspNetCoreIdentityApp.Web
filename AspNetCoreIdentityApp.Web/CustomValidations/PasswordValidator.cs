using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class Passwordvalidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new()
                {
                    Code = "PassvordContainUserName",
                    Description = "Şifre alanı kullanıcı adı içeremez."
                });
            }

            if (password.ToLower().StartsWith("1234"))
            {
                errors.Add(new()
                {
                    Code = "PassvordContain1234",
                    Description = "Şifre alanı ardışık sayı içeremez."
                });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
