using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Service to get Auth0 user data
        private readonly IAuth0User _auth0User;
        // Service to access the database
        private readonly IDatabase _database;

        public UserController(IAuth0User auth0User, IDatabase database)
        {
            _auth0User = auth0User;
            _database = database;
        }

        [HttpGet]
        [Authorize]
        public User GetUser()
        {
            var user = _auth0User.GetUserAsync(User.Identity.Name).Result;
            uint id = _database.GetUser(User.Identity.Name);

            return new User { Id = id, Name = user.name, Email = user.email };
        }

    }
}

