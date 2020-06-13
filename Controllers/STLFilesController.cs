using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IPayYouPrint.Data;
using IPayYouPrint.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPayYouPrint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class STLFilesController : ControllerBase
    {
        private readonly IPayYouPrintContext _context;
        public STLFilesController(IPayYouPrintContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult BannerImage(int id)
        {
            string user_id = "";

            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                user_id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var model = _context._3dModel.AsQueryable();
            model = model.Where(x => x.id == id).Where(x => x.owner_id == Convert.ToInt32(user_id)).AsQueryable();

            if (model.Count() == 0)
            {
                return NotFound();
            }

            var file = Path.Combine(Directory.GetCurrentDirectory(), "Objects", model.FirstOrDefault().file_location);

            return PhysicalFile(file, "application/octet-stream");
        }
    }
}