namespace AspNetCoreIdentityApp.Web.Services
{
    public interface IEmailServices
    {
        Task SendResetPasswordEmail(string resetPasswordMailLink,string toEmail);
    }
}
