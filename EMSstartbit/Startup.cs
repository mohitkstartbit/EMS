using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DAL;
using EMSstartbit.TokenAuthentication;
using BAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;


namespace EMSstartbit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IloginData, loginData>();
            services.AddScoped<IpermissionData, permissionData>();
            services.AddScoped<IroleData, roleData>();
            services.AddScoped<IuserPermissionData, userPermissionData>();
            services.AddScoped<IrolePermissionData, rolePermissionData>();
            services.AddScoped<IemployeeData, employeeData>();
            services.AddScoped<IdesignationData, designationData>();
            services.AddScoped<IdepartmentData, departmentData>();
            services.AddScoped<IshiftData, shiftData>();
            services.AddScoped<ItestData, testData>();
            services.AddScoped<IemailControlData, emailControlData>();
            services.AddScoped<IPolicyData, PolicyData>();
            services.AddScoped<IEmailData, EmailData>();
            //Masters
            services.AddScoped<IbloodgroupData, bloodgroupData>();
            services.AddScoped<IworkmodeData, workmodeData>();

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Implement Swagger UI",
                    Description = "A simple example to Implement Swagger UI",
                });
            });
           
            services.AddCors(options => options.AddPolicy("Development", builder =>
             {
                 // Allow multiple HTTP methods  
                 builder.AllowAnyMethod().AllowAnyHeader()
                   .AllowCredentials().SetIsOriginAllowed(origin =>
                   {

                       if (string.IsNullOrWhiteSpace(origin)) return false;
                       if (origin.ToLower().StartsWith("http://localhost:3000")) return true;
                       if (origin.ToLower().StartsWith("http://192.168.1.85:3000")) return true;
                       if (origin.ToLower().StartsWith("http://192.168.1.83:5000")) return true;
                       if (origin.ToLower().StartsWith("http://192.168.1.82:3000")) return true;
                       return false;
                   });
             })
);


            services.AddDbContext<EMSDataContext>(options => { 
                options.UseNpgsql(Configuration.GetConnectionString("TestDb"));
               
            }, ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


           // app.UseCors("AllowOrigin");

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Showing API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
           app.UseCors("Development");
            // app.UseHttpsRedirection();
            app.UseCookiePolicy(
     new CookiePolicyOptions
     {
         Secure = CookieSecurePolicy.None
     });
            app.UseRouting();

            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Assests")),
                RequestPath = "/Assests"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
