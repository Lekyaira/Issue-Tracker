using System;
namespace server
{
    public interface IAccess
    {
        // Checks it a user is associated with the given project
        public bool isUserAllowedInProject(uint id, uint projectId);
    }
}

