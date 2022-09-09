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

        public IssueController(IAuth0User auth0User, IDatabase database)
        {
            _auth0User = auth0User;
            _db = database;
        }

        // Returns all issues
        // GET /api/issue
        //[EnableCors("Development")]
        [HttpGet]
        [Authorize]
        public List<Issue> getIssues()
        {
            //var userName = User.Identity.Name;

            //// Get user info from the Auth0 server
            //User user = _auth0User.GetUserAsync(userName).Result;
            //System.Console.WriteLine($"UserId: {userName}, User Name: {user.Name}, User Email: {user.Email}");


            return _db.GetIssues();
        }

        // Returns a specified issue
        // GET /api/issue/:id
        [Authorize]
        [HttpGet("{id}")]
        public IssueUser getIssue(uint id)
        {
            // Get the issue from the database
            Issue issue = _db.GetIssue(id);

            // Get the user authId from database
            string authId = _db.GetUser(issue.CreatorId);

            // Get the user data from Auth0 using the issue's creatorId
            var userInfo = _auth0User.GetUserAsync(authId).Result;

            // Create the user object
            User user = new User { Id = issue.CreatorId, Name = userInfo.name, Email = userInfo.email };

            // Return the values
            return new IssueUser { issue = issue, user = user };
        }

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
    }
}