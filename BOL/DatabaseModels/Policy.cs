using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BOL.DatabaseModels
{
    [Table(name:"policy",Schema ="public")]
    public class Policy
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string policy_type { get; set; }
        [Required]
        public string policy { get; set; }
        public bool is_active { get; set; }
        public DateTime created_date { get; set; }

    }
}
