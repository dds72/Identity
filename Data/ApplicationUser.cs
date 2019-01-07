using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data
{
  public class ApplicationUser : IdentityUser
  {
    public bool IsAdmin { get; set; } = false;
  }
}
