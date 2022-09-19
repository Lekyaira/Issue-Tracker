using System;
using System.Collections.Generic;

using server.Models;

namespace server
{
    public class Access: IAccess
    {
        // Database service
        private readonly IDatabase _db;

        public Access(IDatabase database)
        {
            _db = database;
        }

        // Checks it a user is associated with the given project
        public bool isUserAllowedInProject(uint id, uint projectId)
        {
            // Check if user is allowed in this project
            // If the current user is the project's owner, it's allowed
            if (_db.GetProject(projectId).Owner == id) return true;

            // Get all users associated with the project from database
            List<uint> users = _db.GetUsersInProject(projectId);
            foreach (uint user in users)
            {
                // If the user is in the list, it's allowed
                if (user == id)
                {
                    return true;
                }
            }

            // No allowed users found
            return false;
        }
    }
}

