using RESTFull_API_NetCore_DatingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTFull_API_NetCore_DatingApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser usr);
    }
}
