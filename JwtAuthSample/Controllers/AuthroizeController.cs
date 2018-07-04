using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtAuthSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthSample.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthroizeController : ControllerBase
    {

        private readonly JwtSeetings _jwtSeetings;

        public AuthroizeController(IOptions<JwtSeetings> jwtSeetingsOptions)
        {
            _jwtSeetings = jwtSeetingsOptions.Value;
        }


        [HttpPost]
        public ActionResult Post([FromBody]LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (loginViewModel.Name == "jack" && loginViewModel.Password == "rose")
            {

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,"jack"),
                    new Claim(ClaimTypes.Role,"admin")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSeetings.Issuer,
                    _jwtSeetings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    creds
                    );
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest();
        }
    }
}