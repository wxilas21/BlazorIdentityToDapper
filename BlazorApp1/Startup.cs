using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorApp1.Data;
using Microsoft.AspNetCore.Identity;
using BlazorApp1.Models;
using BlazorApp1.Stores;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddTransient<RoleStore>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddSingleton<WeatherForecastService>();

            //Microsoft.AspNetCore.Authorization.Policy.IPolicyEvaluator
            //Microsoft.AspNetCore.Authorization.Policy.PolicyEvaluator

            //Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminClaim", policy => policy.RequireClaim("AdminClaim"));
                options.AddPolicy("RequireNormalUserRole", policy => policy.RequireRole("NormalUser"));
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });
        }

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

            //  Required for authentication.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
