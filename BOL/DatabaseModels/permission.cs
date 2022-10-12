using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table("permissions", Schema = "public")]
    [Serializable]
    public class Permission
    {
        [Key]
        public int permission_id { get; set; }
        public string permission_title { get; set; }
        public bool is_active { get; set; }
        /*  public virtual ICollection<logi> logins { get; set; }
          public virtual ICollection<role> roles { get; set; }*/
    }
}
