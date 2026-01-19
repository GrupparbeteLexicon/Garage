#nullable disable
using System.ComponentModel.DataAnnotations;
using Garage.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Garage.Areas.Admin.Pages.Roles;

[Authorize(Policy = "RequireAdmin")]
public class EditModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public EditModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [Display(Name = "Role Name")]
    public string RoleName { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public string Name { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string name)
    {
        var role = await _roleManager.FindByNameAsync(name);
        if (role == null)
            return NotFound();
        if (role.Name == UserRoles.Admin)
        {
            TempData["ErrorMessage"] = "Cannot change Admin role name.";
            return RedirectToPage("./List");
        }
        RoleName = role.Name;
        Input = new InputModel
        {
            Name = role.Name,
        };
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(string name)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var nameExists = await _roleManager.RoleExistsAsync(Input.Name);
        if (nameExists)
        {
            ModelState.AddModelError(nameof(RoleName), "The role name already exists.");
            return Page();
        }

        if (RoleName == UserRoles.Admin)
        {
            TempData["ErrorMessage"] = "Cannot change Admin role name.";
            return RedirectToPage("./List");
        }

        var role = await _roleManager.FindByNameAsync(name);
        if (role == null)
            return NotFound();
        await _roleManager.SetRoleNameAsync(role, Input.Name);
        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Role updated successfully.";
            return RedirectToPage("./List");
        }
        ModelState.AddModelError(nameof(RoleName), "Could not update role name.");
        return Page();
    }
}
