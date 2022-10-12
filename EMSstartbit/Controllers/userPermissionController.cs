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
    public class userPermissionController : ControllerBase
    {
        private readonly IuserPermissionData userPermissiondata;
        private readonly IemployeeData employeedata;
        public userPermissionController(IuserPermissionData userPermissiondata,IemployeeData employeedata)
        {
            this.userPermissiondata = userPermissiondata;
            this.employeedata = employeedata;
        }
        // GET: api/<userPermissionController>
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 25 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var val = await userPermissiondata.GetByEid(id);
                if (val.Count() != 0 && val != null)
                {
                    return StatusCode(200, val);
                    //return Ok(val);
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
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 25 } })]
        public async Task<IActionResult> Put(int id, List<int> permission_receive)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400,new statusResponse { Message = "Bad Request", Code = 400 });
                }

                var emp = await employeedata.GetById(id);
                var valnew = await userPermissiondata.GetAllUserPermisssionIdByEid(id);
                if( emp == null || permission_receive == null)
                {
                    return StatusCode(404,new statusResponse { Message = "Data Not Found", Code = 404 });
                }
                List<UserPermission> upval = new List<UserPermission>();
                foreach(var i in permission_receive)
                {
                    UserPermission up = new UserPermission();
                    up.employee_id = emp.employee_id;
                    up.permission_id = i;
                    up.role_id = emp.role_id;
                    upval.Add(up);
                }
                var deletedup = await userPermissiondata.DeleteMultiple(valnew);
                var result = await userPermissiondata.InsertMultiple(upval);
                if (result != null)
                {
                    return StatusCode(200, result);
                    //return Ok(result);

                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "UserPermission Update Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

       
    }
}
