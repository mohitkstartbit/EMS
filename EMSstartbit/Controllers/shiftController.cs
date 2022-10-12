using BAL;
using BOL;
using BOL.Responses;
using EMSstartbit.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSstartbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class shiftController : ControllerBase
    {

        private readonly IshiftData shiftdata;
        public shiftController(IshiftData shiftdata)
        {

            this.shiftdata = shiftdata;
        }
        // GET: api/<shiftController>
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await shiftdata.GetAll();
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

        // GET api/<shiftController>/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode(400,new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var val = await shiftdata.GetById(id);
                if (val != null)
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

        // POST api/<shiftController>
        [HttpPost]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11} })]
        public async Task<IActionResult> Post(shift value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400,new statusResponse { Message = "Bad Request", Code = 400 });
                }
                
                //if (value.is_active == null)
                //{
                //    value.is_active = true;
                //}
                var val = await shiftdata.Insert(value);
                if (val != null)
                {
                    return StatusCode(200, val);
                    //return Ok(val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Shift Insert Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // PUT api/<shiftController>/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Put(int id, shift value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400,new statusResponse { Message = "Bad Request", Code = 400 });
                }
                value.shift_id = id;
                var valnew = await shiftdata.Update(value);
                if (valnew != null)
                {
                    return StatusCode(200, valnew);
                    //return Ok(valnew);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Shift Update Failed", Code = 500 });
                }



            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // DELETE api/<shiftController>/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await shiftdata.Delete(id);
                if (val != null)
                {
                    return StatusCode(200, val);
                    //return Ok(val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Shift Delete Failed", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
