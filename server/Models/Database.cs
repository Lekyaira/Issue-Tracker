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
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.description, User.name FROM Issue JOIN User ON Issue.creator=User.id";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);

            using MySqlDataReader reader = command.ExecuteReader();

            // Loop through all results
            while (reader.Read())
            {
                // Create a new Issue object from result data
                Issue issue = new Issue
                {
                    Id = reader.GetUInt32("id"),
                    Title = reader.GetString("title"),
                    Priority = reader.GetInt16("priority"),
                    Creator = reader.GetString("name"),
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
        public Issue GetIssue(int id)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.description, User.name from Issue JOIN User ON Issue.creator=User.id WHERE Issue.id = @issueId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare WHERE value
            command.Parameters.AddWithValue("@issueID", id);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            reader.Read();
            Issue issue = new Issue
            {
                Id = reader.GetUInt32("id"),
                Title = reader.GetString("title"),
                Priority = reader.GetInt16("priority"),
                Creator = reader.GetString("name"),
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
            string QUERYSTRING = "SELECT id FROM User WHERE name = @userName"; // TODO: Probably want to pass the user id from the POST rather than look it up by name - perhaps tuple (int, Issue)?
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare WHERE value
            command.Parameters.AddWithValue("@userName", issue.Creator);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            // Get the user id
            reader.Read();
            uint id = reader.GetUInt32("id");

            QUERYSTRING = "INSERT INTO Issue (title, priority, description, creator) VALUES (@issueTitle, @issuePriority, @issueDescription, @issueCreator)"; // Let auto_increment handle the new id
            using MySqlCommand insertCommand = new MySqlCommand(QUERYSTRING, conn);
            // Prepare VALUES
            insertCommand.Parameters.AddWithValue("@issueTitle", issue.Title);
            insertCommand.Parameters.AddWithValue("@issuePriority", issue.Priority);
            insertCommand.Parameters.AddWithValue("@issueDescription", issue.Description);
            insertCommand.Parameters.AddWithValue("@issueCreator", id);
            insertCommand.Prepare();
            // Execute command
            insertCommand.ExecuteNonQuery();
        }

        // Returns a formed connection string for MySql database connections
        private string GetConnectionString()
        {
            return $"server={Server};userid={User};password={Password};database={DB}";
        }
    }
}
