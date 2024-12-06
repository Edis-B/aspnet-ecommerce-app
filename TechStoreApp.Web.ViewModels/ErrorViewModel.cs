using System.Security.Policy;

namespace TechStoreApp.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public int StatusCode { get; set; } = 0;
        public List<string> Messages { get; set; } = new List<string>();
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
