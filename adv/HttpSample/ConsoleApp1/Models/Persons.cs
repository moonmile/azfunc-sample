using System;
using System.Collections.Generic;

namespace ConsoleApp1.Models
{
    public partial class Persons
    {
        public int Id { get; set; }
        public string PersonNo { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
