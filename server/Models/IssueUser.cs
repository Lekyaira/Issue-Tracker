using System;
namespace server.Models
{
    public record IssueUser
    {
        public Issue issue { get; set; }
        public User user { get; set; }
    }
}

