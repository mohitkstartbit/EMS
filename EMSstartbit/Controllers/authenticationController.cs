using EMSstartbit.TokenAuthentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL;
using System.Threading;
using BOL.ApiModels;
using BOL.Responses;

namespace EMSstartbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authenticationController : ControllerBase
    {
        private readonly ITokenManager tokenManager;
        private readonly IuserPermissionData _userpermissiondata;
        private readonly IloginData logindata;
        private readonly IemployeeData employeedata;


        public authenticationController(ITokenManager tokenManager, IuserPermissionData updata, IloginData logindata, IemployeeData employeedata)
        {
            this.tokenManager = tokenManager;
            _userpermissiondata = updata;
            this.logindata = logindata;
            this.employeedata = employeedata;
        }

        [HttpPost, Route("login")]

        public async Task<IActionResult> Login(AuthModel au)
        {
            try
            {
                if (!ModelState.IsValid || au is null)
                {
                    return StatusCode(400, new statusResponse
                    {
                        Message = "Invalid Input",
                        Code = 400
                    });
                }
                var result = await tokenManager.Authenticate(au);

                if (result.status.Code == 200)
                {

                    var logvalue = await logindata.getByEid(result.employedata.employee_id);
                    if (logvalue == null)
                    {
                        return StatusCode(500, new statusResponse { Code = 500, Message = "Internal Error" });
                    }
                    var Token = await tokenManager.NewToken(logvalue.employee_id.ToString());
                    var refreshtoken = await tokenManager.GenerateRefreshToken();
                    logvalue.refreshtoken = refreshtoken;
                    logvalue.refreshtokenexpires = DateTime.Now.AddHours(7);
                    var logupdateresult = await logindata.Update(logvalue);
                    if (logvalue == null)
                    {
                        return StatusCode(500, new statusResponse { Code = 500, Message = "Internal Error" });
                    }
                    Response.Cookies.Append("X-Username", logupdateresult.employee_id.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddDays(1d) });
                    Response.Cookies.Append("X-Refresh-Token", logupdateresult.refreshtoken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddDays(1d) });
                    return Ok(new { Token, result.permissionlist, result.employedata });
                }
                else
                {
                    return StatusCode(result.status.Code, result.status);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse
                {
                    Message = ex.Message,
                    Code = 500
                });
            }

        }
        [HttpGet]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshtoken)))
                { return StatusCode(400, new statusResponse { Message = "Invalid client request", Code = 400 }); }
                var user = await logindata.getByRefreshtoken(refreshtoken, Convert.ToInt32(userName));
                if (user == null)
                {
                    return StatusCode(400, new statusResponse { Message = "Invalid Refresh Token", Code = 400 });
                }
                if (user.refreshtokenexpires <= DateTime.Now || user.refreshtokenexpires == null)
                {
                    return StatusCode(400, new statusResponse { Message = " Refresh Token Expired", Code = 400 });
                }
                var permissionlist = await _userpermissiondata.GetByEid(user.employee_id);
                var employedata = await employeedata.GetById(user.employee_id);
                if (employedata == null)
                {
                    return StatusCode(500, new statusResponse { Code = 500, Message = "Internal Error" });
                }
                var Token = await tokenManager.NewToken(user.employee_id.ToString());
                refreshtoken = await tokenManager.GenerateRefreshToken();
                user.refreshtoken = refreshtoken;
                var logupdateresult = await logindata.Update(user);
                if (user == null)
                {
                    return StatusCode(500, new statusResponse { Code = 500, Message = "Internal Error" });
                }
                Response.Cookies.Append("X-Username", logupdateresult.employee_id.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddDays(1d) });
                Response.Cookies.Append("X-Refresh-Token", logupdateresult.refreshtoken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddDays(1d) });

                return Ok(new { Token, permissionlist, employedata });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse
                {
                    Message = ex.Message,
                    Code = 500
                });
            }
        }
        [HttpGet]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshtoken)))
            { return StatusCode(400, new statusResponse { Message = "Invalid client request", Code = 400 }); }
            var user = await logindata.getByRefreshtoken(refreshtoken, Convert.ToInt32(userName));
            if (user == null)
            {
                return StatusCode(400, new statusResponse { Message = "Invalid Refresh Token", Code = 400 });
            }
            user.refreshtoken = null;
            user.refreshtokenexpires = null;
            var logupdateresult = await logindata.Update(user);
            if (logupdateresult == null)
            {
                return StatusCode(500, new statusResponse { Code = 500, Message = "Internal Error" });
            }
            Response.Cookies.Append("X-Username", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddDays(-1d) });
            Response.Cookies.Append("X-Refresh-Token", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddDays(-1d) });

            return Ok();

        }
        [HttpPut]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(Changepassword Changepass)

        {
            try
            {
                if (Changepass.id == 0)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var val = await logindata.getByEid(Changepass.id);
                if (Changepass.newpassword == Changepass.confirmpassword)
                {
                    if (val != null)
                    {
                        if (val.password == Changepass.oldpassword)
                        {
                            val.password = Changepass.newpassword;
                            await logindata.Update(val);
                            return StatusCode(200, val);
                        }
                        else
                        {
                            return StatusCode(404, new statusResponse { Message = "old password dosen't match", Code = 404 });
                        }
                    }
                    else
                    {
                        return StatusCode(404, new statusResponse { Message = "Data Not Found", Code = 404 });
                    }
                }
                else
                {
                    return StatusCode(404, new statusResponse { Message = "new password and confirm password not matched", Code = 404 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

    }
}
