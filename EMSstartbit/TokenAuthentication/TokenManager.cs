using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BAL;
using BOL.Responses;
using BOL.ApiModels;
using System.Security.Cryptography;

namespace EMSstartbit.TokenAuthentication
{
    public class TokenManager : ITokenManager
    {
        private JwtSecurityTokenHandler tokenHandler;
        private byte[] secretKey;
        private readonly IloginData _ldata;
        private readonly IemployeeData employeedata;
        private readonly IuserPermissionData userpermessiondata;
        public TokenManager(IloginData ldata, IemployeeData employeedata, IuserPermissionData userpermessiondata)
        {
            this.employeedata = employeedata;
            this.userpermessiondata = userpermessiondata;
            _ldata = ldata;
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyzabcdef");
        }

        public object Ecoding { get; }

        public async Task<AuthResponse> Authenticate(AuthModel au)
        {
            var isNumeric = int.TryParse(au.name, out _);
            var emp = (isNumeric) ? await employeedata.GetById(Convert.ToInt32(au.name)) : await employeedata.GetByEmailId(au.name);
           
            if (emp == null )
            {
                //error message employee not found 
                return new AuthResponse { status =new statusResponse {Code=404,Message= "employee is not Found or internal error" } , permissionlist = null, employedata = null };
            }
            var loginvalue = await _ldata.getByEid(emp.employee_id);
            if (loginvalue == null)
            {
                //error message employee not found 
                return new AuthResponse { status = new statusResponse { Code=404,Message= "user is not Found or internal error" }, permissionlist = null, employedata = null };
            }
            if (loginvalue.is_active == false)
            {
                return new AuthResponse { status=new statusResponse { Code= 403 ,Message= "User is not Active" } , permissionlist = null, employedata = null };
            }

            if (!string.IsNullOrWhiteSpace(au.password) && au.password == loginvalue.password)
            {
                var permessionlist = await userpermessiondata.GetByEid(emp.employee_id);
                //Authenticate success
                return new AuthResponse { status =new statusResponse {Code=200,Message= "Authenticated" }, permissionlist = permessionlist ,employedata = emp };
            }
            else
            {
                //Password doesn't not match
                return new AuthResponse { status = new statusResponse { Code=400,Message= "Password Doesn't Match" }, permissionlist = null ,employedata=null};


            }

        }
        public async Task<string> NewToken(string username)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, username) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //var JwtString = tokenHandler.WriteToken(token);
            //return  JwtString;
            return await Task.Run(() => tokenHandler.WriteToken(token));
        }
        public async Task<string> GenerateRefreshToken()
        {
            await Task.Yield();
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }


        }
        public async Task<string> VerifyToken(string token)
        {

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                //var accountId = jwtToken.Claims.First(x => x.Type == "email").Value;

                //return accountId;
                return await Task.Run(() => jwtToken.Claims.First(x => x.Type == "email").Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
