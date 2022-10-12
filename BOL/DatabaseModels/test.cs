using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "test", Schema = "public")]
    public class Test
    {
        [Key]

        public int id { get; set; }
        public string name { get; set; }
        public int no { get; set; }
        public string image { get; set; }
       
    }
}