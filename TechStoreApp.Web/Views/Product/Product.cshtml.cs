//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//

//namespace TechStoreApp.Web.Views.Product
//{
//    public class ProductModel : PageModel
//    {
//        private readonly TechStoreDbContext _context;

//        public ProductModel(TechStoreDbContext context)
//        {
//            _context = context;
//        }

//        public List<Review> Reviews { get; set; }

//        public async Task<IActionResult> OnGetAsync(int productId)
//        {
//            Reviews = await _context.Reviews
//                .Include(r => r.User) 
//                .Where(r => r.ProductId == productId) 
//                .ToListAsync();

//            if (Reviews == null || !Reviews.Any())
//            {
//                return NotFound();
//            }

//            return Page();
//        }

//    }
//}
