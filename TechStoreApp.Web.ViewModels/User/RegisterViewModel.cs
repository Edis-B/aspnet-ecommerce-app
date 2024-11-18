using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TechStoreApp.Web.ViewModels.User
{
    public class RegisterViewModel
    {
        [Display(Name = "ProfilePicture")]
        public string? ProfilePictureUrl { get; set; }
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessage = "The Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The UserName is required.")]
        [PersonalData]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}

