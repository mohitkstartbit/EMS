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
    public class designationController : ControllerBase
    {
        private readonly IdesignationData designationdata;
        public designationController(IdesignationData designationdata)
        {
            this.designationdata = designationdata;
        }

        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11, 19 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await designationdata.GetAll();
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

        // GET api/<designationController>/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11, 19 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var val = await designationdata.GetById(id);
                if (val != null)
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

        // POST api/<designationController>
        [HttpPost]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 19 } })]
        public async Task<IActionResult> Post(designation value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                value.is_active = true;
                var val = await designationdata.Insert(value);
                if (val != null)
                {
                    return StatusCode(200, val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Designation Insert Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // PUT api/<designationController>/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 19 } })]
        public async Task<IActionResult> Put(int id, designation value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var desg = await designationdata.GetById(id);
                if (desg == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Designation not found or Internal server error", Code = 404 });
                }
                desg.description = value.description;
                desg.name = value.name;
                desg.is_active = value.is_active;
                var valnew = await designationdata.Update(desg);
                if (valnew != null)
                {
                    return StatusCode(200, valnew);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Designation Update Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // DELETE api/<designationController>/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 19 } })]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await designationdata.Delete(id);
                if (val != null)
                {
                    return StatusCode(200, val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Designation Delete Failed", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
