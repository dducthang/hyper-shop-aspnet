using HyperShop.DataAccess;
using HyperShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace HyperShop.Web.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class ResponseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResponseController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region API CALLS
        [Authorize]
        [HttpPost]
        public IActionResult Post(int commentId, string body)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new Response()
            {
                CommentId = commentId,
                Body = body,
                ApplicationUserId = userId
            };
            _context.Responses.Add(response);
            _context.SaveChanges();
            response.ApplicationUser = _context.ApplicationUsers.First(u=>u.Id==userId);
            return Json(response);
        }
        #endregion

    }
}
