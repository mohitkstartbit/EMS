using BAL;
using BOL;
using BOL.Responses;
using EMSstartbit.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMSstartbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 /*
    [TokenAuthenticationFilter(1)]*/
    
    public class usersController : ControllerBase
    {
        private IemployeeData _ldata;
        public usersController(IemployeeData ldata)
        {
            _ldata = ldata;
        }
        // GET: api/<UsersController>
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 25 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await _ldata.GetAll();
                if (val.Count() != 0 && val != null)
                {
                    var empData = from res in val
                                  select new { res.employee_id,employee_name = res.firstname +" "+ res.middlename +" "+ res.lastname,role_id=res.role_id };
                    return StatusCode(200, empData);
                    ///return Ok(empData);
                }
                else
                {
                    return StatusCode(404, new statusResponse{ Message = "Data Not Found", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

      
     
    }
}
