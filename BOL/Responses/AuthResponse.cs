using System.Collections.Generic;

namespace BOL.Responses
{
    public class AuthResponse
    {
        public StatusResponse status { get; set; }
        public IEnumerable<Permission> permissionlist { get; set; }
        public Employee employedata { get; set; }

    }
}
