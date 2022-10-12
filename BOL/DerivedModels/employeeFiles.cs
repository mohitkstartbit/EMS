using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOL.DerivedClasses
{
   public  class employeeFiles : Employee
    {
        public ICollection<IFormFile> panfile { get; set; }
        public ICollection<IFormFile> aadharfile { get; set; }
        public ICollection<IFormFile> cancelcheckfile { get; set; }
        public ICollection<IFormFile> ppfile { get; set; }
     
        
    }
}
