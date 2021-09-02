using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RESTFull_API_NetCore_DatingApp.Entities;
using RESTFull_API_NetCore_DatingApp.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RESTFull_API_NetCore_DatingApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser usr)
        {
            //adding claimns
            var claims = new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.NameId, usr.UserName)
           };

            //adding some creds
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //add dewscriptions how the token would look like
            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //creating a tolen handler to create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesciptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
