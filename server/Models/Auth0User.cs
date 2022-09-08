using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Auth0.ManagementApi;

namespace server.Models
{
    public class Auth0User: IAuth0User
    {
        private Auth0Token _auth;
        private ManagementApiClient _client;

        public Auth0User()
        {
            _auth = new Auth0Token(
                     "https://dev-7gr-w4iu.us.auth0.com",                               // Auth0 REST server
                     "8r40EwEOdnjbzPbrqnTJxDBy45hgL01r",                                // Client ID
                     "GhkDV-uL6RF63_PAoL7lAflYPefFMY_YxRM15EFPiybAmlUE76VB_ew6plGa9QOy" // Client Secret
                 );
        }

        public async Task<User> GetUserAsync(string authId)
        {
            // Create Auth0 connection client
            var client = new ManagementApiClient(
                    _auth.AccessToken,                                                  // Returns string access_token
                                                                                        // This is a .Wait() if the token hasn't been retreived before
                    new Uri("https://dev-7gr-w4iu.us.auth0.com/api/v2/")                // From the Auth0 dashboard
                );
            // Get user info from Auth0 server
            var result = await client.Users.GetAsync(authId);

            // Return the new user
            return new User() { Id = authId, Name = result.FirstName, Email = result.Email };
        }
    }
}

