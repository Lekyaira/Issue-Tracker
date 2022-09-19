using System;
namespace server.Models
{
    public class Category
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public uint ProjectId { get; set; }
    }
}
