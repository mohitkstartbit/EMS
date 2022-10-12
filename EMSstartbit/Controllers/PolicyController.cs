using BAL;
using BOL.DatabaseModels;
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

    public class policyController : ControllerBase
    {
        public readonly IPolicyData policyData;
        public policyController(IPolicyData policyData)
        {
            this.policyData = policyData;
        }
        // GET: api/<PolicyController>
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] {2, 27 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await policyData.GetAll();
                if (val.Count() != 0 && val != null)
                {
                    
                    return StatusCode(200, val);
                }
                else
                {
                    return StatusCode(404, new statusResponse { Message = "Data Not Found", Code = 404 });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // GET api/<PolicyController>/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 2, 27 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                   
                }
                var val = await policyData.GetById(id);
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

        // POST api/<PolicyController>
        [HttpPost]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 27 } })]
        public async Task<IActionResult> Post(Policy value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                    
                }
                value.is_active = true;
                value.created_date= DateTime.Now;
                var val = await policyData.Insert(value);
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

        // PUT api/<PolicyController>/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 27 } })]
        public async Task<IActionResult> Put(int id, Policy value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                   
                }
                value.id = id;
                var dept = await policyData.GetById(id);
                if (dept == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Department not found or internal error", Code = 404 });
                }
                dept.policy_type = value.policy_type;
                
                dept.policy = value.policy;
                var valnew = await policyData.Update(dept);
                if (valnew != null)
                {
                    return StatusCode(200, valnew);
                  
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
        [Route("active/{id:int}")]
        [HttpPut]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 27 } })]
        public async Task<IActionResult> ActiveToggle(int id, [FromBody] bool value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                   
                }
                var emp = await policyData.GetById(id);
                if (emp == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Employee not found or internal error ", Code = 404 });
                }
                emp.is_active = value;
                var result1 = await policyData.Update(emp);
                if (result1 != null)
                {
                    return StatusCode(200, new {result1});
                    
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "activation or deactivation  Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
