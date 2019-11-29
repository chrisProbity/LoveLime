using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Core.DTOs;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserManager _userManger;
        private readonly IConfiguration _config;

        public AccountsController( IUserManager userManger, IConfiguration config)
        {
            _userManger = userManger;
          _config = config;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
       
            return "value";
        }

        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
               if (ModelState.IsValid)
               {
                //throw new Exception("computer says no");
                    registerDTO.Username = registerDTO.Username.ToLower();
                    var userExist = await _userManger.UserExists(registerDTO.Username);
                    if (userExist)
                    {
                        return BadRequest("Username and/or Password already exists");
                    }
                    var userToCreate = new User
                    {
                        Username = registerDTO.Username,
                    };
                    var user = await _userManger.Register(userToCreate, registerDTO.Password);
                    return StatusCode(201);
               }
                return BadRequest(ModelState);
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
                if (ModelState.IsValid)
                {
                //throw new Exception("computer says no");
                    var user = await _userManger.Login(loginDTO.username.ToLower(), loginDTO.Password);
                    if (user == null)
                    {
                        return Unauthorized();
                    }
                    var claims = new Claim[]
                    {
                      new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                      new Claim(ClaimTypes.Name, user.Username.ToString())
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = creds,
                        Issuer = "http://localhost:44387",
                        Audience = "http://localhost:4200"
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Ok(new
                    {
                        token = tokenHandler.WriteToken(token)
                    });
                }
                return BadRequest(ModelState);
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
