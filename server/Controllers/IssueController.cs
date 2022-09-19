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
    public class IssueController : ControllerBase
    {
        // Create a database connection
        //private readonly Database db = new Database("localhost", "ryan", "Taradhve", "IssueTracker");

        // Service to get Auth0 user data
        private readonly IAuth0User _auth0User;

        // Database service
        private readonly IDatabase _db;

        // Access service
        private readonly IAccess _access;

        public IssueController(IAuth0User auth0User, IDatabase database, IAccess access)
        {
            _auth0User = auth0User;
            _db = database;
            _access = access;
        }

        // Users shouldn't be able to retreive all issues normally. Save for debug purposes.

        // Returns all issues
        // GET /api/issue
        //[EnableCors("Development")]
        //[HttpGet]
        //[Authorize]
        //public List<Issue> getIssues()
        //{
        //    //var userName = User.Identity.Name;

        //    //// Get user info from the Auth0 server
        //    //User user = _auth0User.GetUserAsync(userName).Result;
        //    //System.Console.WriteLine($"UserId: {userName}, User Name: {user.Name}, User Email: {user.Email}");


        //    return _db.GetIssues();
        //}

        // Returns all issues by given project id
        // GET /api/issue/project/:id
        [Authorize]
        [HttpGet("project/{id}")]
        public List<Issue> getIssues(uint id)
        {
            // Get currently logged in user
            uint currentUser = getLoggedInUser();

            // Check if user is allowed in this project
            // Get all users associated with the project from database
            List<uint> users = _db.GetUsersInProject(id);
            foreach (uint user in users)
            {
                // Return list of all issues in the project if allowed, else return an empty list
                if (user == currentUser)
                {
                    return _db.GetIssuesByProject(id);
                }
            }

            try // If we hit a problem, just return an empty list
            {
                // Check if currently logged in user is allowed in this project.
                // If so, return the project. Else return an empty list.
                if (_access.isUserAllowedInProject(getLoggedInUser(), id))
                {
                    return _db.GetIssuesByProject(id);
                }
                else
                {
                    return new List<Issue>();
                }
            }
            catch (Exception ex)
            {
                return new List<Issue>();
            }
        }

        // Returns a specified issue
        // GET /api/issue/:id
        [Authorize]
        [HttpGet("{id}")]
        public IssueUser getIssue(uint id)
        {
            // Get the issue from the database
            Issue issue = _db.GetIssue(id);

            // If the user is allowed to access this project, return the requested issue
            if (_access.isUserAllowedInProject(getLoggedInUser(), issue.ProjectId))
            {

                // Get the user authId from database
                string authId = _db.GetUser(issue.CreatorId);

                // Get the user data from Auth0 using the issue's creatorId
                var userInfo = _auth0User.GetUserAsync(authId).Result;

                // Create the user object
                User user = new User { Id = issue.CreatorId, Name = userInfo.name, Email = userInfo.email };

                // Return the values
                return new IssueUser { issue = issue, user = user };
            }
            else // The user is not allowed, return an empty IssueUser
            {
                return new IssueUser();
            }
        }

        //TODO: We're kind of allowing users to inject data into any project/category in the system here. Should probably add some kind of check for this.

        // Create a new issue
        // POST /api/issue
        [Authorize]
        [HttpPost]
        public void createIssue(Issue issue)
        {
            _db.CreateIssue(issue);
        }

        // Update an existing issue by id
        // PUT /api/issue
        [Authorize]
        [HttpPut]
        public void updateIssue(Issue issue)
        {
            _db.UpdateIssue(issue);
        }

        // Delete an existing issue by id
        // delete /api/issue/:id
        [Authorize]
        [HttpDelete("{id}")]
        public void deleteIssue(uint id)
        {
            _db.DeleteIssue(id);
        }

        // Returns the currently logged in user by database id
        private uint getLoggedInUser()
        {
            // Get user AuthID user name
            var userName = User.Identity.Name;

            // Return user database id
            return _db.GetUser(userName);
        }

        //// Checks it a user is associated with the given project
        //private bool isUserAllowedInProject(uint id, uint projectId)
        //{
        //    // Check if user is allowed in this project
        //    // If the current user is the project's owner, it's allowed
        //    if (_db.GetProject(projectId).owner == id) return true;

        //    // Get all users associated with the project from database
        //    List<uint> users = _db.GetUsersInProject(projectId);
        //    foreach (uint user in users)
        //    {
        //        // If the user is in the list, it's allowed
        //        if (user == id)
        //        {
        //            return true;
        //        }
        //    }

        //    // No allowed users found
        //    return false;
        //}
    }
}