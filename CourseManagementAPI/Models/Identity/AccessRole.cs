using Microsoft.AspNetCore.Identity;

namespace CourseManagementAPI.Models;

public class AccessRole : IdentityRole<Guid>
{
    // do mesmo jeito que o identity user já tem caracteristicas esse aq ja tem roles pre setadas
}