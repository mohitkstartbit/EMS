    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;
using BOL;
using BOL.Responses;

namespace BAL
    {
    public class emailControlData : IemailControlData
    {
        public static string OTP = "";
        public static int forgetuserid = 0;
        private IloginData _lodata;

        private IemployeeData _ldata;
        private IEmailData _Edata;
        public emailControlData(IemployeeData ldata ,IEmailData Edata, IloginData lodata)
        {
            _lodata = lodata;
            _ldata = ldata;
            _Edata = Edata;
        }
        public async Task<statusResponse> resetPassword(employee emp)
        {
            var emailtemp = await _Edata.GetByEmailType("Reset Password By Admin");
            var template = emailtemp.emailtemplate;
            var username=emp.firstname+" "+emp.middlename+" "+emp.lastname;
            var loginobj =await  _lodata.getByEid(emp.employee_id);
            if(loginobj == null)
            {
                return new statusResponse { Code = 404, Message = "Login Not Found" };
            }
            var Password = Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-"));
            
            loginobj.password = Password;
            var updatelogin = await _lodata.Update(loginobj);
            if(updatelogin == null)
            {
                return new statusResponse { Code = 500, Message = "Password update Failed" };
            }
            var subject = "Reset password Request";

            string body = template.Replace("{username}", username).Replace("{employeeid}", emp.employee_id.ToString()).Replace("{password}", Password);
            SendEmail(emp.officeemail, body, subject);
            return new statusResponse { Code = 200, Message = "Password Reset Successfull" };
        }
        public async Task<statusResponse> CheckId(string id)
        {
            var emailtemp = await _Edata.GetByEmailType("OTP For Forget Password");
            var template = emailtemp.emailtemplate;
           
            var isdigit = int.TryParse(id, out _);
            var otp = GeneratePassword().ToString();
            OTP = otp;
            var subject = "Ems forget password otp.";
         
            string body = template.Replace("{otp}", otp);
            //string body="<div style ="+"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2"+"><div style ="+ "margin:50px auto;width:70%;padding:20px 0"+"><div style = "+"border-bottom:1px solid #eee"+ "><a href = '' style = "+"font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600" + "> Startbit IT Solutions Pvt. Ltd.</a></div><p style = " + "font-size:1.1em" + "> Dear Team Member,</p><p> Use The Following OTP To Reset Your Password.</p><h2 style = " + "background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;" +">"+otp+"</h2><p style = "+"font-size:0.9em;"+ " > Thanks & Regards<br/> Startbit IT Solutions Pvt. Ltd<br/>(An ISO 27001 / 9001 Certified Company) </p><hr style =" + "border:none;border-top:1px solid #eee"+" /></div></div>";

            if (id.Contains('@') && id.Contains('.'))
            {
                var user = await _ldata.GetByEmailId(id);
                if(user == null)
                {
                    return new statusResponse { Code=404, Message = "User Not Found" };
                }
                var useremail = user.officeemail;
                forgetuserid = user.employee_id;
                SendEmail(useremail, body, subject);
                return new statusResponse { Code = 200, Message = "OTP sent Successfully" };


            }
            else if (isdigit)
            {
                var user = await _ldata.GetById(Convert.ToInt32(id));
                if (user == null)
                {
                    return new statusResponse { Code = 404, Message = "User Not Found" };
                }
                var useremail = user.officeemail;
                forgetuserid = user.employee_id;
                SendEmail(useremail, body, subject);
                return new statusResponse { Code = 200, Message = "OTP sent Successfully" };
            }
            else
            {
                return new statusResponse { Code = 400, Message = "Entered Email/Id is not valid" };
            }
        }
        public async Task<statusResponse> ChangePassword(forget fo)
        {
            if (fo.otp == OTP)
            {

                if (fo.newpassword == fo.confirmpassword)
                {
                    var user = await _lodata.getByEid(forgetuserid);
                    if(user == null)
                    {
                        return new statusResponse { Code = 404, Message = "User Not Found" };
                    }
                    user.password = fo.newpassword;
                    await _lodata.Update(user);
                    return new statusResponse { Code = 200, Message = "Changed Successfully" };

                }
                else
                {
                    return new statusResponse { Code = 400, Message = "new password and confirm password not matched!" };

                }
            }
            else
            {
                return new statusResponse { Code = 400, Message = "Invalid OTP!" };
            }
        }
        private string GeneratePassword()
        {
            int PasswordLength = 6;
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";


            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i <PasswordLength; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }
            return NewPassword;
        }
        public  statusResponse SendEmail(string emailAddress, string body, string subject)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("supportntest@gmail.com");
                    mail.To.Add(emailAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;


                    using (SmtpClient smtp = new SmtpClient("smtp-relay.sendinblue.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential("", "");
                        smtp.Send(mail);
                    }
                }
                return new statusResponse { Code = 200, Message = "Mail sent Success fully" };

            }
            catch(Exception ex)
            {
                return new statusResponse { Code = 500, Message = ex.Message };
            }
        }
      
    }
}


