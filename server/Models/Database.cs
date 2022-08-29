using System;
using System.Collections.Generic;       // List<>
using System.Threading.Tasks;
using MySql.Data.MySqlClient;           // Database access

namespace server.Models
{
    public class Database
    {
        // Properties
        public string Server { get; set; } = "localhost";
        public string User { get; set; } = "root";
        public string Password { get; set; }
        public string DB { get; set; }

        /// <summary>
        /// Create a new empty database object
        /// </summary>
        public Database()
        {

        }

        /// <summary>
        /// Create a new database object with connection properties
        /// </summary>
        /// <param name="server">Server path to connect to</param>
        /// <param name="user">Username to connect with</param>
        /// <param name="password">User password to connect with</param>
        /// <param name="database">Database name</param>
        public Database(string server, string user, string password, string database): base()
        {
            Server = server;
            User = user;
            Password = password;
            DB = database;
        }

        // Returns a formed connection string for MySql database connections
        private string GetConnectionString()
        {
            return $"server={Server};userid={User};password={Password};database={DB}";
        }

        // ISSUECONTROLLER

        /// <summary>
        /// Get all issues from database
        /// </summary>
        /// <returns>List of Issue objects from database</returns>
        public List<Issue> GetIssues()
        {
            // Create an empty list to store results
            List<Issue> issues = new List<Issue>();

            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.category, Issue.description, Issue.creator, User.name AS username, IFNULL(Category.name, 'undefined') AS categoryname, IFNULL(Category.color, 'undefined') AS color " +
                "FROM Issue JOIN User ON Issue.creator=User.id LEFT JOIN category ON Issue.category=category.id";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);

            using MySqlDataReader reader = command.ExecuteReader();

            // Loop through all results
            while (reader.Read())
            {
                string? catName = reader.GetString("categoryname");
                if (catName == "undefined") catName = null;
                string? catColor = reader.GetString("color");
                if (catColor == "undefined") catColor = null;
                // Create a new Issue object from result data
                Issue issue = new Issue
                {
                    Id = reader.GetUInt32("id"),
                    Title = reader.GetString("title"),
                    Priority = reader.GetInt16("priority"),
                    Creator = reader.GetString("username"),
                    CreatorId = reader.GetUInt16("creator"),
                    Category = catName,
                    CategoryId = reader.GetUInt32("category"),
                    CategoryColor = catColor,
                    Description = reader.GetString("description")
                };

                // Add result to list
                issues.Add(issue);
            }

            // Return list of result objects
            return issues;
        }

        /// <summary>
        /// Get one issue from database by id
        /// </summary>
        /// <param name="id">Id of the issue to be retreived</param>
        /// <returns>Issue from database by id</returns>
        public Issue GetIssue(uint id)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.category, Issue.description, Issue.creator, User.name AS username, IFNULL(category.name, 'undefined') AS categoryname, IFNULL(category.color, 'undefined') AS color " +
                "from Issue JOIN User ON Issue.creator=User.id LEFT JOIN category ON Issue.category=category.id WHERE Issue.id = @issueId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare WHERE value
            command.Parameters.AddWithValue("@issueID", id);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            reader.Read();      // TODO: Need some kind of error catching for invalid queries
            string? catName = reader.GetString("categoryname");
            if (catName == "undefined") catName = null;
            string? catColor = reader.GetString("color");
            if (catColor == "undefined") catColor = null;
            Issue issue = new Issue
            {
                Id = reader.GetUInt32("id"),
                Title = reader.GetString("title"),
                Priority = reader.GetInt16("priority"),
                Creator = reader.GetString("username"),
                CreatorId = reader.GetUInt16("creator"),
                Category = catName,
                CategoryId = reader.GetUInt32("category"),
                CategoryColor = catColor,
                Description = reader.GetString("description")
            };

