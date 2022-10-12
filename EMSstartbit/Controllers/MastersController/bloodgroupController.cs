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

    public class bloodgroupController : ControllerBase
    {
        private readonly IbloodgroupData bloodgroupdata;
        public bloodgroupController(IbloodgroupData bloodgroupdata)
        {

            this.bloodgroupdata = bloodgroupdata;
        }
        // GET: api/<bloodgroupController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await bloodgroupdata.GetAll();
                if (val.Count() != 0 && val != null)
                {
                    return Ok(val);
                }
                else
                {
                    return NotFound(new statusResponse { Message = "Data Not Found", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }



        // POST api/<bloodgroupController>
        [HttpPost]
        public async Task<IActionResult> Post(bloodgroup value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var val = await bloodgroupdata.Insert(value);
                if (val != null)
                {
                    return Ok(val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Blood Group Insert Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }


        // DELETE api/<bloodgroupController>/5
        [HttpDelete("{id}")]
  
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await bloodgroupdata.Delete(id);
                if (val != null)
                {
                    return Ok(val);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Blood Group Delete Failed", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
