using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTFull_API_NetCore_DatingApp.Data;
using RESTFull_API_NetCore_DatingApp.DTOs;
using RESTFull_API_NetCore_DatingApp.Entities;
using RESTFull_API_NetCore_DatingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RESTFull_API_NetCore_DatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        //injecting context class and TokenService
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserRegistrationDTO usrDto)
        {
            if (await IsUsernameExists(usrDto.Username)) return BadRequest("Username already exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = usrDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usrDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            { 
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync( usr => usr.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i=0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }


        //checking for Unique username
        private async Task<bool> IsUsernameExists(string username)
        {
            return await _context.Users.AnyAsync(usr => usr.UserName == username.ToLower());
        }
    }
}
