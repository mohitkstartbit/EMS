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

    public class departmentController : ControllerBase
    {
        private readonly IdepartmentData departmentdata;
        public departmentController(IdepartmentData departmentdata)
        {

            this.departmentdata = departmentdata;
        }
        // GET: api/<departmentController>
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11,18 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await departmentdata.GetAll();
                if (val.Count() != 0 && val != null)
                {
                    return StatusCode(200, val);
                }
                else
                {
                    return StatusCode(404, new statusResponse { Message = "Data Not Found", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // GET api/<departmentController>/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] {11,18} })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var val = await departmentdata.GetById(id);
                if (val != null)
                {
                    return StatusCode(200,val);
                }
                else
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // POST api/<departmentController>
        [HttpPost]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 18 } })]
        public async Task<IActionResult> Post(department value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                value.is_active = true;
                var val = await departmentdata.Insert(value);
                if (val != null)
                {
                    return StatusCode(200, val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Department Insert Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // PUT api/<departmentController>/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] {18 } })]
        public async Task<IActionResult> Put(int id, department value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                value.department_id = id;
                var dept = await departmentdata.GetById(id);
                if(dept == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Department not found or internal error", Code = 404 });
                }
                dept.description = value.description;
                dept.is_active = value.is_active;
                dept.name = value.name;
                var valnew = await departmentdata.Update(dept);
                if (valnew != null)
                {
                    return StatusCode(200,valnew);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Department Update Failed", Code = 500 });
                }



            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // DELETE api/<departmentController>/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 18 } })]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await departmentdata.Delete(id);
                if (val != null)
                {
                    return StatusCode(200,val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Department Delete Failed", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
