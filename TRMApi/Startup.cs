using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TRMApi.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using TRMApi.Models;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Internal.DataAccess;
//using Microsoft.Open

namespace TRMApi
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //Personal Services
            services.AddTransient<IInventoryData, InventoryData>();
            services.AddTransient<IProductData, ProductData>();
            services.AddTransient<ISaleData, SaleData>();
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<ISQLDataAccess, SQLDataAccess>();

            //Configuration for taking tokens and making sure the user is authenticated and token has
            //not expired.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "jwtbearer";
                options.DefaultChallengeScheme = "jwtbearer";
            })
            .AddJwtBearer("jwtbearer", jwtbeareroptions =>
            {
                jwtbeareroptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Secrets:SecurityKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    //makes sure tolerance for time checks
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

            //Swagger Addition
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                    "V1",
                    new OpenApiInfo
                    {
                        Title = "TimCo Retail Manager API",
                        Version = "v1"
                    });
            });

            //Configuration to add authorization filter to every operation e.g get, post, put, delete
            services.ConfigureSwaggerGen(setup =>
            {
                setup.OperationFilter<AuthorizationOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //This is swagger configuration for swagger endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/V1/swagger.json", "TimCo API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
