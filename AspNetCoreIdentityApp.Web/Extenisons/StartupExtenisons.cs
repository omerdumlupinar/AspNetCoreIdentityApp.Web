using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localizations;
using AspNetCoreIdentityApp.Web.Models;

namespace AspNetCoreIdentityApp.Web.Extenisons
{
    public static class StartupExtenisons
    {
        public static void AddIdentityWhitExtenisons(this IServiceCollection Services)
        {
            Services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                //username için geçerli olan karakterler
                //opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvyxwqx1234567890_";
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.MaxFailedAccessAttempts = 5;

            })
                .AddPasswordValidator<Passwordvalidator>()
                .AddErrorDescriber<LocalizationsIdentityErrorDescriber>()
                .AddUserValidator<UserValidator>()
                .AddEntityFrameworkStores<AppDbContext>();

        }
    }
}
