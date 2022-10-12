using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSstartbit.TokenAuthentication
{
    public class Token
    {
        public string value { get; set; }
        public DateTime ExpiryDate { get; set;  }
    }
}
