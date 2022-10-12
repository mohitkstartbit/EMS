using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BOL.DatabaseModels
{
    [Table(name: "email_template", Schema = "public")]
    public class EmailTemplate
    {
        [Key]
        public int id { get; set; }
       
        public string email_type { get; set; }
        [Required]
        public string emailtemplate { get; set; }
        public bool is_active { get; set; }
        public DateTime created_date { get; set; }

    }
}
