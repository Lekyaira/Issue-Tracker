using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        // Create a database connection
        private readonly Database db = new Database("localhost", "ryan", "Taradhve", "IssueTracker");

        // Returns all issues
        // GET /api/issue
        [EnableCors("Development")]
        [HttpGet]
        public List<Issue> getIssues()
        {
            return db.GetIssues();
        }

        // Returns a specified issue
        // GET /api/issue/:id
        [EnableCors("Development")]
        [HttpGet("{id}")]
        public Issue getIssue(uint id)
        {
            return db.GetIssue(id);
        }

        // Create a new issue
        // POST /api/issue
        [EnableCors("Development")]
        [HttpPost]
        public void createIssue(Issue issue)
        {
            db.CreateIssue(issue);
        }

        // Update an existing issue by id
        // PUT /api/issue
        [EnableCors("Development")]
        [HttpPut]
        public void updateIssue(Issue issue)
        {
            db.UpdateIssue(issue);
        }

        // Delete an existing issue by id
        // delete /api/issue/:id
        [EnableCors("Development")]
        [HttpDelete("{id}")]
        public void deleteIssue(uint id)
        {
            db.DeleteIssue(id);
        }
    }
}