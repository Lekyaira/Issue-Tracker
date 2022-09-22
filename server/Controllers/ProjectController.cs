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
                // Log the exception
                System.Console.WriteLine($"GetProjectsByUser ERROR: {ex.Message}");
                // Just return an empty list of projects
                return new List<Project>();
            }
        }

        // Create a new project
        // POST /api/project
        [Authorize]
        [HttpPost]
        public void CreateProject(Project project)
        {
            Console.WriteLine($"Create! : {project.Name}");
            // Set the project's owner to the currently logged in user
            project.Owner = getLoggedInUser();
            _db.CreateProject(project);
        }

        // Update an existing project
        // PUT /api/project
        [Authorize]
        [HttpPut]
        public void UpdateProject(Project project)
        {
            if (project.Id.HasValue)
            {
                // Make sure the user is able to udpate this project
                uint user = getLoggedInUser();
                Project oldProject = _db.GetProject(project.Id.Value);
                if (oldProject.Owner == user)
                {
                    // Make sure no funny business is going on with the owner
                    project.Owner = user;
                    // We're good to update the project
                    _db.UpdateProject(project);
                }
                // If not, just ignore the request
            }
        }

        // Delete an existing project
        // DELETE api/project/:id
        [Authorize]
        [HttpDelete("{id}")]
        public void DeleteProject(uint id)
        {
            // Make sure the user is able to delete this project
            Project project = _db.GetProject(id);
            if(project.Owner == getLoggedInUser())
            {
                _db.DeleteProject(id);
            }
            // If not, just ignore the request
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