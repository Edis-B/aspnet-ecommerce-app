using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.User
{
    public class ConfirmEmailViewModel
    {
        [TempData]
        public string StatusMessage { get; set; }
        public string Code { get; set; }
    }
}
