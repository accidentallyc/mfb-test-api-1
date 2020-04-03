using System;
using System.ComponentModel.DataAnnotations;

namespace mfbcustomizerserver.Models
{
    public class User
    {
        [Key]
        public String Id { get; set; }
        public String FullName { get; set; }
        public String Role { get; set; }
        public String Password { get; set; }
    }
}