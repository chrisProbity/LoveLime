using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using DatingApp.Core.DTOs;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{   [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IUserManager _manager;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySetings> _cloudinariConfig;
        private readonly Cloudinary _cloudinary;

        public PhotosController(IUserManager manager, IMapper mapper, IOptions<CloudinarySetings> cloudinariConfig)
        {
            _manager = manager;
            _mapper = mapper;
            _cloudinariConfig = cloudinariConfig;

            Account acc = new Account
            {
                Cloud = _cloudinariConfig.Value.CloudName,
                ApiKey = _cloudinariConfig.Value.ApiKey,
                ApiSecret = _cloudinariConfig.Value.ApiSecret
            };
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("id", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var photo = await _manager.GetPhoto(id);
            var result = _mapper.Map<PhotoForReturnDTO>(photo);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMainPhoto(int userId)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var mainPhoto = _manager.GetMainPhoto(userId);
            return Ok(mainPhoto);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto (int userId, [FromForm]PhotoUploadDTO uploadDTO)
        {
            try
            {
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                {
                    return Unauthorized();
                }
                var userInfo = await _manager.GetUser(userId);

                var file = uploadDTO.File;
                var uploadResult = new ImageUploadResult();

                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        uploadResult = _cloudinary.Upload(uploadParams);
                    }
                }
                uploadDTO.Url = uploadResult.Uri.ToString();
                uploadDTO.PublicId = uploadResult.PublicId;

                var photo = _mapper.Map<Photo>(uploadDTO);
                if (!userInfo.Photos.Any(x => x.IsMain))
                {
                    photo.IsMain = true;
                }
                userInfo.Photos.Add(photo);
                if (await _manager.SaveChanges())
                {
                    var photoToReturn = _mapper.Map<PhotoForReturnDTO>(photo);
                    return CreatedAtRoute("GetPhoto", new { id = photo.ID }, photoToReturn);
                }
                return BadRequest("Adding photo failed on save");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException.ToString());
            }
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int id, int userId)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _manager.GetUser(userId);
            if(!user.Photos.Any(p => p.UserId == userId))
            {
                return Unauthorized();
            }
            var photo = await _manager.GetPhoto(id);
            if (photo.IsMain)
            {
                return BadRequest("This is already the main photo");
            }
            var currentMainPhoto = await _manager.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;

            photo.IsMain = true;
            if (await _manager.SaveChanges())
                return NoContent();
            return BadRequest("An error occured while processing your request");

        }
    }
}