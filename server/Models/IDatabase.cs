using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace server.Models
{
    public interface IDatabase
    {
        /// <summary>
        /// Get all issues from database
        /// </summary>
        /// <returns>List of Issue objects from database</returns>
        public List<Issue> GetIssues();

        /// <summary>
        /// Get one issue from database by id
        /// </summary>
        /// <param name="id">Id of the issue to be retreived</param>
        /// <returns>Issue from database by id</returns>
        public Issue GetIssue(uint id);

        /// <summary>
        /// Create a new issue
        /// </summary>
        /// <param name="issue">Issue to be created</param>
        public void CreateIssue(Issue issue);

        /// <summary>
        /// Update an existing issue
        /// </summary>
        /// <param name="issue">Issue to update. Database id will be pulled from this object.</param>
        public void UpdateIssue(Issue issue);

        /// <summary>
        /// Deletes an existing issue by id
        /// </summary>
        /// <param name="id">Id of issue to delete</param>
        public void DeleteIssue(uint id);

        /// <summary>
        /// Gets all categories from database
        /// </summary>
        /// <returns>List of category objects</returns>
        public List<Category> GetCategories();

        // CATEGORYCONTROLLER

        /// <summary>
        /// Gets a specific category from database by id
        /// </summary>
        /// <param name="id">Id of category to retreive</param>
        /// <returns>Category</returns>
        public Category GetCategory(uint id);

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="category">Category to be created</param>
        public void CreateCategory(Category category);

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="category">Category to update. Database id will be pulled from this object.</param>
        public void UpdateCategory(Category category);

        /// <summary>
        /// Deletes an existing category by id
        /// </summary>
        /// <param name="id">Id of category to delete</param>
        public void DeleteCategory(uint id);

        // USERCONTROLLER

        /// <summary>
        /// Returns a user from database by DB id
        /// </summary>
        /// <param name="id">Id to retreive</param>
        /// <returns></returns>
        public string GetUser(uint id);

        /// <summary>
        /// Returns a user from database by auth id
        /// </summary>
        /// <param name="authId">Id to retreive by</param>
        /// <returns></returns>
        public uint GetUser(string authId);
    }
}

