using System;
using System.   Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BoardGameStore.Models;

namespace BoardGameStore
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
            Configuration.GetConnectionString("Product");
            string boardGameHubConnectionString = Configuration.GetConnectionString("BoardGameHub");

            //services.AddDbContext<IdentityDbContext>(opt => opt.UseInMemoryDatabase("Identities"));
            services.AddDbContext<BoardGameHubDbContext>(opt => opt.UseSqlServer(boardGameHubConnectionString));

            services.AddIdentity<BoardGameHubUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<BoardGameHubDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient((s) => {
                return new SendGrid.SendGridClient(Configuration.GetValue<string>("SendGridKey"));
            });
            services.AddTransient<Braintree.IBraintreeGateway>((s) => {
                return new Braintree.BraintreeGateway(
                    Configuration.GetValue<string>("BraintreeEnvironment"),
                    Configuration.GetValue<string>("BraintreeMerchantID"),
                    Configuration.GetValue<string>("BraintreePublicKey"),
                    Configuration.GetValue<string>("BraintreePrivateKey")
                );
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BoardGameHubDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //I need this to instruct my app to use Cookies for tracking Signin/SignOut status
            app.UseAuthentication();
            app.UseStaticFiles();
            db.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
   }
}
