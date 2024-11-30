using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.ApiViewModels.Users
{
    public class UserDetailsApiViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<string> Roles { get; set; }
    }
}
