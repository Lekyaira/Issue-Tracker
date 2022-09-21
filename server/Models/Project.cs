using System.Collections.Generic;

namespace server.Models
{
    public class Project
    {
        public uint? Id { get; set; }
        public string Name { get; set; }
        public uint? Owner { get; set; }
        public List<uint> Users { get; set; }
    }
}

