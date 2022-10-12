using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "user_log_information", Schema = "public")]
    class User_log_information
    {
        [Key]
        public int log_id { get; set; }
        public string log_ifo { get; set; }
        public DateTime created_date { get; set; }
        [Required]
        public int login_id { get; set; }
        [ForeignKey("login_id")]
        public virtual Login logins { get; set; }
    }
}
