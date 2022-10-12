using EMSstartbit.TokenAuthentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.EntityFrameworkCore;
using BAL;
using Microsoft.AspNetCore.Http;

namespace EMSstartbit.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public int[] permissionid { get; set; }
        public IuserPermissionData _userpermissiondata;
        private readonly IloginData _logindata;
        public bool check = true;

        public TokenAuthenticationFilter(IuserPermissionData pd,IloginData ldata,int[] perid)
        {
            this._userpermissiondata = pd;
            this.permissionid = perid;
            _logindata = ldata;
        }
      
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine(permissionid);
            var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));
            var result = true;
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                result = false;
            }
            string token = string.Empty;
            if (result)
            {
                try
                {
                    token = context.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value;
                    var employeeid = await tokenManager.VerifyToken(token);
                    if(employeeid == null)
                    {
                        throw new Exception("token Verification Failed");
                    }
                    Console.WriteLine(employeeid);
                    var logdata = await  _logindata.getByEid(Convert.ToInt32(employeeid));
                    if(logdata == null)
                    {
                        throw new Exception("Data Fetch Failed");
                    }
                    if (logdata.is_active == false)
                    {
                        throw new Exception("User is Locked");
                    }
                    var employeepermissions = await _userpermissiondata.GetByEid(Convert.ToInt32(employeeid));
                    var ids = employeepermissions.Select(x => x.permission_id).Intersect(permissionid.Select(x => x));

                    if (ids.Count() != 0)
                    {
                        check = false;
                    }
                    if (check != false)
                    {
                        throw new Exception("Permission Denied");
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    context.ModelState.AddModelError("Unauthorized", ex.ToString());
                }

            }
            if (!result)
            {
                context.Result = new UnauthorizedObjectResult(context.ModelState);
              
            }
        }
    }
}
