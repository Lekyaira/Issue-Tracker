using System;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task<(string name, string email)> GetUserAsync(string authId)
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
            return (result.FirstName, result.Email);
        }

        public async Task<List<(string name, string authId)>> GetUserByEmailAsync(string email)
        {
            // Create Auth0 connection client
            var client = new ManagementApiClient(                                       
                _auth.AccessToken,                                                      // Returns string access_token
                                                                                        // This is a .Wait() if the token hasn't been retreived before
                new Uri("https://dev-7gr-w4iu.us.auth0.com/api/v2/")                    // From the Auth0 dashboard
            );

            // Get user info from Auth0 server
            var result = await client.Users.GetUsersByEmailAsync(email);

            // Make an empty list for our return object
            List<(string name, string authId)> users = new();

            // Loop through results and populate our list
            foreach (var user in result)
            {
                users.Add((user.FirstName, user.UserId));
            }

            // Return the result
            return users;
        }
    }
}

