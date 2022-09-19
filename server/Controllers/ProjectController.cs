using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Auth0.ManagementApi;
using Auth0.Core;
using RestSharp;

using server.Models;
using System.Threading;
using Ubiety.Dns.Core;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        // Service to get Auth0 user data
        private readonly IAuth0User _auth0User;

        // Database service
        private readonly IDatabase _db;

        // Access service
        private readonly IAccess _access;

        public ProjectController(IAuth0User auth0User, IDatabase database, IAccess access)
        {
            _auth0User = auth0User;
            _db = database;
            _access = access;
        }

        // Returns the specified project information
        // GET /api/project/:id
        [Authorize]
        [HttpGet("{id}")]
        public Project GetProject(uint id)
        {
            try
            {
                // Check if the user is allowed access to this project
                if (_access.isUserAllowedInProject(getLoggedInUser(), id))
                {
                    // Get the project data
                    return _db.GetProject(id);
                }
                else // Return an empty project
                {
                    return new Project();
                }
            }
            catch (Exception ex) // Just return an empty project
            {
                return new Project();
            }
        }

        // Returns all projects current user has access to
        // GET /api/project
        [Authorize]
        [HttpGet]
        public List<Project> GetProjectsByUser()
        {
            try
            {
                // Return all projects the given user has access to
                return _db.GetProjectsByUser(getLoggedInUser());
            }
            catch (Exception ex)
            {
                // Just return an empty list of projects
                return new List<Project>();
            }
        }

        // Returns the currently logged in user by database id
        private uint getLoggedInUser()
        {
            // Get user AuthID user name
            var userName = User.Identity.Name;

            // Return user database id
            return _db.GetUser(userName);
        }
    }
}