using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BOL.ApiModels
{
    public class PermissionReceive
    {
        [Required]
        public int employee_id { get; set; }
        [Required]
        public List<Permission> permisionlist { get; set; }
    }
}
