using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IReviewService
    {
        IEnumerable<ReviewViewModel> GetProductReviews(int productId);

    }
}
