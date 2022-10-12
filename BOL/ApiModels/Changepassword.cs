using System;
using System.Collections.Generic;
using System.Text;

namespace BOL.ApiModels
{
   public class Changepassword
    {
        public int id { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
        public string confirmpassword { get; set; }
    }
}
