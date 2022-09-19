using System;
namespace server.Models
{
    public class Issue
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public uint CreatorId { get; set; }
        public string? Category { get; set; }
        public uint? CategoryId { get; set; }
        public string? CategoryColor { get; set; }
        public uint ProjectId { get; set; }
    }
}
