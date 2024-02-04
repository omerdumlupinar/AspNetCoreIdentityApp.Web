using AspNetCoreIdentityApp.Web.OptionsModels;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings _emailSettings;

        public EmailServices(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordMailLink, string toEmail)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Host = _emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email);

            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "link";
            mailMessage.Body = @$"

<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Şifre Sıfırlama</title>
  <style>
    body {{
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 0;
    }}
    .container {{
      max-width: 600px;
      margin: 20px auto;
      background-color: #fff;
      border-radius: 5px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
      overflow: hidden;
    }}
    .header {{
      background-color: #4CAF50;
      color: #fff;
      text-align: center;
      padding: 10px;
    }}
    .content {{
      padding: 20px;
    }}
    .button {{
      display: inline-block;
      font-size: 16px;
      font-weight: bold;
      padding: 10px 20px;
      text-align: center;
      text-decoration: none;
      background-color: #4CAF50;
      border-radius: 5px;
      color:white;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <div class=""header"">
      <h2>Şifre Sıfırlama</h2>
    </div>
    <div class=""content"">
      <p>Merhaba,</p>
      <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıyı tıklayın:</p>
      <p><a class=""button"" href='{resetPasswordMailLink}'>Şifre Sıfırlama</a></p>
      <p>Bu bağlantı 24 saat boyunca geçerlidir. Eğer şifrenizi sıfırlamak istemiyorsanız, bu e-postayı dikkate almayabilirsiniz.</p>
      <p>İyi günler dileriz.</p>
    </div>
  </div>
</body>
</html>





";

            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
