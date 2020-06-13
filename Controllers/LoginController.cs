using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using IPayYouPrint.Models;
using System.Text;
using IPayYouPrint.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace IPayYouPrint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private readonly IPayYouPrintContext _context;

        public LoginController(IPayYouPrintContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]User login)
        {
            IActionResult response = Unauthorized();

            Byte[] inputBytes = Encoding.UTF8.GetBytes(login.password);
            Byte[] hashedBytes = new SHA256CryptoServiceProvider().ComputeHash(inputBytes);

            login.password = BitConverter.ToString(hashedBytes);

            var user = await _context.User.Where(x => x.email == login.email).Where(x => x.password == login.password).FirstOrDefaultAsync();

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userInfo.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}