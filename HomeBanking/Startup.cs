using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeBanking
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
            services.AddRazorPages();

            //Agregamos el Controller
            services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            //Agregamos el contexto de la base de datos
            services.AddDbContext<HomeBankingContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HomeBankingConnection")));

            //Agregamos el scoped de ClientRepository
            services.AddScoped<IClientRepository, ClientRepository>();
            //Agregamos el scoped de AccountRepository
            services.AddScoped<IAccountRepository, AccountRepository>();
            //Agregamos el scoped de CardRepository
            services.AddScoped<ICardRepository, CardRepository>();
            //Agregamos el scoped de TransactionRepository
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //Autenticacion
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.LoginPath = new PathString("/index.html");
                });

            //Autorizaci�n
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();


                //
                //app.UseEndpoints(endpoints =>
                //{
                //    endpoints.MapRazorPages();
                //    endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=games}/{ action = Get}");
                //});
            });
        }
    }
}
