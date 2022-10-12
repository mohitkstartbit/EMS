using System;
using Microsoft.EntityFrameworkCore;
using BOL;
using BOL.DatabaseModels;

namespace DAL
{
    public class EMSDataContext : DbContext
    {
        public EMSDataContext(DbContextOptions<EMSDataContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

          
        }   

        public virtual DbSet<login> logins { get; set; }
        public virtual DbSet<permission> permissionss { get; set; }
        public virtual  DbSet<role> roles { get; set; }
        public virtual DbSet<RolePermission> role_permissions { get; set; }
        public virtual DbSet<UserPermission> user_permissions { get; set; }
        public virtual DbSet<department> departments { get; set; }
        public virtual DbSet<designation> designations { get; set; }
        public virtual DbSet<employee> emlpoyees { get; set; }
        public virtual DbSet<shift> shifts { get; set; }
        public virtual DbSet<test> tests { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        


        //Masters
        public virtual DbSet<bloodgroup> bloodgroups { get; set; }
        public virtual DbSet<workmode> workmodes { get; set; }

    }
}
