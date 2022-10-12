using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{

    [Table(name: "role_permission", Schema = "public")]
    [Serializable]
    public class RolePermission
    {
        [Key]
        public int role_permission_id { get; set; }
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual Role roles { get; set; }

   
        public int permission_id { get; set; }
        [ForeignKey("permission_id")]
        public virtual Permission permissions { get; set; }
    }
}
