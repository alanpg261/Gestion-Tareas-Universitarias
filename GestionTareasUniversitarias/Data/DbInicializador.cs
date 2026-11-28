using Microsoft.AspNetCore.Identity;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Data;

public static class DbInicializador
{
    public const string RolEstudiante = "Estudiante";
    public const string RolAdministrador = "Administrador";

    public const string ClaveDemo = "Temporal123!";

    public static async Task InicializarAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var rol in new[] { RolEstudiante, RolAdministrador })
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }

        await CrearUsuarioAsync(userManager, "admin@ejemplo.com", RolAdministrador);
        await CrearUsuarioAsync(userManager, "estudiante@ejemplo.com", RolEstudiante);
    }

    private static async Task CrearUsuarioAsync(UserManager<ApplicationUser> userManager, string correo, string rol)
    {
        if (await userManager.FindByEmailAsync(correo) != null)
        {
            return;
        }

        var usuario = new ApplicationUser
        {
            UserName = correo,
            Email = correo,
            EmailConfirmed = true
        };

        var resultado = await userManager.CreateAsync(usuario, ClaveDemo);
        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(usuario, rol);
        }
    }
}
