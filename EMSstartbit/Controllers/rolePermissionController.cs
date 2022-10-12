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
    public class rolePermissionController : ControllerBase
    {
        private readonly IrolePermissionData rolePermissiondata;
        private readonly IemployeeData employeedata;
        private readonly IuserPermissionData userPermissiondata;
        public rolePermissionController(IuserPermissionData userPermissiondata, IemployeeData employeedata, IrolePermissionData rolePermissiondata)
        {
            this.userPermissiondata = userPermissiondata;
            this.employeedata = employeedata;
            this.rolePermissiondata = rolePermissiondata;
        }
        // GET: api/<rolePermissionController>
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 26 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var val = await rolePermissiondata.GetByroleid(id);
                if (val.Count() != 0 && val != null)
                {
                    return StatusCode(200, val);
                    
                }
                else
                {
                    return StatusCode(404,new statusResponse { Message = "Data Not Found", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        

        //// PUT api/<roleController>/5
        [HttpPut()]
        [Route("{id:int}/{istrue:bool}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 26 } })]
        public async Task<IActionResult> Put(int id,bool istrue,List<int> permission_receive)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new statusResponse { Message = "Bad Request", Code = 400 });
                }

             
                var roleids = await rolePermissiondata.GetPermissionIdsByroleid(id);
                var ids = permission_receive.Select(x => x).Intersect(roleids.Select(x => x));
                var toadd = permission_receive.Where(p => !ids.Any(p2 => p2 == p));
                var todelete = roleids.Where(p => !ids.Any(p2 => p2 == p));
                var deletedRolePremissions =await rolePermissiondata.DeleteByRoleAndPermissionids(id, todelete);
                var addedRolePremissions =await rolePermissiondata.AddByRoleAndPermissionids(id, toadd);
                if (istrue)
                {
                    var emploeidlist = await employeedata.GetEmployeeIdswithRoleids(id);
                    var deleteduserpermissionlist = await userPermissiondata.DeleteByRoleAndPermissionids(id, todelete);
                    var addeduserpermisionlist = await userPermissiondata.AddByRoleAndPermissionidsAndEmp(emploeidlist, id, toadd);

                }
                return StatusCode(200, new statusResponse { Message= "Changed Successfully" ,Code=200});
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

       
    }
}
