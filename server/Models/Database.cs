using System;
using System.Collections.Generic;       // List<>
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;           // Database access

namespace server.Models
{
    public class Database: IDatabase
    {
        /// <summary>
        /// Create a new empty database object
        /// </summary>
        public Database()
        {
            
        }

        // Returns a formed connection string for MySql database connections
        private string GetConnectionString()
        {
            return $"server=localhost;userid=ryan;password=Taradhve;database=IssueTracker";     //TODO: Pull this from appsettings, probably
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
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.category, Issue.description, Issue.creator, IFNULL(Category.name, 'undefined') AS categoryname, IFNULL(Category.color, 'undefined') AS color, category.project " +
                "FROM Issue LEFT JOIN category ON Issue.category=category.id ORDER BY Issue.priority";
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
                    Priority = reader.GetInt32("priority"),
                    CreatorId = reader.GetUInt32("creator"),
                    Category = catName,
                    CategoryId = reader.GetUInt32("category"),
                    CategoryColor = catColor,
                    Description = reader.GetString("description"),
                    ProjectId = reader.GetUInt32("project")
                };

                // Add result to list
                issues.Add(issue);
            }

            // Return list of result objects
            return issues;
        }

        /// <summary>
        /// Get all issues under specified project id.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns>List of issues</returns>
        public List<Issue> GetIssuesByProject(uint id)
        {
            // Create an empty list to store results
            List<Issue> issues = new List<Issue>();

            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.category, Issue.description, Issue.creator, IFNULL(Category.name, 'undefined') AS categoryname, IFNULL(Category.color, 'undefined') AS color, category.project " +
                "FROM Issue LEFT JOIN category ON Issue.category=category.id WHERE category.project = @projectId ORDER BY Issue.priority";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare WHERE
            command.Parameters.AddWithValue("@projectId", id);
            command.Prepare();

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
                    Priority = reader.GetInt32("priority"),
                    CreatorId = reader.GetUInt32("creator"),
                    Category = catName,
                    CategoryId = reader.GetUInt32("category"),
                    CategoryColor = catColor,
                    Description = reader.GetString("description"),
                    ProjectId = reader.GetUInt32("project")
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
            string QUERYSTRING = "SELECT Issue.id, Issue.title, Issue.priority, Issue.category, Issue.description, Issue.creator, IFNULL(category.name, 'undefined') AS categoryname, IFNULL(category.color, 'undefined') AS color, category.project " +
                "from Issue LEFT JOIN category ON Issue.category=category.id WHERE Issue.id = @issueId";
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
                Priority = reader.GetInt32("priority"),
                CreatorId = reader.GetUInt32("creator"),
                Category = catName,
                CategoryId = reader.GetUInt32("category"),
                CategoryColor = catColor,
                Description = reader.GetString("description"),
                ProjectId = reader.GetUInt32("project")
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

        // CATEGORYCONTROLLER

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
                    Color = reader.GetString("color"),
                    ProjectId = reader.GetUInt32("project")
                };

                // Add result to list
                categories.Add(category);
            }

            return categories;
        }

        /// <summary>
        /// Gets all categories by project id
        /// </summary>
        /// <param name="id">Id of project to retreive</param>
        /// <returns>List of categories</returns>
        public List<Category> GetCategoriesByProject(uint id)
        {
            // Temp list to hold categories
            List<Category> categories = new List<Category>();

            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM category WHERE project=@projectId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare WHERE value
            command.Parameters.AddWithValue("@projectId", id);
            command.Prepare();
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
                    Color = reader.GetString("color"),
                    ProjectId = reader.GetUInt32("project")
                };

                // Add result to list
                categories.Add(category);
            }

            return categories;
        }

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
                Color = reader.GetString("color"),
                ProjectId = reader.GetUInt32("project")
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
            string QUERYSTRING = "INSERT INTO Category (name, color, project) VALUES (@categoryName, @categoryColor, @projectId)"; // Let auto_increment handle the new id
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare VALUES
            command.Parameters.AddWithValue("@categoryName", category.Name);
            command.Parameters.AddWithValue("@categoryColor", category.Color);
            command.Parameters.AddWithValue("@projectId", category.ProjectId);
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
            string QUERYSTRING = "UPDATE Category SET name=@categoryName, color=@categoryColor, project=@projectId WHERE id=@categoryId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare values
            command.Parameters.AddWithValue("@categoryName", category.Name);
            command.Parameters.AddWithValue("@categoryColor", category.Color);
            command.Parameters.AddWithValue("@categoryId", category.Id);
            command.Parameters.AddWithValue("@projectId", category.ProjectId);
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

        // USERCONTROLLER

        /// <summary>
        /// Returns a user from database by DB id
        /// </summary>
        /// <param name="id">Id to retreive</param>
        /// <returns></returns>
        public string GetUser(uint id)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM user WHERE id=@userId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare value
            command.Parameters.AddWithValue("@userId", id);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            // Check if values were returned
            if (reader.HasRows)
            {
                // Read and return data from database
                reader.Read();      // TODO: Need some kind of error catching for invalid queries
                return reader.GetString("authId");
            }
            else // If no values were returned, tell the caller that no users were found
            {
                // TODO: Handle this error more nicely
                throw new Exception("User Id not found!");
            }
        }

        /// <summary>
        /// Returns a user from database by auth id
        /// </summary>
        /// <param name="authId">Id to retreive by</param>
        /// <returns></returns>
        public uint GetUser(string authId)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM user WHERE authId=@authId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare value
            command.Parameters.AddWithValue("@authId", authId);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            // Check if values were returned
            if (reader.HasRows)
            {
                // Read and return data from database
                reader.Read();      // TODO: Need some kind of error catching for invalid queries
                return reader.GetUInt32("id");
            }
            else // If no values were returned, we need to add the logged in user to the database
            {
                // Create the new user and return the new database id
                return CreateUser(authId);
            }
        }

        /// <summary>
        /// Adds a new user to database with given authId.
        /// </summary>
        /// <param name="authId">AuthId to add. Should be pulled from currently logged in user.</param>
        /// <returns>New database user id.</returns>
        public uint CreateUser(string authId)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "INSERT INTO user (authId) VALUES (@authId)";      // Let the database assign a new id
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare value
            command.Parameters.AddWithValue("@authId", authId);
            command.Prepare();
            // Execute command
            command.ExecuteNonQuery();

            // Return the new user's database id
            return GetUser(authId);
        }

        /// <summary>
        /// Get userId and authId from all users associated with a project
        /// </summary>
        /// <param name="projectId">Project to retreive users from</param>
        /// <returns></returns>
        public List<(uint userId, string authId)> UserGetUsersInProject(uint projectId)
        {
            // Create an empty list to hold the data
            List<(uint userId, string authId)> usersList = new();

            // Get all the users associated with the project
            List<uint> users = GetUsersInProject(projectId);

            if (users.Count > 0)
            {

                // Get user authIds from database
                // Connect to the database
                using MySqlConnection conn = new MySqlConnection(GetConnectionString());
                conn.Open();

                // Create query and execute
                string QUERYSTRING = "SELECT * FROM user WHERE IN (@userId)";
                using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
                {
                    // Prepare variables
                    command.Parameters.AddWithValue("@userId", String.Join(',', users));
                    command.Prepare();

                    // Execute command
                    using MySqlDataReader reader = command.ExecuteReader();

                    // Read the data out and populate our output list
                    while (reader.Read())
                    {
                        usersList.Add((reader.GetUInt32("id"), reader.GetString("authId")));
                    }
                }
            }

            return usersList;
        }

        // PROJECTCONTROLLER

        /// <summary>
        /// Returns all users associated with a particular project
        /// </summary>
        /// <param name="project">Project id to get users for</param>
        /// <returns>List if unsigned int representing user ids</returns>
        public List<uint> GetUsersInProject(uint project)
        {
            // connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT user FROM userproject WHERE project=@projectId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare value
            command.Parameters.AddWithValue("@projectId", project);
            command.Prepare();
            // Execute command
            using MySqlDataReader reader = command.ExecuteReader();

            // Create a temp list to hold users
            List<uint> users = new List<uint>();

            // For each user returned, add it to the list
            while (reader.Read())
            {
                users.Add(reader.GetUInt32("user"));
            }

            // Return the list
            return users;
        }

        /// <summary>
        /// Returns all projects in database
        /// </summary>
        /// <returns>List of tuples containing project data</returns>
        public List<Project> GetProjects()
        {
            // Create an empty list to store results
            List<Project> projects = new();

            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM project";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);

            using MySqlDataReader reader = command.ExecuteReader();

            // Loop through all results
            while (reader.Read())
            {
                // Get all users associated with this project
                List<uint> users = GetUsersInProject(reader.GetUInt32("id"));

                // Get the rest of the project data
                uint id = reader.GetUInt32("id");
                string name = reader.GetString("name");
                uint owner = reader.GetUInt32("owner");

                // Add result to list
                projects.Add(new Project { Id = id, Name = name, Owner = owner, Users = users });
            }

            // Return list of result objects
            return projects;
        }

        /// <summary>
        /// Gets a project by project id
        /// </summary>
        /// <param name="id">Id of project to return</param>
        /// <returns>Tuple containing project data</returns>
        public Project GetProject(uint id)
        {
            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM project WHERE id=@projectId";
            using MySqlCommand command = new MySqlCommand(QUERYSTRING, conn);
            // Prepare variable
            command.Parameters.AddWithValue("@projectId", id);
            command.Prepare();

            using MySqlDataReader reader = command.ExecuteReader();

            // If we found data, return it. Else throw an error.
            if (reader.HasRows)
            {
                reader.Read();
                // Get all users assoeciated with this project
                List<uint> users = new();
                users = GetUsersInProject(id);
                return new Project { Id = id, Name = reader.GetString("name"), Owner = reader.GetUInt32("owner"), Users = users };
            }
            else
            {
                throw new Exception("Project does not exist!");
            }
        }

        /// <summary>
        /// Returns all projects given user has access to
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>List of Projects accessible by user</returns>
        public List<Project> GetProjectsByUser(uint id)
        {
            // Temp list to store data
            List<Project> projects = new();

            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Get all projects the user owns
            // Create the query and execute
            string QUERYSTRING = "SELECT * FROM project WHERE owner=@ownerId";
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {


                // Prepare variable
                command.Parameters.AddWithValue("@ownerId", id);
                command.Prepare();

                using MySqlDataReader reader = command.ExecuteReader();

                // Add all projects the user owns to the list
                while (reader.Read())
                {
                    projects.Add(new Project {
                        Id = reader.GetUInt32("id"),
                        Name = reader.GetString("name"),
                        Owner = id,
                        Users = GetUsersInProject(reader.GetUInt32("id"))
                    });
                }

                // Close the reader so we can open a new one
                reader.Close();
            }

            // Get the projects the user has access to
            QUERYSTRING = "SELECT * FROM project JOIN userproject ON userproject.project=project.id WHERE userproject.user=@userId";
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare variable
                command.Parameters.AddWithValue("@userId", id);
                command.Prepare();

                using MySqlDataReader reader = command.ExecuteReader();

                // Add all projects the user is associated with to the list
                while (reader.Read())
                {
                    projects.Add(new Project {
                        Id = reader.GetUInt32("id"),
                        Name = reader.GetString("name"),
                        Owner = reader.GetUInt32("owner"),
                        Users = GetUsersInProject(reader.GetUInt32("id"))
                    });
                }
            }

            // Return the completed list
            return projects;
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="project">Project to put into the database</param>
        public void CreateProject(Project project)
        {
            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "INSERT INTO project (name, owner) VALUES (@projectName, @projectOwner)";  // We'll let the database assign an id
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare variables
                command.Parameters.AddWithValue("@projectName", project.Name);
                command.Parameters.AddWithValue("@projectOwner", project.Owner);
                command.Prepare();

                // Execute command
                command.ExecuteNonQuery();

                // Get the generated id from the database
                // NOTE: This is not threadsafe. If using connection pools later this will need to be changed.
                project.Id = (uint)command.LastInsertedId;
            }

            // Insert the associated users into the database
            QUERYSTRING = "INSERT INTO userproject (user, project) VALUES (@user, @project)";
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare the variables
                command.Prepare();

                // Loop through all entries and add them to the databse
                foreach (uint user in project.Users)
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@project", project.Id);
                    // Execute the command
                    command.ExecuteNonQuery();
                    // Clear the parameters to prepare for the next pass
                    command.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Updates an existing project in database
        /// </summary>
        /// <param name="project">Project to be updated</param>
        public void UpdateProject(Project project)
        {
            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "UPDATE project SET name=@projectName, owner=@projectOwner WHERE id=@projectId";
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare variables
                command.Parameters.AddWithValue("@projectName", project.Name);
                command.Parameters.AddWithValue("@projectOwner", project.Owner);
                command.Parameters.AddWithValue("@projectId", project.Id);
                command.Prepare();
                // Execute the command
                command.ExecuteNonQuery();
            }

            // Delete existing users associated with the project.
            QUERYSTRING = "DELETE FROM userproject WHERE project=@projectId";
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare variables
                command.Parameters.AddWithValue("@projectId", project.Id);
                command.Prepare();
                // Execute the command
                command.ExecuteNonQuery();
            }

            // Add the users back in. This allows us to "modify" existing users, "add" new users and "delete" removed users.
            QUERYSTRING = "INSERT INTO userproject (user, project) VALUES (@user, @project)";
            using(MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare variables
                command.Prepare();

                // Add each user
                foreach(uint user in project.Users)
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@project", project.Id);
                    // Execute the command
                    command.ExecuteNonQuery();
                    // Clear the parameters to prepare for the next pass
                    command.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Delete a project from the database
        /// </summary>
        /// <param name="id">Project id to be deleted</param>
        public void DeleteProject(uint id)
        {
            // Connect to the database
            using MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();

            // Create the query and execute
            string QUERYSTRING = "DELETE FROM project WHERE id=@projectId";
            using (MySqlCommand command = new MySqlCommand(QUERYSTRING, conn))
            {
                // Prepare variables
                command.Parameters.AddWithValue("@projectId", id);
                command.Prepare();
                // Exectue the command
                command.ExecuteNonQuery();      // We don't worry about deleting associated user links because that should be handled by database cascade
            }
        }
    }
}
