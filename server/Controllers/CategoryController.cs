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
    public class CategoryController : ControllerBase
    {
        // Create a database connection
        private Database db = new Database("localhost", "ryan", "Taradhve", "IssueTracker");

        [EnableCors("Development")]
        [HttpGet]
        public List<Category> GetCategories()
        {
            return db.GetCategories();
        }
    }
}