using System.Security.Policy;

namespace TechStoreApp.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public string Error { get; set; }
        public List<string> Messages { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
