using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.User
{
    public class ManageUsersViewModel
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public string? UserNameQuery { get; set; }
        public string? EmailQuery { get; set; }
        public List<UsersDetailsViewModel> Users { get; set; }

    }
}
