using RestSharp;
using RestSharp.Authenticators;

namespace Auth0ManagementAccessTest
{
    /// <summary>
    /// Generates an access token for Auth0 management api from their server.
    /// </summary>
    public class Auth0Token
    {
        // Internal variables
        readonly string _baseUrl;
        readonly string _clientId;
        readonly string _clientSecret;

        /// <summary>
        /// DateTime when the access token expires.
        /// Will renew automatically when Token is next accessed.
        /// Null if Token has never been accessed (read).
        /// </summary>
        public DateTime? Expires { get; protected set; } = null;

        private AuthorizationTokenResponse? _token = null;
        /// <summary>
        /// Token returned by Auth0 server.
        /// Updates automatically when read, if needed.
        /// </summary>
        public AuthorizationTokenResponse Token
        {
            get
            {
                if (_token is null || Expires < DateTime.Now)
                {
                    GetToken().Wait();
                    _token = GetToken().Result;
                }
                return _token;
            }
            protected set
            {
                _token = value;
            }
        }

        // Expose Token properties
        public string AccessToken => Token.AccessToken;
        public string TokenType => Token.TokenType;
        public string Scope => Token.Scope;

        /// <summary>
        /// Creates an Auth0 authorization token for management API access.
        /// </summary>
        /// <param name="baseUrl">Auth0 REST server URL</param>
        /// <param name="clientId">ClientID from Auth0 dashboard</param>
        /// <param name="clientSecret">Client Secret from Auth0 dashboard</param>
        public Auth0Token(string baseUrl, string clientId, string clientSecret)
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// Gets the authentication token from Auth0 servers.
        /// </summary>
        /// <returns>AuthorizationTokenResponse</returns>
        /// <exception cref="Exception">If the response is null, will throw an generic exception.</exception>
        protected async Task<AuthorizationTokenResponse> GetToken()
        {
            // Create the REST client
            var client = new RestClient(_baseUrl);                                          // _baseUrl is the Auth0 REST server
            client.Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret);    // _clientId and _clientSecret are from the
                                                                                            // Auth0 dashboard.

            // Create the request
            var request = new RestRequest("oauth/token")                                    // "oauth/token" is the server endpoint
                .AddParameter("grant_type", "client_credentials")                           // Required paremeter as-is, nothing special here
                .AddParameter("audience", "https://dev-7gr-w4iu.us.auth0.com/api/v2/");     // Audience field from the Auth0 dashboard

            // Request the token from Auth0 server
            var response = await client.PostAsync<AuthorizationTokenResponse>(request);
            if (response is null) throw new Exception("Response is null!");                 // VS complained that this might be null

            // Update Expires
            Expires = DateTime.Now.AddSeconds(response.ExpiresIn);                          // Used to check when we need to renew

            // Return the response
            return response;
        }
    }
}

