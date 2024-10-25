using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechStoreApp.Web.ViewModels.User
{
    public class LoginViewModel
    {
        [PersonalData]
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [PersonalData]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
