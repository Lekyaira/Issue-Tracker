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
        private readonly Database db = new Database("localhost", "ryan", "Taradhve", "IssueTracker");

        // Returns all categories
        // GET /api/category
        [EnableCors("Development")]
        [HttpGet]
        public List<Category> GetCategories()
        {
            return db.GetCategories();
        }

        // Returns a specified category
        // GET /api/category/:id
        [EnableCors("Development")]
        [HttpGet("{id}")]
        public Category GetCategory(uint id)
        {
            return db.GetCategory(id);
        }

        // Create a new category
        // POST /api/category
        [EnableCors("Development")]
        [HttpPost]
        public void CreateCategory(Category category)
        {
            db.CreateCategory(category);
        }

        // Update an existing category by id
        // PUT /api/category
        [EnableCors("Development")]
        [HttpPut]
        public void UpdateCategory(Category category)
        {
            db.UpdateCategory(category);
        }

        // Delete an existing category by id
        // delete /api/category/:id
        [EnableCors("Development")]
        [HttpDelete("{id}")]
        public void DeleteCategory(uint id)
        {
            db.DeleteCategory(id);
        }
    }
}