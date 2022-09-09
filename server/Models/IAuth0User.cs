using System;
using System.Threading.Tasks;

namespace server.Models
{
    public interface IAuth0User
    {
        public Task<(string name, string email)> GetUserAsync(string authId);
    }
}

