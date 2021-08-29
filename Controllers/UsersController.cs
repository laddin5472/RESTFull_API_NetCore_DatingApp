using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTFull_API_NetCore_DatingApp.Entities;
using RESTFull_API_NetCore_DatingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTFull_API_NetCore_DatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        //Use the controller class to get data from database and 
        //we are going to achieve that by using Dependency Injection
        //So, we are injecting the DataContext class which is derieved from
        //DbContext class of EF in order to connect to DB and fetch data
        public UsersController(DataContext context)
        {
            _context = context;
        }

        //Following actions are performed synchronously
        //Let's do the same assynchronously
        /*
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
        */

        //By doing the DB call async we are telling the thread that handles the request
        //to defer it to a Task by launching another thread so the current thread is
        //available for subsequent task without waiting for the task to complete
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
    }
}
