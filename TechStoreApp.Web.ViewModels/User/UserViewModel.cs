using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string PictureUrl { get; set; }
        public List<string> Roles { get; set; }
    }
}
