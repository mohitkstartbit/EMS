using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "workmode", Schema = "public")]
    public class workmode
    {
        [Key]
        public int workmode_id { get; set; }

        [Required]
        public string name { get; set; }
    }
}
