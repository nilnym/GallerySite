using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GallerySite.Models;
using GallerySite.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GallerySite
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GallerySite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<GallerySiteContext>(options => options.UseSqlServer(connString));

            services.AddTransient<GalleryService>();
            services.AddTransient<FileService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();

            app.UseStaticFiles();
        }
    }
}
