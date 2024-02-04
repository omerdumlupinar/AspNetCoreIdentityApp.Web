using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {

        public ResetPasswordViewModel()
        {
                    
        }
        public ResetPasswordViewModel(string email)
        {
            Email = email;
        }

        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış.")]
        [Display(Name = "Email :")]
        public string Email { get; set; }
    }
}
