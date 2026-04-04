using Microsoft.AspNetCore.Identity;

namespace CCSS_API.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public DateTime Birth_Date { get; set; }
}
