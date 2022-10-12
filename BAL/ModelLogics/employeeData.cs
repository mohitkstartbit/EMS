using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Threading.Tasks;
using System.Linq;
using BOL.DerivedClasses;

namespace BAL
{
    public class employeeData : IemployeeData
    {
        private readonly IUnitOfWork _unitofwork;
        public employeeData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<employee>> GetAll()
        {
            await _unitofwork.roles.GetData();
            await _unitofwork.departments.GetData();
            await _unitofwork.designations.GetData();
            return await _unitofwork.employees.GetData();
        }
        public async Task<employee> Insert(employee u)
        {
            var result = await _unitofwork.employees.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<employee> Update(employee u)
        {
            var result = await _unitofwork.employees.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<employee> GetById(int id)
        {
            await _unitofwork.roles.GetData();
            await _unitofwork.departments.GetData();
            await _unitofwork.designations.GetData();
            var x = await _unitofwork.employees.GetByExpression(u => u.employee_id == id);
            return x;
        }
        public async Task<employee> GetByEmailId(string id)
        {
            var x = await _unitofwork.employees.GetByExpression(u => u.officeemail.ToLower() == id.ToLower());
            return x;
        }
        public async Task<employee> Delete(int id)
        {
            var result= await _unitofwork.employees.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<IEnumerable<int>> GetEmployeeIdswithRoleids(int roleid)
        {
            var x = await _unitofwork.employees.GetData();
            var result = x.Where(u => u.role_id == roleid).Select(f=>f.employee_id);
            return result;
        }
        public async Task<employee> GetEmployeefromEmpFiles(employeeFiles empfiles)
        {
            await Task.Yield();
            employee value = new employee();
            value.employee_id = empfiles.employee_id;
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
            value.date_of_marriage = empfiles.date_of_marriage;
            value.spouse_name = empfiles.spouse_name;
            value.emergencycontact = empfiles.emergencycontact;
            value.emergencycontactperson = empfiles.emergencycontactperson;
            value.personrelation = empfiles.personrelation;
            value.allergy_diseases = empfiles.allergy_diseases;
            value.pfno = empfiles.pfno;
            value.uanno = empfiles.uanno;
            value.esino = empfiles.esino;
            value.pan = empfiles.pan;
            value.aadhar = empfiles.aadhar;
            value.cancelcheque = empfiles.cancelcheque;
            value.pp_photo = empfiles.pp_photo;
            return value;
        }
    }
}
