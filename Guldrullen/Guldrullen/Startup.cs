using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Guldrullen.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Guldrullen
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Guldrullen database
            var connString = @"Data Source=guldrullen.database.windows.net;Initial Catalog=Guldrullen;Integrated Security=False;User ID=teamguldrullen;Password=Qk7Kagr4bb4r;Connect Timeout=15;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<GuldrullenContext>(o => o.UseSqlServer(connString));

            // User database
            var connStringUsers = @"Data Source=guldrullen.database.windows.net;Initial Catalog=GuldrullenUsers;Integrated Security=False;User ID=teamguldrullen;Password=Qk7Kagr4bb4r;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connStringUsers));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Cookies.ApplicationCookie.LoginPath = "/users/login";
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();


            services.AddMvc();
            services.AddSession();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseIdentity();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();
        }
    }
}
