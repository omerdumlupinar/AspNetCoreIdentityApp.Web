﻿using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var userDigit = int.TryParse(user.UserName[0].ToString(), out _);

            if (userDigit)
            {
                errors.Add(new()
                {
                    Code = "UserNameContainFirstLaterDigit",
                    Description = "Kullanıcı adının ilk karakteri sayısal bir karakter içeremez."
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
