// src/Infrastructure/DependencyInjection.cs
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Database=SourcingAndTendering;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(conn));

        services
            .AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
        return services;
    }
}

public interface IDatabaseSeeder
{
    Task SeedAsync(CancellationToken ct = default);
}

public class DatabaseSeeder(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    : IDatabaseSeeder
{
    private static readonly string[] Roles = new[] { "Procurement", "Supplier", "Admin" };

    public async Task SeedAsync(CancellationToken ct = default)
    {
        // Roles
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Seed users
        async Task EnsureUserAsync(string email, string fullName, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = fullName
                };
                await userManager.CreateAsync(user, "P@ssw0rd!");
                await userManager.AddToRoleAsync(user, role);
            }
        }

        await EnsureUserAsync("admin@udc.qa", "System Admin", "Admin");
        await EnsureUserAsync("procurement@udc.qa", "Procurement User", "Procurement");
        await EnsureUserAsync("supplier@udc.qa", "Supplier User", "Supplier");
    }
}
