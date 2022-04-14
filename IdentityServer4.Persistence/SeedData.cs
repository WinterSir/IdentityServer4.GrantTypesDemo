using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace IdentityServer4.Persistence
{
    public class SeedData
    {
        public static void InitData(IApplicationBuilder serviceProvider)
        {
            using (var scope = serviceProvider.ApplicationServices.CreateScope())
            {
                //初始化种子数据：配置、资源、客户端等
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                {
                    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    InitSeedData(context);
                }
                //初始化种子数据：用户
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    foreach (var user in InMemoryConfig.GetTestUser())
                    {
                        var find = userManager.FindByNameAsync(user.Username).Result;
                        if (find == null)
                        {
                            IdentityUser u = new IdentityUser() { UserName = user.Username };
                            //密码格式严格（至少一个非字母字符、至少一位0-9数字）
                            var ret = userManager.CreateAsync(u, "WinterSir123!").Result;
                            if (ret.Succeeded)
                            {
                                userManager.AddClaimsAsync(u, user.Claims);
                            }
                        }
                    }
                }
            }
        }
        private static void InitSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in InMemoryConfig.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in InMemoryConfig.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in InMemoryConfig.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in InMemoryConfig.GetApiScopes())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
