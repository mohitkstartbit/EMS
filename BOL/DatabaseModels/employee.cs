using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "employee", Schema = "public")]
    public class Employee
    {
        [Key]
        public int employee_id { get; set; }
        [Required]
        public string firstname { get; set; }
        public string? middlename { get; set; }
        [Required]
        public string lastname { get; set; }
        public string? personalemail { get; set; }
        public string? officeemail { get; set; }
        public string? skypeid { get; set; }
        public string? currentaddressline1 { get; set; }
        public string? currentaddressline2 { get; set; }

        public string? currentaddressline3 { get; set; }
        public string? currentcity { get; set; }
        public string? current_zip { get; set; }

        public string? currentstate { get; set; }
        public string? permanentaddreaaline1 { get; set; }
        public string? permanentaddreaaline2 { get; set; }
        public string? permanentaddreaaline3 { get; set; }
        public string? city { get; set; }
        public string? permanent_zip { get; set; }
        public string? state { get; set; }
        public string? phone { get; set; }

        public bool? marital_status { get; set; }
        public string? spouse_name { get; set; }
        public string? alternatephone { get; set; }
        public DateTime? date_of_marriage { get; set; }
        public DateTime? date_of_resignation { get; set; }
        public string pfno { get; set; }
        public DateTime? doj { get; set; }
        public DateTime? dob { get; set; }
        [Required]
        public int designation_id { get; set; }

        [ForeignKey("designation_id ")]

        public virtual Designation designations { get; set; }
        [Required]
        public int department_id { get; set; }

        [ForeignKey("department_id  ")]
        public virtual Department departments { get; set; }
        [Required]
        public int role_id { get; set; }

        [ForeignKey("role_id  ")]
        public virtual Role roles { get; set; }
        [Required]
        public int shift_id { get; set; }

        [ForeignKey("shift_id")]
        public virtual Shift shifts { get; set; }

        public bool is_active { get; set; }
        public string? emergencycontact { get; set; }


        public string? emergencycontactperson { get; set; }
        public string? bloodgroup { get; set; }
        public string? personrelation { get; set; }



        public string? fathername { get; set; }
        public string? mothername { get; set; }
        public string? pan { get; set; }
        public string? aadhar { get; set; }
        public string? uanno { get; set; }
        public string? esino { get; set; }
        public bool? gratuity { get; set; }
        public string? cancelcheque { get; set; }
        public string? pp_photo { get; set; }
        public string? workmode { get; set; }
        public string? allergy_diseases { get; set; }
        public DateTime created_date { get; set; }
    }
    }
