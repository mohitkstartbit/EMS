using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "login", Schema = "public")]

    public class Login
    {
        [Key]

        public int login_id { get; set; }
        public int employee_id { get; set; }

        [ForeignKey("employee_id")]
        public virtual Employee employees { get; set; }
        public string password { get; set; }
        public DateTime created_date { get; set; }

        public bool is_active { get; set; }

        public int role_id { get; set; }

        [ForeignKey("role_id")]
        public virtual Role roles { get; set; }
        public string? refreshtoken { get; set; }
        public DateTime? refreshtokenexpires { get; set; }
    }
}
