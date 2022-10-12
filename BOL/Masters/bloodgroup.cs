using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "bloodgroup", Schema = "public")]
    public class bloodgroup
    {
        [Key]
        public int bloodgroup_id { get; set; }

        [Required]
        public string group { get; set; }
    }
}
