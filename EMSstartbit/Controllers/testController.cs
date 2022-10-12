using BAL;
using BOL;
using BOL.DerivedClasses;
using BOL.Responses;
using EMSstartbit.Filters;
using EMSstartbit.TokenAuthentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMSstartbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase

    {
        private readonly ItestData testdata;

        private readonly ITokenManager tkm;
        public testController(ItestData testdata ,ITokenManager tkm)
        {
            this.testdata = testdata;
            this.tkm = tkm;
        }
        // GET: api/<roleController>
        
        [HttpGet]
        public async Task<IActionResult> Get()
            {
            try
            {
                var val = await testdata.GetAll();
                if (val.Count() == 0 || val == null)
                {
                    var newRefreshToken =await  tkm.GenerateRefreshToken();
                    var cookieOptions = new CookieOptions
                    {
                  HttpOnly = true,
                        //Expires = DateTime.Now.AddDays(7),
                        SameSite= SameSiteMode.Unspecified,
                       Secure=true,
                        //IsEssential = true,
                        
                    };

                    Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);
                    return Ok();
                    // return StatusCode(200, val);
                   // return Ok(val);
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

        // GET api/<roleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode(400,new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var val = await testdata.GetById(id);
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

        // POST api/<roleController>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] testfile model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(404,new statusResponse { Message = "Bad Request", Code = 400 });
                }
                await Task.Yield();
                if (model == null || model.FileToUpload == null || model.FileToUpload.Length == 0)
                    return StatusCode(400,new statusResponse { Code = 400, Message = "file not selected" });

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Assests","EmployeeDocuments","1");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    DirectoryInfo info = new DirectoryInfo(path);
                    DirectorySecurity security = info.GetAccessControl();
                    security.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                    security.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                    info.SetAccessControl(security);
                }
                var newpath = Path.Combine(path, model.FileToUpload.FileName);
                await model.FileToUpload.CopyToAsync(new FileStream(newpath,FileMode.Create));
                if (model.FileToUpload != null)
                {
                    return StatusCode(200,new statusResponse { Code=200,Message= "true" });
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Role Insert Failed", Code = 500 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // PUT api/<roleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, test value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new statusResponse { Message = "Bad Request", Code = 400 });
                }

                value.id = id;
                var valnew = await testdata.Update(value);
                if (valnew != null)
                {
                    return StatusCode(200, valnew);
                   // return Ok(valnew);
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Role Update Failed", Code = 500});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // DELETE api/<roleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await testdata.Delete(id);
                if (val != null)
                {
                    return StatusCode(200, val);
                   // return Ok(val);
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
