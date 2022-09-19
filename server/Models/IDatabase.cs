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
        /// Get all issues under specified project id.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns>List of issues</returns>
        public List<Issue> GetIssuesByProject(uint id);

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

        // CATEGORYCONTROLLER

        /// <summary>
        /// Gets all categories from database
        /// </summary>
        /// <returns>List of category objects</returns>
        public List<Category> GetCategories();

        /// <summary>
        /// Gets all categories by project id
        /// </summary>
        /// <param name="id">Id of project to retreive</param>
        /// <returns></returns>
        public List<Category> GetCategoriesByProject(uint id);

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

        /// <summary>
        /// Adds a new user to database with given authId.
        /// </summary>
        /// <param name="authId">AuthId to add. Should be pulled from currently logged in user.</param>
        /// <returns>New database user id.</returns>
        public uint CreateUser(string authId);

        // PROJECT

        /// <summary>
        /// Returns all users associated with a particular project
        /// </summary>
        /// <param name="project">Project id to get users for</param>
        /// <returns>List if unsigned int representing user ids</returns>
        public List<uint> GetUsersInProject(uint project);

        /// <summary>
        /// Returns all projects in database
        /// </summary>
        /// <returns>List of tuples containing project data</returns>
        public List<Project> GetProjects();

        /// <summary>
        /// Gets a project by project id
        /// </summary>
        /// <param name="id">Id of project to return</param>
        /// <returns>Tuple containing project data</returns>
        public Project GetProject(uint id);

        /// <summary>
        /// Returns all projects given user has access to
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>List of Projects accessible by user</returns>
        public List<Project> GetProjectsByUser(uint id);
    }
}

