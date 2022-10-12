using BAL;
using BOL;
using EMSstartbit.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using BOL.Responses;
using BOL.DerivedClasses;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.AccessControl;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMSstartbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class employeeController : ControllerBase

    {
    
        private readonly IemployeeData employeedata;
        private readonly IrolePermissionData rolepdata;
        private readonly IuserPermissionData userpdata;
        private readonly IloginData logindata;
        private readonly IEmailData emaildata;
        private readonly IemailControlData emailcontrol;
        public employeeController(IemailControlData emailcontrol,IEmailData emaildata,IemployeeData employeedata, IrolePermissionData rolepdata, IuserPermissionData userpdata, IloginData logindata)
        {
            this.emailcontrol = emailcontrol;
            this.employeedata = employeedata;
            this.rolepdata = rolepdata;
            this.userpdata = userpdata;
            this.logindata = logindata;
            this.emaildata = emaildata;
        }
        // GET: api/<employeeController>
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 14 } })]
        public async Task<IActionResult> Get()
        {
            try
            {
                var val = await employeedata.GetAll();
                if (val.Count() != 0 && val != null)
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

        // GET api/<employeeController>/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 14,1 } })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                  
                }
                var val = await employeedata.GetById(id);
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
                return StatusCode(500, new statusResponse {Message = ex.Message, Code = 500 });
            }
        }
        [Route("Email/{Eid}")]
        [HttpGet]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 14, 1 } })]
        public async Task<IActionResult> Email(string Eid)
        {
            try
            {
                if (Eid == "")
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                  
                }
                var val = await employeedata.GetByEmailId(Eid);
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

        // POST api/<employeeController>
        [HttpPost]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] employeeFiles empfiles)
        {
            try
            {
                if (!ModelState.IsValid || empfiles.panfile.Count() == 0 || empfiles.aadharfile.Count() == 0 || empfiles.ppfile.Count() == 0 || empfiles.cancelcheckfile.Count() == 0)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                    
                }
                var checkemployeeid = await employeedata.GetById(empfiles.employee_id);
                if (checkemployeeid != null)
                {
                    return StatusCode(409, new statusResponse { Message = "Employee Id already exists", Code = 409 });
                }
                var panresult = await AddFile(empfiles.panfile.ToList(), empfiles.employee_id, "pan");
                var cancelcheckresult = await AddFile(empfiles.cancelcheckfile.ToList(), empfiles.employee_id, "cancelcheck");
                var aadharresult = await AddFile(empfiles.aadharfile.ToList(), empfiles.employee_id, "aadhar");
                var ppresult = await AddFile(empfiles.ppfile.ToList(), empfiles.employee_id, "pp");
                if (panresult == "false" || cancelcheckresult == "false" || aadharresult == "false" || ppresult == "false")
                {
                    return StatusCode(500, new statusResponse { Message = "Employee Files Insert Failed", Code = 500 });
                }
                empfiles.pan = panresult;
                empfiles.pp_photo = ppresult;
                empfiles.aadhar = aadharresult;
                empfiles.cancelcheque = cancelcheckresult;
                var value = await employeedata.GetEmployeefromEmpFiles(empfiles);
                if (value == null)
                {
                    return StatusCode(500, new statusResponse { Message = "Employee Files Insert Failed", Code = 500 });
                }
                value.is_active = true;
                value.created_date = DateTime.Now;
                value.date_of_resignation = null;
                var PermissionsList = await rolepdata.GetByroleid(value.role_id);

                var val = await employeedata.Insert(value);
                if (val == null)
                {
                    return StatusCode(500, new statusResponse { Message = "Employee Insert Failed", Code = 500 });
                }
                List<UserPermission> Ups = new List<UserPermission>();
                foreach (var el in PermissionsList)
                {
                    UserPermission up = new UserPermission();
                    up.employee_id = val.employee_id;
                    up.role_id = val.role_id;
                    up.permission_id = el.permission_id;
                    Ups.Add(up);
                }
                await userpdata.InsertMultiple(Ups);
                var login = await logindata.Insert(new login
                {
                    employee_id = val.employee_id,
                    created_date = val.created_date,
                    password = Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-")),
                    role_id = val.role_id,
                    is_active = val.is_active
                });
                if (login == null)
                {
                    return StatusCode(500, new statusResponse { Message = "Login Insert Failed", Code = 500 });
                }
                var emailtemp = await emaildata.GetByEmailType("New  User Password Send");
                var template = emailtemp.emailtemplate;
                var subject = "Welcome to Startbit It Solutions Pvt. Ltd.";
                string body = template.Replace("{username}", val.firstname + " " + val.middlename + " " + val.lastname).Replace("{employeeid}", val.employee_id.ToString()).Replace("{password}", login.password);
          
                emailcontrol.SendEmail(val.officeemail.ToLower(), body, subject);
                return StatusCode(200, value);
                 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        async Task<string> AddFile(ICollection<IFormFile> files, int empid, string type)
        {
            try
            {
                await Task.Yield();
                var count = 0;
                foreach (var item in files)
                {
                    count += 1;
                    if (item == null || type == "")
                    { return "false"; }
                    string filename =  count.ToString()+ System.IO.Path.GetExtension(item.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Assests", "EmployeeDocuments", empid.ToString(),type);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        DirectoryInfo info = new DirectoryInfo(path);
                        DirectorySecurity security = info.GetAccessControl();
                        security.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        security.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        info.SetAccessControl(security);
                    }
                    var newpath = Path.Combine(path, filename);
                    await item.CopyToAsync(new FileStream(newpath, FileMode.Create));
                 
                }
                return count.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "false";
            }
        }

         bool DeleteFile(int empid,string filename)
        {
            try
            {
                if ( filename == "")
                {
                    return false;
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Assests", "EmployeeDocuments", empid.ToString(),filename);
                if (Directory.Exists(path))
                {
                    var files =new DirectoryInfo(path).GetFiles();
                    foreach(var fl in files)
                    {
                        fl.Delete();
                    }
                }
                return true;
                }
            catch(Exception e)
            {
                return false;
            }
        }
        // PUT api/<employeeController>/5
        // [Route("update/{id:int}")]
        [HttpPut("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Put(int id, [FromForm] employeeFiles empfiles)
         {
            try
            {
                if (!ModelState.IsValid || id != empfiles.employee_id)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var value = await employeedata.GetById(empfiles.employee_id);
          
                if (value == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Employee doesn't exists", Code = 404 });
                }
                int oldrole = value.role_id;
                var panresult = "";
                var cancelcheckresult = "";
                var aadharresult = "";
                var ppresult = "";
                if (empfiles.panfile.Count() != 0)
                {
                    var pp= DeleteFile(value.employee_id, "pan");
                    panresult = await AddFile(empfiles.panfile.ToList(), empfiles.employee_id, "pan");
                }
                else
                {
                    panresult = value.pan;
                }
                if (empfiles.cancelcheckfile.Count() != 0)
                {
                     DeleteFile(value.employee_id, "cancelcheck");
                    cancelcheckresult = await AddFile(empfiles.cancelcheckfile.ToList(), empfiles.employee_id, "cancelcheck");
                }
                else
                {
                    cancelcheckresult = value.cancelcheque;
                }
                if (empfiles.aadharfile.Count() != 0)
                {
                     DeleteFile(value.employee_id, "aadhar");
                    aadharresult = await AddFile(empfiles.aadharfile.ToList(), empfiles.employee_id, "aadhar");
                }
                else
                {
                    aadharresult = value.aadhar;
                }
                if (empfiles.ppfile.Count() != 0)
                {
                     DeleteFile(value.employee_id, "pp");
                    ppresult = await AddFile(empfiles.ppfile.ToList(), empfiles.employee_id, "pp");
                }
                else
                {
                    ppresult = value.pp_photo;
                }
                if (panresult == "false" || cancelcheckresult == "false" || aadharresult == "false" || ppresult == "false")
                {
                    return StatusCode(500, new statusResponse { Message = "Employee Files Insert Failed", Code = 500 });
                }
                value.firstname = empfiles.firstname;
                value.middlename = empfiles.middlename;
                value.lastname = empfiles.lastname;
                value.personalemail = empfiles.personalemail;
                value.dob = empfiles.dob;
                value.bloodgroup = empfiles.bloodgroup;
                value.currentaddressline1 = empfiles.currentaddressline1;
                value.currentaddressline2 = empfiles.currentaddressline2;
                value.currentaddressline3 = empfiles.currentaddressline3;
                value.currentcity = empfiles.currentcity;
                value.current_zip = empfiles.current_zip;
                value.currentstate = empfiles.currentstate;
                value.permanentaddreaaline1 = empfiles.permanentaddreaaline1;
                value.permanentaddreaaline2 = empfiles.permanentaddreaaline2;
                value.permanentaddreaaline3 = empfiles.permanentaddreaaline3;
                value.city = empfiles.city;
                value.permanent_zip = empfiles.permanent_zip;
                value.state = empfiles.state;
                value.phone = empfiles.phone;
                value.alternatephone = empfiles.alternatephone;
                value.officeemail = empfiles.officeemail;
                value.skypeid = empfiles.skypeid;
                value.doj = empfiles.doj;
                value.designation_id = empfiles.designation_id;
                value.department_id = empfiles.department_id;
                value.role_id = empfiles.role_id;
                value.shift_id = empfiles.shift_id;
                value.workmode = empfiles.workmode;
                value.gratuity = empfiles.gratuity;
                value.fathername = empfiles.fathername;
                value.mothername = empfiles.mothername;
                value.marital_status = empfiles.marital_status;
                if(value.marital_status == false)
                {
                    value.date_of_marriage = null;
                    value.spouse_name = null;
                }
                else
                {
                    value.date_of_marriage = empfiles.date_of_marriage;
                    value.spouse_name = empfiles.spouse_name;
                }
                value.emergencycontact = empfiles.emergencycontact;
                value.emergencycontactperson = empfiles.emergencycontactperson;
                value.personrelation = empfiles.personrelation;
                value.allergy_diseases = empfiles.allergy_diseases;
                value.pfno = empfiles.pfno;
                value.uanno = empfiles.uanno;
                value.esino = empfiles.esino;
                value.pan = panresult;
                value.pp_photo = ppresult;
                value.aadhar = aadharresult;
                value.cancelcheque = cancelcheckresult;
                value.date_of_resignation = null;
                if (value == null)
                {
                    return StatusCode(500, new statusResponse { Message = "Employee  Insert Failed", Code = 500 });
                }
               
                var val = await employeedata.Update(value);
                if (val == null)
                {
                    return StatusCode(500, new statusResponse { Message = "Employee Update Failed", Code = 500 });
                }
                if (oldrole != empfiles.role_id)
                {
                    var userpermissionslist = await userpdata.GetAllUserPermisssionByEid(val.employee_id);
                    var oldrolePermissions = await rolepdata.GetPermissionIdsByroleid(oldrole);
                    var newrolePermissions = await rolepdata.GetPermissionIdsByroleid(empfiles.role_id);
                    var val23 = await userpdata.DeleteByEmployeeAndPermissionids(empfiles.employee_id, oldrolePermissions);

                    var newuserpermissionslist = await userpdata.GetAllUserPermisssionByEid(val.employee_id);
                    foreach (var el in newuserpermissionslist)
                    {
                        el.role_id = val.role_id;
                        await userpdata.Update(el);
                    }

                    List<UserPermission> userPermissions = new List<UserPermission>();
                    foreach (var el in newrolePermissions)
                    {
                        UserPermission up = new UserPermission();
                        up.employee_id = empfiles.employee_id;
                        up.role_id = empfiles.role_id;
                        up.permission_id = el;
                        userPermissions.Add(up);
                    }
                    var val234 = await userpdata.InsertMultiple(userPermissions);
                 
                    //Login role id change
                    var oldlogin = await logindata.getByEid(val.employee_id);
                    oldlogin.role_id = val.role_id;
                    var newlogin = await logindata.Update(oldlogin);
                    if (newlogin == null)
                    {
                        return StatusCode(500, new statusResponse { Message = "Login Update Failed", Code = 500 });
                    }
                }
                return StatusCode(200,value);
                

            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }

        // DELETE api/<employeeController>/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var val = await employeedata.Delete(id);
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
        // PUT api/<employeeController>/active/
        [Route("active/{id:int}")]
        [HttpPut]
       [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> Putactive(int id,[FromBody] bool value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var emp = await employeedata.GetById(id);
                if(emp== null)
                {
                    return StatusCode(404,new statusResponse { Message = "Employee not found or internal error ", Code = 404});
                }
                emp.is_active = value;
                var result1 = await employeedata.Update(emp);
                var loginvalue = await logindata.getByEid(emp.employee_id);
                if (loginvalue == null)
                {
                    return StatusCode(404,new statusResponse { Message = "Login not found or internal error ",Code = 404 });
                }
                loginvalue.is_active = value;
                var result2 = await logindata.Update(loginvalue);
                if (result1 != null && result2 != null)
                {
                    return StatusCode(200, new { result1, result2 });
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
        // PUT api/<employeeController>/profileupdate/5
        [Route("profileupdate/{id:int}")]
        [HttpPut]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 1 } })]
        public async Task<IActionResult> profileupdate(int id,employee emp)
        {
            try
            {
                if (!ModelState.IsValid || id != emp.employee_id)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                }
                var value = await employeedata.GetById(emp.employee_id);
                if (value == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Employee doesn't exists", Code = 404 });
                }
                value.dob = emp.dob;
                value.currentaddressline1 = emp.currentaddressline1;
                value.currentaddressline2 = emp.currentaddressline2;
                value.currentaddressline3 = emp.currentaddressline3;
                value.currentcity = emp.currentcity; 
                value.currentstate = emp.currentstate; 
                value.current_zip = emp.current_zip; 
                value.permanentaddreaaline1 = emp.permanentaddreaaline1; 
                value.permanentaddreaaline2 = emp.permanentaddreaaline2; 
                value.permanentaddreaaline3 = emp.permanentaddreaaline3; 
                value.state = emp.state; 
                value.city = emp.city;
                value.permanent_zip = emp.permanent_zip;
                value.phone = emp.phone;
                value.alternatephone = emp.alternatephone;
                value.pfno = emp.pfno;
                var valuenew = await employeedata.Update(value);
                if (valuenew == null)
                {
                    return StatusCode(500, new statusResponse { Message = "Employee Update Failed", Code = 500 });
                }
                return StatusCode(200, valuenew);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
        [Route("employeeresign/{id:int}")]
        [HttpPut]
        [TypeFilter(typeof(TokenAuthenticationFilter), Arguments = new object[] { new int[] { 11 } })]
        public async Task<IActionResult> employeeResign(int id, [FromBody]  DateTime resigndate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new statusResponse { Message = "Bad Request", Code = 400 });
                    
                }
                var emp = await employeedata.GetById(id);
                if (emp == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Employee not found or internal error ", Code = 404 });
                }
                emp.is_active = false;
                emp.date_of_resignation = resigndate;
                var result1 = await employeedata.Update(emp);
                var loginvalue = await logindata.getByEid(emp.employee_id);
                if (loginvalue == null)
                {
                    return StatusCode(404, new statusResponse { Message = "Login not found or internal error ", Code = 404 });
                }
                loginvalue.is_active = false;
                var result2 = await logindata.Update(loginvalue);
                if (result1 != null && result2 != null)
                {
                    return StatusCode(200, new { result1, result2 });
                }
                else
                {
                    return StatusCode(500, new statusResponse { Message = "Resignaion  Failed", Code = 500 });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, new statusResponse { Message = ex.Message, Code = 500 });
            }
        }
    }
}
