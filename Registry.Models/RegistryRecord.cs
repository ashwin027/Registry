using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Models
{
    // TODO: Add a user table with user related details (username, userid etc.)
    public class RegistryRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
