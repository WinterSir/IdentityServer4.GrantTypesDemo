using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace Hybrid
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
            //�ر�Jwtӳ��
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //ע����Ȩ
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5000";       //��֤����
                options.RequireHttpsMetadata = true;                //����ʹ��Https�������û��޷���¼
                options.ClientId = "Hybrid";
                options.ClientSecret = "Hybrid";
                options.ResponseType = "code id_token";
                options.Scope.Clear();
                options.Scope.Add("UserScope");
                options.Scope.Add("ProductScope");
                options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                options.Scope.Add(OidcConstants.StandardScopes.Profile);
                //options.Scope.Add(OidcConstants.StandardScopes.Email);
                //options.Scope.Add(OidcConstants.StandardScopes.Phone);
                //options.Scope.Add(OidcConstants.StandardScopes.Address);
                //options.Scope.Add(OidcConstants.StandardScopes.0fflineAccess);//��ȡ��ˢ��Token
                options.SaveTokens = true; //��ʾTokenҪ�洢
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
