using System;
namespace server.Models
{
    public class Issue
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public string Creator { get; set; }
    }
}
