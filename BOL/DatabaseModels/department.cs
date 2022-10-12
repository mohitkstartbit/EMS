using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "department", Schema = "public")]
    public class Department
    {
        [Key]

        public int department_id { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
    }
}