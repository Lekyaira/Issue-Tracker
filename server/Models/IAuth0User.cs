using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Models
{
    public interface IAuth0User
    {
        public Task<(string name, string email)> GetUserAsync(string authId);
        public Task<List<(string name, string authId)>> GetUserByEmailAsync(string email);
    }
}

