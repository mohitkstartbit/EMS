using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BOL.ApiModels
{
    public class AuthModel
    {
      
        public string name { get; set; }
     
        public string password { get; set; }
    }
}
