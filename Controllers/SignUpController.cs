using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IPayYouPrint.Data;
using IPayYouPrint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPayYouPrint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IPayYouPrintContext _context;

        public SignUpController(IPayYouPrintContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if(user.email != null && user.password != null && user.name != null && user.surname != null && user.address != null)
            {
                var _user = await _context.User.Where(x => x.email == user.email).FirstOrDefaultAsync();
                if (_user == null)
                {
                    Byte[] inputBytes = Encoding.UTF8.GetBytes(user.password);
                    Byte[] hashedBytes = new SHA256CryptoServiceProvider().ComputeHash(inputBytes);

                    user.password = BitConverter.ToString(hashedBytes);
                   
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();

                    return user;
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return BadRequest();
            }
            
        }

        private string ComputeHash(object password, SHA256CryptoServiceProvider sHA256CryptoServiceProvider)
        {
            throw new NotImplementedException();
        }
    }
}