#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Garage.Areas.Admin.Pages.Roles;

[Authorize(Policy = "RequireAdmin")]
public class CreateModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public CreateModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        public string Name { get; set; }
    }

    public async Task<IActionResult> OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var nameExists = await _roleManager.RoleExistsAsync(Input.Name);
        if (nameExists)
        {
            ModelState.AddModelError(nameof(Input) + "." + nameof(Input.Name), "The role name already exists.");
            return Page();
        }
        await _roleManager.CreateAsync(new IdentityRole(Input.Name));
        return RedirectToPage("./List");
    }
}