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
        public User GetCurrentUser()
        {
            var user = _auth0User.GetUserAsync(User.Identity.Name).Result;
            uint id = _database.GetUser(User.Identity.Name);

            return new User { Id = id, Name = user.name, Email = user.email };
        }

        [Authorize]
        [HttpGet("{id}")]
        public User GetUser(uint id)
        {
            // Get user info from database
            string authUser = _database.GetUser(id);
            // Get user info from Auth0
            var user = _auth0User.GetUserAsync(authUser).Result;

            // Build and return a user
            return new User { Id = id, Name = user.name, Email = user.email };
        }

        [Authorize]
        [HttpGet("project/{id}")]
        public List<User> GetUsersByProject(uint id)
        {
            // Get the user info from the database
            List<(uint id, string authId)> dbUsers = _database.UserGetUsersInProject(id);
            // Empty list to store our output
            List<User> users = new();
            foreach(var user in dbUsers)
            {
                var info = _auth0User.GetUserAsync(user.authId).Result;
                users.Add(new User { Id = user.id, Name = info.name, Email = info.email });
            }

            // Return our completed list
            return users;
        }
    }
}

