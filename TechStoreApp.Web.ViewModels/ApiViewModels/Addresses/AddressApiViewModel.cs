using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.ApiViewModels.Addresses
{
    public class AddressApiViewModel
    {
        public int AddressId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string Details { get; set; }
    }
}
