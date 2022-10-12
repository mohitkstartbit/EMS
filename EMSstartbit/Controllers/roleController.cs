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
    public class roleController : ControllerBase

    {
        private readonly IroleData roledata;
        public roleController(IroleData roledata)
        {
            this.roledata = roledata;
        }
        // GET: api/<roleController>
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 24,11 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await roledata.GetAll();
                if (val.Count() != 0 && val!=null)
                {
                    
                    return StatusCode(200,val);

                }
                else
                {
                    return StatusCode(404,new statusResponse { Message = "Data Not Found", Code = 404 });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500,new statusResponse { Message= ex.Message ,Code=500});
            }
        }

        // GET api/<roleController>/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 24,11 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if(id == 0)
                {
                    return StatusCode(400,new statusResponse {Message="Bad Request" ,Code=400});
                }
                var val = await roledata.GetById(id);
                if (val != null)
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

        // POST api/<roleController>
        [HttpPost]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 24 } })]
        public async Task<IActionResult> Post(role value)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(new statusResponse { Message = "Bad Request", Code = 400 });
                }
              value.is_active = true;
                
                var val = await roledata.Insert(value);
                if (val != null)
                {
                    return StatusCode(200, val);
                    
                }
                else
                {
                    return StatusCode(500,new statusResponse { Message = "Role Insert Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // PUT api/<roleController>/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 24 } })]
        public async Task<IActionResult> Put(int id,role value)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return StatusCode(400,new statusResponse { Message = "Bad Request", Code = 400 });
                }

                var result = await roledata.GetById(id);
                if(result == null)
                {
                    return StatusCode(404,new statusResponse { Code=404,Message="Role not Found"});
                }
                result.role_name = value.role_name;
                result.is_active = value.is_active;
                    var valnew = await roledata.Edit(result);
                    if (valnew != null)
                    {
                    return StatusCode(200, valnew);
                   
                }
                    else
                    {
                        return StatusCode(500, new statusResponse { Message = "Role Update Failed", Code = 500 });
                    }   
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // DELETE api/<roleController>/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 24 } })]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await roledata.Delete(id);
                    if (val != null)
                {
                    return StatusCode(200, val);
                   
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Role Delete Failed", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
