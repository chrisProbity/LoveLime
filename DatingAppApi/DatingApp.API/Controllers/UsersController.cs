using AutoMapper;
using DatingApp.Core.DTOs;
using DatingApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserManager _manager;
        private readonly IMapper _mapper;

        public UsersController(IUserManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _manager.GetUsers();
            if(users != null)
            {
              var usersToReturn =  _mapper.Map<IEnumerable<UserListDTO>>(users);
                return Ok(usersToReturn);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(int id)
        {
            var user = await _manager.GetUser(id);
            if (user != null)
            {
                var userToReturn =  _mapper.Map<UserDetailDTO>(user);
                return Ok(userToReturn);
            }
            return NotFound();
        }
        [HttpDelete("{userId}/delete")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
             _manager.DeleteUser(userId);
            
            return NoContent();
        }
        [HttpPost()]
        public async Task<IActionResult> AddUser<T>(T entity) where T: class
        {
               _manager.Add( entity);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, ProfileUpdateDTO updateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userForUpdate = await _manager.GetUser(id);
            if(userForUpdate != null)
            {
                _mapper.Map(updateDTO, userForUpdate);
            }
           if( await _manager.SaveChanges())
           {
                return NoContent();
           }
            return BadRequest($"Updating user {id} failed on save");

            
        }


    }
}
