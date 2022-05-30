using HyperShop.DataAccess;
using HyperShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region API CALLS

        [HttpPost]
        public IActionResult Post(int productId, string body)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = new Comment()
            {
                ProductId = productId,
                Body = body,
                ApplicationUserId = userId
            };
            _context.Comments.Add(comment);
            comment.ApplicationUser = _context.ApplicationUsers.First(u=>u.Id==userId);
            return Json(comment);
        }
        #endregion

    }
}
