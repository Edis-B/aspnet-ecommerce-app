using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.User
{
    public class UsersDetailsViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<string> UserRoles { get; set; }
        public List<string> MissingRoles { get; set; }
    }
}
