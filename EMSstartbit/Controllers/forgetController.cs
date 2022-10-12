using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BAL;
using BOL;
using BOL.Responses;
using EMSstartbit.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMSstartbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class forgetController : ControllerBase
    {

        private readonly IemailControlData _iforgetdData;

        public readonly IemployeeData employeedata;

        public forgetController(IemailControlData iforgetdData,IemployeeData employeedata)
        {

            _iforgetdData = iforgetdData;
            this.employeedata = employeedata;
        }
        [Route("forgetpassword")]
        [HttpPost]

        public async Task<IActionResult> ForgetPassword(string id)
        {
            try
            {
                var message = await _iforgetdData.CheckId(id);
                if(message == null)
                {
                    throw new Exception("Internal Server Error");
                }
                return StatusCode(message.Code, new statusResponse { Message = message.Message, Code = message.Code});

            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
        [Route("confirmforgetpassword")]
        [HttpPost]
        public async Task<IActionResult> ConfirmForgetPassword(forget fo)
        {
            try
            {
                var message = await _iforgetdData.ChangePassword(fo);
                if (message == null)
                {
                    throw new Exception("Internal Server Error");
                }
                return StatusCode(message.Code, new statusResponse { Message = message.Message, Code = message.Code});

            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
        [Route("resetpassword/{id:int}")]
        [HttpPut]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]

        public async Task<IActionResult> resetpassword(int id)
        {
            try
            {
                var employeeobj =await employeedata.GetById(id);
                if(employeeobj == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Employee not found or internal error ", Code = 404 });
                }
                var message = await _iforgetdData.resetPassword(employeeobj);
                if (message == null)
                {
                    throw new Exception("Internal Server Error");
                }
                return StatusCode(message.Code, new statusResponse { Message = message.Message, Code = message.Code });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
