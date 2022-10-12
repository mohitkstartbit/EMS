using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table("roles", Schema = "public")]

    public class Role
    {
        [Key]
        public int role_id { get; set; }
        [Required]
        public string role_name { get; set; }
        public bool is_active { get; set; }

    }
}