            return issue;
        }

        /// <summary>
        /// Create a new issue
        /// </summary>
        /// <param name="issue">Issue to be created</param>
        public void CreateIssue(Issue issue)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "INSERT INTO Issue (title, priority, description, creator, category) VALUES (@issueTitle, @issuePriority, @issueDescription, @issueCreator, @issueCategory)"; // Let auto_increment handle the new id
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare VALUES
            command.Parameters.AddWithValue("@issueTitle", issue.Title);
            command.Parameters.AddWithValue("@issuePriority", issue.Priority);
            command.Parameters.AddWithValue("@issueDescription", issue.Description);
            command.Parameters.AddWithValue("@issueCreator", issue.CreatorId);
            command.Parameters.AddWithValue("@issueCategory", issue.CategoryId);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update an existing issue
        /// </summary>
        /// <param name="issue">Issue to update. Database id will be pulled from this object.</param>
        public void UpdateIssue(Issue issue)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "UPDATE Issue SET title=@issueTitle, priority=@issuePriority, description=@issueDescription, creator=@issueCreatorId, category=@issueCategoryId WHERE id=@issueId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare values
            command.Parameters.AddWithValue("@issueTitle", issue.Title);
            command.Parameters.AddWithValue("@issuePriority", issue.Priority);
            command.Parameters.AddWithValue("@issueDescription", issue.Description);
            command.Parameters.AddWithValue("@issueCreatorId", issue.CreatorId);
            command.Parameters.AddWithValue("@issueCategoryId", issue.CategoryId);
            command.Parameters.AddWithValue("@issueId", issue.Id);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes an existing issue by id
        /// </summary>
        /// <param name="id">Id of issue to delete</param>
        public void DeleteIssue(uint id)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "DELETE FROM Issue WHERE id=@issueId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare value
            command.Parameters.AddWithValue("@issueId", id);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all categories from database
        /// </summary>
        /// <returns>List of category objects</returns>
        public List<Category> GetCategories()
        {
            // Temp list to hold categories
            List<Category> categories = new List<Category>();

            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM category";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            // Loop through all results
            while (reader.Read())
            {
                // Create a new Issue object from result data
                Category category = new Category
                {
                    Id = reader.GetUInt32("id"),
                    Name = reader.GetString("name"),
                    Color = reader.GetString("color")
                };

                // Add result to list
                categories.Add(category);
            }

            return categories;
        }

        // CATEGORYCONTROLLER

        /// <summary>
        /// Gets a specific category from database by id
        /// </summary>
        /// <param name="id">Id of category to retreive</param>
        /// <returns>Category</returns>
        public Category GetCategory(uint id)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM Category WHERE id=@categoryId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare WHERE value
            command.Parameters.AddWithValue("@categoryId", id);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            reader.Read();      // TODO: Need some kind of error catching for invalid queries
            Category category = new Category
            {
                Id = reader.GetUInt32("id"),
                Name = reader.GetString("name"),
                Color = reader.GetString("color")
            };

            return category;
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="category">Category to be created</param>
        public void CreateCategory(Category category)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "INSERT INTO Category (name, color) VALUES (@categoryName, @categoryColor)"; // Let auto_increment handle the new id
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare VALUES
            command.Parameters.AddWithValue("@categoryName", category.Name);
            command.Parameters.AddWithValue("@categoryColor", category.Color);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="category">Category to update. Database id will be pulled from this object.</param>
        public void UpdateCategory(Category category)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "UPDATE Category SET name=@categoryName, color=@categoryColor WHERE id=@categoryId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare values
            command.Parameters.AddWithValue("@categoryName", category.Name);
            command.Parameters.AddWithValue("@categoryColor", category.Color);
            command.Parameters.AddWithValue("@categoryId", category.Id);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes an existing category by id
        /// </summary>
        /// <param name="id">Id of category to delete</param>
        public void DeleteCategory(uint id)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "DELETE FROM Category WHERE id=@categoryId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare value
            command.Parameters.AddWithValue("@categoryId", id);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();
        }
    }
}
