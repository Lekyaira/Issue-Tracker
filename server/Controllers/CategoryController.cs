using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // Create a database connection
        //private readonly Database db = new Database("localhost", "ryan", "Taradhve", "IssueTracker");

        private readonly IDatabase _db;

        public CategoryController(IDatabase database)
        {
            _db = database;
        }

        // Returns all categories
        // GET /api/category
        [HttpGet]
        [Authorize]
        public List<Category> GetCategories()
        {
            return _db.GetCategories();
        }

        // Returns a specified category
        // GET /api/category/:id
        [Authorize]
        [HttpGet("{id}")]
        public Category GetCategory(uint id)
        {
            return _db.GetCategory(id);
        }

        // Create a new category
        // POST /api/category
        [Authorize]
        [HttpPost]
        public void CreateCategory(Category category)
        {
            _db.CreateCategory(category);
        }

        // Update an existing category by id
        // PUT /api/category
        [Authorize]
        [HttpPut]
        public void UpdateCategory(Category category)
        {
            _db.UpdateCategory(category);
        }

        // Delete an existing category by id
        // delete /api/category/:id
        [Authorize]
        [HttpDelete("{id}")]
        public void DeleteCategory(uint id)
        {
            _db.DeleteCategory(id);
        }
    }
}