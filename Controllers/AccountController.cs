using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
  [Produces("application/json")]
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IConfiguration configuration)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _configuration = configuration;
    }

    [HttpPost]
    public async Task<object> Login([FromBody] UserLogin model)
    {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

      if (result.Succeeded)
      {
        var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
        return Ok();
      }

      return BadRequest();
    }

    [HttpPost]
    public async Task<object> Register([FromBody] UserRegister model)
    {
      var user = new ApplicationUser
      {
        UserName = model.Email,
        Email = model.Email
      };
      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, false);
        return Ok();
      }

      return BadRequest();
    }

    [HttpPost]
    public async Task<object> ChangeEmail(
      [FromBody] UserChangeEmail changeEmail)
    {
      var user = _userManager.Users.SingleOrDefault(
        s => s.Email == changeEmail.CurrentEmail);

      if (user == null)
      {
        return NotFound();
      }

      user.Email = changeEmail.NewEmail;
      user.UserName = changeEmail.NewEmail;

      var result = await _userManager.UpdateAsync(user);

      if (result.Succeeded)
      {
        return Ok();
      }

      return BadRequest();
    }

    public class UserLogin
    {
      [Required]
      public string Email { get; set; }

      [Required]
      public string Password { get; set; }

    }

    public class UserRegister
    {
      [Required]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
      public string Password { get; set; }
    }

    public class UserChangeEmail
    {
      [Required]
      public string CurrentEmail { get; set; }

      [Required]
      public string NewEmail { get; set; }
    }
  }
}