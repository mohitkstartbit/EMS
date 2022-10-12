using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "log_information", Schema = "public")]
    class Log_information
    {
        [Key]
        public int log_id { get; set; }
        public string log_ifo { get; set; }
        public DateTime created_date { get; set; }
    }
}
