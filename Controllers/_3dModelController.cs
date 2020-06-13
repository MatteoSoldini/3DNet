using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPayYouPrint.Data;
using IPayYouPrint.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace IPayYouPrint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _3dModelController : ControllerBase
    {
        private readonly IPayYouPrintContext _context;

        public _3dModelController(IPayYouPrintContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: api/_3dModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<_3dModel>>> Get_3dModel()
        {
            string id = "";
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var model = _context._3dModel.AsQueryable();
            model = model.Where(x => x.owner_id == Convert.ToInt32(id)).AsQueryable();
            
            if(model == null)
            {
                return NotFound();
            }
            
            return await model.ToListAsync();
        }

        [Authorize]
        // GET: api/_3dModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<_3dModel>>> Get_3dModel(int _id)
        {
            string id = "";
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var model = _context._3dModel.AsQueryable();
            model = model.Where(x => x.owner_id == Convert.ToInt32(id)).Where(x => x.id == Convert.ToInt32(_id)).AsQueryable();

            if (model == null)
            {
                return Unauthorized();
            }

            return await model.ToListAsync();
        }

        [Authorize]
        // POST: api/_3dModel
        [HttpPost]
        public async Task<ActionResult<_3dModel>> Post_3dModel()
        {
            string id = "";
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var request = Request;
            var filePath = DateTime.Now.ToString("dd-MM-yyyy-hh_mm-ss") + request.Headers["filename"];
            using (var fs = new System.IO.FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Objects", filePath), System.IO.FileMode.Create))
            {
                request.Body.CopyTo(fs);
            }
            _3dModel temp = new _3dModel();
            temp.file_location = filePath;
            temp.owner_id = Convert.ToInt32(id);
            _context._3dModel.Add(temp);
            await _context.SaveChangesAsync();

            return temp;
        }

        [Authorize]
        // DELETE: api/_3dModel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<_3dModel>> Delete_3dModel(int id)
        {
            var _3dModel = await _context._3dModel.FindAsync(id);
            if (_3dModel == null)
            {
                return NotFound();
            }

            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Objects", _3dModel.file_location));

            _context._3dModel.Remove(_3dModel);
            await _context.SaveChangesAsync();

            return _3dModel;
        }

        private bool _3dModelExists(int id)
        {
            return _context._3dModel.Any(e => e.id == id);
        }
    }
}
