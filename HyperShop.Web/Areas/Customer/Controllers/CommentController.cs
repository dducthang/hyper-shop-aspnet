using HyperShop.DataAccess;
using HyperShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyperShop.Web.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region API CALLS
        [Authorize]
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
            _context.SaveChanges();
            comment.ApplicationUser = _context.ApplicationUsers.First(u=>u.Id==userId);
            return Json(comment);
        }

        public IActionResult Get(int page, int taking, int productId)
        {
            var comments = _context.Comments.Where(c => c.ProductId == productId)
                .OrderByDescending(c=>c.PostedDate)
                .Skip(taking*(page-1)).Take(taking)
                .Include(c=>c.ApplicationUser)
                .Include(c=>c.Responses)
                .ToList();

            comments.Reverse();

            
            return Json(new
            {
                comments = comments,
                commentCount = _context.Comments.Count(c => c.ProductId == productId),
            });

        }
        #endregion

    }
}
