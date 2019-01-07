using IdentityServer.Data;
using IdentityServer.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
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
      services
        .AddDbContext<ApplicationDbContext>(
          options =>
            options.UseNpgsql(
              Configuration.GetConnectionString("DefaultConnection")));

      services
        .AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
          options.Password.RequireDigit = true;
          options.Password.RequiredLength = 6;
          options.Password.RequiredUniqueChars = 1;
          options.Password.RequireLowercase = true;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

      services.AddMvc();

      services.AddSingleton<IEmailSender, EmailSender>();

      services
        .AddIdentityServer(options =>
        {
          options.IssuerUri = Configuration["IdentityServer:Issuer"];
        })
        .AddDeveloperSigningCredential()
        .AddInMemoryPersistedGrants()
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiResources(Config.GetApiResources())
        .AddInMemoryClients(Config.GetClients())
        .AddProfileService<ProfileService>()
        .AddAspNetIdentity<ApplicationUser>();

      services.AddTransient<IProfileService, ProfileService>();
    }

    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      ApplicationDbContext dbContext)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseIdentityServer();

      app.UseMvc();

      dbContext.Database.EnsureCreated();
    }
  }
}
